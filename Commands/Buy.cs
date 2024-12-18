﻿using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

namespace ZaupShop.Commands
{
    public class Buy : IRocketCommand
    {
        public string Name => "buy";

        public string Syntax => "[v.]<name or id> [amount] [25 | 50 | 75 | 100]";

        public string Permission => null;

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Help => "Buy something";

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "zaupshop.buy" };
            }
        }

        List<string> IRocketCommand.Aliases
        {
            get { return new List<string>(); }
        }

        public async void Execute(IRocketPlayer caller, string[] command)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                UnturnedPlayer player = caller as UnturnedPlayer ?? null;
                if (player is null)
                {
                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("command_error_null")));
                    return;
                }
                //#region tarned
                //// In tarned context you can buy only in certain locations
                //var position = player.Position;
                //// Verify X position
                //if (position.x < 431.41 || position.x > 440.92)
                //{
                //    UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_not_safezone"));
                //    return;
                //}
                //// Verify Y position
                //else if (position.y < 51.00 || position.y > 54.00)
                //{
                //    UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_not_safezone"));
                //    return;
                //}
                //// Verifiy Z position
                //else if (position.z < 437.25 || position.z > 447.05)
                //{
                //    UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_not_safezone"));
                //    return;
                //}
                //#endregion

                if (command.Length == 0)
                {
                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_command_usage")));
                    return;
                }

                byte amttobuy = 1;
                if (command.Length > 1)
                {
                    if (!byte.TryParse(command[1], out amttobuy))
                    {
                        Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("invalid_amt")));
                        return;
                    }
                }

                string[] components = Parser.getComponentsFromSerial(command[0], '.');
                if (components.Length == 2 && components[0].Trim() != "v" || components.Length == 1 && components[0].Trim() == "v" || components.Length > 2 || components[0].Trim() == string.Empty)
                {
                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_command_usage")));
                    return;
                }
                switch (components[0])
                {
                    case "v":
                        {
                            if (!ZaupShop.instance.Configuration.Instance.CanBuyVehicles)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_vehicles_off")));
                                return;
                            }
                            string name = null;
                            ushort vehicleId;
                            System.Guid vehicleGuid = System.Guid.Empty;
                            if (components.Length < 2 || components[1] == null || components[1] == string.Empty)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_command_usage")));
                                return;
                            }
                            if (!ushort.TryParse(components[1].ToString(), out vehicleId))
                                foreach (VehicleRedirectorAsset vAsset in Assets.find(EAssetType.VEHICLE).Cast<VehicleRedirectorAsset>())
                                {
                                    if (vAsset?.FriendlyName != null && vAsset.FriendlyName.ToLower().Contains(components[1].ToLower()))
                                    {
                                        vehicleId = vAsset.id;
                                        vehicleGuid = vAsset.GUID;
                                        name = vAsset.FriendlyName;
                                        break;
                                    }
                                }
                            decimal cost;
                            if (vehicleGuid == System.Guid.Empty)
                            {
                                if (Assets.find(EAssetType.VEHICLE, vehicleId) == null)
                                {
                                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("could_not_find", components[1])));
                                    return;
                                }
                                else if (name == null && vehicleId != 0)
                                {
                                    name = Assets.find(EAssetType.VEHICLE, vehicleId)?.FriendlyName;
                                }
                                cost = ZaupShop.instance.Database.GetVehicleCost(vehicleId);
                            }else cost = ZaupShop.instance.Database.GetVehicleCost(vehicleGuid);

                            if (cost <= 0m)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("vehicle_not_available", name)));
                                return;
                            }
                            decimal balance = ZaupShop.instance.Database.GetBalance(player.Id);
                            if (balance < cost)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("not_enough_currency_msg", ZaupShop.instance.Configuration.Instance.UconomyCurrencyName, "1", name)));
                                return;
                            }
                            if (vehicleGuid == System.Guid.Empty)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>
                                {
                                    if (!player.GiveVehicle(vehicleId))
                                    {
                                        UnturnedChat.Say(caller, ZaupShop.instance.Translate("error_giving_item", name));
                                        return;
                                    }
                                });
                            }
                            else if(ZaupShop.instance.VehiclesGuid.TryGetValue(vehicleGuid, out XML.Structs.Vehicle val))
                            {
                                VehicleAsset Veh = Assets.FindVehicleAssetByGuidOrLegacyId(vehicleGuid, vehicleId);
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>
                                {
                                    if (VehicleManager.SpawnVehicleV3(Veh, 0, 0, 1f, player.Position, player.Player.transform.rotation, false, false, false, false, 100, val.Health, 100, player.CSteamID, player.SteamGroupID, false, new byte[0][], byte.MaxValue) != null)
                                    {
                                        UnturnedChat.Say(caller, ZaupShop.instance.Translate("error_giving_item", name));
                                        return;
                                    }
                                });
                            }

                            ZaupShop.instance.Database.RemoveBalance(player.Id, cost);
                            var newBal = ZaupShop.instance.Database.GetBalance(player.Id);

                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("vehicle_buy_msg", name, cost, ZaupShop.instance.Configuration.Instance.UconomyCurrencyName, newBal, ZaupShop.instance.Configuration.Instance.UconomyCurrencyName)));
                            break;
                        }
                    default:
                        {
                            if (!ZaupShop.instance.Configuration.Instance.CanBuyItems)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("buy_items_off")));
                                return;
                            }
                            string name = null;
                            if (!ushort.TryParse(command[0].ToString(), out ushort itemId))
                            {
                                foreach (ItemAsset vAsset in Assets.find(EAssetType.ITEM))
                                {
                                    if (vAsset?.itemName != null && vAsset.itemName.ToLower().Contains(command[0].ToLower()))
                                    {
                                        itemId = vAsset.id;
                                        name = vAsset.itemName;
                                        break;
                                    }
                                }
                            }
                            if (itemId == 0 || (name == null && ((name = Assets.find(EAssetType.ITEM, itemId)?.FriendlyName) == null || name == string.Empty)))
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("could_not_find", command[0])));
                                return;
                            }
                            decimal cost = decimal.Round(ZaupShop.instance.Database.GetItemCost(itemId) * amttobuy, 2);
                            decimal balance = ZaupShop.instance.Database.GetBalance(player.Id);

                            if (cost <= 0m)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("item_not_available", name)));
                                return;
                            }
                            if (balance < cost)
                            {
                                Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("not_enough_currency_msg", ZaupShop.instance.Configuration.Instance.UconomyCurrencyName, amttobuy, name)));
                                return;
                            }
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>
                            {
                                if (!player.GiveItem(itemId, amttobuy))
                                {
                                    UnturnedChat.Say(caller, ZaupShop.instance.Translate("error_giving_item", name));
                                    return;
                                }
                                System.Threading.Tasks.Task.Run(() => {
                                    ZaupShop.instance.Database.RemoveBalance(player.Id, cost);
                                    decimal newBal = ZaupShop.instance.Database.GetBalance(player.Id);
                                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("item_buy_msg", name, cost, ZaupShop.instance.Configuration.Instance.UconomyCurrencyName, newBal, ZaupShop.instance.Configuration.Instance.UconomyCurrencyName, amttobuy)));
                                });
                            });
                            break;
                        }
                }
            });
        }
    }
}
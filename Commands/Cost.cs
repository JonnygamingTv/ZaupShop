using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace ZaupShop.Commands
{
    public class Cost : IRocketCommand
    {
        public string Name => "cost";

        public string Syntax => "[v] <name or id>";

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Help => "Tells you the cost of a selected item.";

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "zaupshop.cost" };
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
                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("command_error_null")));
                    return;
                }

                string[] components = Parser.getComponentsFromSerial(command[0], '.');
                if (command.Length == 0 || components.Length == 0 || components.Length == 2 && components[0].Trim() != "v" || components.Length == 1 && components[0].Trim() == "v" || components.Length > 2 || command[0].Trim() == string.Empty)
                {
                    Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() => UnturnedChat.Say(caller, ZaupShop.instance.Translate("cost_command_usage")));
                    return;
                }

                switch (components[0])
                {
                    case "v":
                        string name = null;
                        ushort vehicleId;
                        System.Guid vehicleGuid = System.Guid.Empty;
                        if (components[1] == null || components[1] == string.Empty)
                        {
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("cost_command_usage")));
                            return;
                        }
                        if(!ushort.TryParse(components[1].ToString(), out vehicleId))
                        foreach (VehicleAsset vAsset in Assets.find(EAssetType.VEHICLE).Cast<VehicleAsset>())
                        {
                            if (vAsset != null && vAsset.vehicleName != null && vAsset.vehicleName.ToLower().Contains(components[1].ToLower()))
                            {
                                vehicleId = vAsset.id;
                                vehicleGuid = vAsset.GUID;
                                name = vAsset.vehicleName;
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
                                name = ((VehicleAsset)Assets.find(EAssetType.VEHICLE, vehicleId)).vehicleName;
                            }
                            cost = ZaupShop.instance.Database.GetVehicleCost(vehicleId);
                        }else cost = ZaupShop.instance.Database.GetVehicleCost(vehicleGuid);

                        if (cost <= 0m)
                        {
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("vehicle_not_available", name)));
                        }
                        else
                        {
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("vehicle_cost_msg", name, cost.ToString(CultureInfo.CurrentCulture), ZaupShop.instance.Configuration.Instance.UconomyCurrencyName)));
                        }
                        break;
                    default:
                        name = null;
                        if (!ushort.TryParse(components[0], out ushort id))
                        {
                            Asset[] array = Assets.find(EAssetType.ITEM);
                            Asset[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                ItemAsset iAsset = (ItemAsset)array2[i];
                                if (iAsset != null && iAsset.itemName != null && iAsset.itemName.ToLower().Contains(string.Join(".",components).ToLower()))
                                {
                                    id = iAsset.id;
                                    name = iAsset.itemName;
                                    break;
                                }
                            }
                        }
                        if (Assets.find(EAssetType.ITEM, id) == null)
                        {
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("could_not_find", string.Join(".", components))));
                            return;
                        }
                        else if (name == null && id != 0)
                        {
                            name = ((ItemAsset)Assets.find(EAssetType.ITEM, id)).itemName;
                        }
                        cost = ZaupShop.instance.Database.GetItemCost(id);
                        decimal bbp = ZaupShop.instance.Database.GetItemBuyPrice(id);
                        if (cost <= 0m)
                        {
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("error_getting_cost", name)));
                        }
                        else
                        {
                            Rocket.Core.Utils.TaskDispatcher.QueueOnMainThread(() =>UnturnedChat.Say(caller, ZaupShop.instance.Translate("item_cost_msg", name, cost.ToString(CultureInfo.CurrentCulture), ZaupShop.instance.Configuration.Instance.UconomyCurrencyName, bbp.ToString(CultureInfo.CurrentCulture), ZaupShop.instance.Configuration.Instance.UconomyCurrencyName)));
                        }
                        break;
                }
            });
        }
    }
}
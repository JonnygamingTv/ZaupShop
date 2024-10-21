using Rocket.API;

namespace ZaupShop
{
    public class ZaupShopConfiguration : IRocketPluginConfiguration
    {
        public int DatabasePort;
        public string DatabaseAddress;
        public string DatabaseName;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string ItemShopTableName;
        public string VehicleShopTableName;
        public string UconomyTableName;
        public string UconomyCurrencyName;
        public XML.enums.SaveType ShopType;
        public System.Collections.Generic.List<XML.Structs.Item> Items;
        public System.Collections.Generic.List<XML.Structs.Vehicle> Vehicles;
        public bool CanBuyItems;
        public bool CanBuyVehicles;
        public bool CanSellItems;
        public bool QualityCounts;
        public bool xpMode;

        public void LoadDefaults()
        {
            DatabasePort = 3306;
            DatabaseAddress = "127.0.0.1";
            DatabaseName = "unturned";
            DatabaseUsername = "admin";
            DatabasePassword = "root";
            ItemShopTableName = "uconomyitemshop";
            VehicleShopTableName = "uconomyvehicleshop";
            UconomyTableName = "uconomy";
            UconomyCurrencyName = "Credits";
            ShopType = XML.enums.SaveType.XML;
            CanBuyItems = true;
            CanBuyVehicles = false;
            CanSellItems = true;
            QualityCounts = true;
            xpMode = true;
            Items = new System.Collections.Generic.List<XML.Structs.Item>();
            Vehicles = new System.Collections.Generic.List<XML.Structs.Vehicle>();
            //XML.Structs.ItemP Tmp = new XML.Structs.ItemP(5,1,100);
            //XML.Structs.Vehicle TmpV = new XML.Structs.Vehicle { Cost = 50, Health = 100 };
            SDG.Unturned.VehicleAsset Noob = SDG.Unturned.Assets.FindVehicleAssetByGuidOrLegacyId(System.Guid.Empty, 33);
            Items.Add(new XML.Structs.Item(13));
            Vehicles.Add(new XML.Structs.Vehicle(33));
            Vehicles.Add(new XML.Structs.Vehicle(Noob.GUID.ToString()));
        }
    }
}
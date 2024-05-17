using Rocket.API;

namespace ZaupShop
{
    public class ZaupShopConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseAddress = "127.0.0.1";
        public string DatabaseName = "unturned";
        public string DatabaseUsername = "admin";
        public string DatabasePassword = "root";
        public int DatabasePort = 3306;
        public string ItemShopTableName = "uconomyitemshop";
        public string VehicleShopTableName = "uconomyvehicleshop";
        public string UconomyTableName = "uconomy";
        public string UconomyCurrencyName = "Credits";
        public bool CanBuyItems = true;
        public bool CanBuyVehicles = false;
        public bool CanSellItems = true;
        public bool QualityCounts = true;

        public void LoadDefaults()
        {

        }
    }
}

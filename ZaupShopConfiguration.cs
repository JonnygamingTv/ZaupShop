﻿using Rocket.API;

namespace ZaupShop
{
    public class ZaupShopConfiguration : IRocketPluginConfiguration
    {
        public string ItemShopTableName { get; set; } = "uconomyitemshop";
        public string VehicleShopTableName { get; set; } = "uconomyvehicleshop";
        public string UconomyTableName { get; set; } = "uconomy";
        public string UconomyCurrencyName { get; set; } = "Credits";
        public bool CanBuyItems { get; set; } = true;
        public bool CanBuyVehicles { get; set; } = false;
        public bool CanSellItems { get; set; } = true;
        public bool QualityCounts { get; set; } = true;
        public string DatabaseName { get; set; } = "unturned";
        public string DatabaseAddress { get; set; } = "localhost";
        public int DatabasePort { get; set; } = 3306;
        public string DatabaseUsername { get; set; } = "root";
        public string DatabasePassword { get; set; } = "mypassword";

        public void LoadDefaults()
        {

        }
    }
}

﻿using System;
using Rocket.RocketAPI;
using Rocket.Logging;

namespace UconomyBasicShop
{
    public class UconomyBasicShopConfiguration : RocketConfiguration
    {
        public string ItemShopTableName;
        public string VehicleShopTableName;
        public bool CanBuyItems;
        public bool CanBuyVehicles;
        public bool CanSellItems;
        public bool QualityCounts;

        public RocketConfiguration DefaultConfiguration
        {
            get
            {
                return new UconomyBasicShopConfiguration
                {
                    ItemShopTableName = "uconomyitemshop",
                    VehicleShopTableName = "uconomyvehicleshop",
                    CanBuyItems = true,
                    CanBuyVehicles = false,
                    CanSellItems = true,
                    QualityCounts = true
                };
            }
        }
    }
}

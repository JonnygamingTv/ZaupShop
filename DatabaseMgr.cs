using System;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;

namespace ZaupShop
{
    public class DatabaseMgr
    {
        private readonly ZaupShop _zaupShop;

        internal DatabaseMgr(ZaupShop zaupShop)
        {
            _zaupShop = zaupShop;
            CheckSchema();
        }

        private void CheckSchema()
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("show tables like '", _zaupShop.Configuration.Instance.ItemShopTableName, "'");
                mySqlConnection.Open();
                if (mySqlCommand.ExecuteScalar() == null)
                {
                    mySqlCommand.CommandText = string.Concat("CREATE TABLE `", _zaupShop.Configuration.Instance.ItemShopTableName, "` (`id` int(6) NOT NULL,`itemname` varchar(32) NOT NULL,`cost` decimal(15,2) NOT NULL DEFAULT '20.00',`buyback` decimal(15,2) NOT NULL DEFAULT '0.00',PRIMARY KEY (`id`)) ");
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("show tables like '", _zaupShop.Configuration.Instance.VehicleShopTableName, "'");
                mySqlConnection.Open();
                if (mySqlCommand.ExecuteScalar() == null)
                {
                    mySqlCommand.CommandText = string.Concat("CREATE TABLE `", _zaupShop.Configuration.Instance.VehicleShopTableName, "` (`id` int(6) NOT NULL,`vehiclename` varchar(32) NOT NULL,`cost` decimal(15,2) NOT NULL DEFAULT '100.00',PRIMARY KEY (`id`)) ");
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception}");
            }
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("show columns from `", _zaupShop.Configuration.Instance.ItemShopTableName, "` like 'buyback'");
                mySqlConnection.Open();
                if (mySqlCommand.ExecuteScalar() == null)
                {
                    mySqlCommand.CommandText = string.Concat("ALTER TABLE `", _zaupShop.Configuration.Instance.ItemShopTableName, "` ADD `buyback` decimal(15,2) NOT NULL DEFAULT '0.00'");
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
        }

        public MySqlConnection CreateConnection()
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                mySqlConnection = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", _zaupShop.Configuration.Instance.DatabaseAddress, _zaupShop.Configuration.Instance.DatabaseName, _zaupShop.Configuration.Instance.DatabaseUsername, _zaupShop.Configuration.Instance.DatabasePassword, _zaupShop.Configuration.Instance.DatabasePort));
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
            return mySqlConnection;
        }

        public bool AddItem(int id, string name, decimal cost, bool change)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                if (!change)
                {
                    mySqlCommand.CommandText = string.Concat("Insert into `", _zaupShop.Configuration.Instance.ItemShopTableName, "` (`id`, `itemname`, `cost`) VALUES ('", id.ToString(), "', '", name, "', '", cost.ToString(), "');");
                }
                else
                {
                    mySqlCommand.CommandText = string.Concat("update `", _zaupShop.Configuration.Instance.ItemShopTableName, "` set itemname='", name, "', cost='", cost.ToString(), "' where id='", id.ToString(), "';");
                }
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
                return false;
            }
        }

        public bool AddVehicle(int id, string name, decimal cost, bool change)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                if (!change)
                {
                    mySqlCommand.CommandText = string.Concat("Insert into `", _zaupShop.Configuration.Instance.VehicleShopTableName, "` (`id`, `vehiclename`, `cost`) VALUES ('", id.ToString(), "', '", name, "', '", cost.ToString(), "');");
                }
                else
                {
                    mySqlCommand.CommandText = string.Concat("update `", _zaupShop.Configuration.Instance.VehicleShopTableName, "` set vehiclename='", name, "', cost='", cost.ToString(), "' where id='", id.ToString(), "';");
                }
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
                return false;
            }
        }

        public decimal GetItemCost(int id)
        {
            decimal num = new(0);
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("select `cost` from `", _zaupShop.Configuration.Instance.ItemShopTableName, "` where `id` = '", id.ToString(), "';");
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out num);
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
            return num;
        }

        public decimal GetVehicleCost(int id)
        {
            decimal num = new(0);
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("select `cost` from `", _zaupShop.Configuration.Instance.VehicleShopTableName, "` where `id` = '", id.ToString(), "';");
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out num);
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
            return num;
        }

        public decimal GetBalance(string id)
        {
            decimal num = new(0);
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("select `balance` from `", _zaupShop.Configuration.Instance.UconomyTableName, "` where `id` = '", id, "';");
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out num);
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
            return num;
        }

        public void RemoveBalance(string id, decimal cost)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = $"update `{_zaupShop.Configuration.Instance.UconomyTableName}` set `balance` = `balance` - {cost} where `id` = {id};";
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed by {id}, reason: {exception.Message}");
            }
        }

        public void AddBalance(string id, decimal quantity)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = $"update `{_zaupShop.Configuration.Instance.UconomyTableName}` set `balance` = `balance` + {quantity} where `id` = {id};";
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed by {id}, reason: {exception.Message}");
            }
        }

        public bool DeleteItem(int id)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("delete from `", _zaupShop.Configuration.Instance.ItemShopTableName, "` where id='", id.ToString(), "';");
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
                return false;
            }
        }

        public bool DeleteVehicle(int id)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("delete from `", _zaupShop.Configuration.Instance.VehicleShopTableName, "` where id='", id.ToString(), "';");
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
                return false;
            }
        }

        public bool SetBuyPrice(int id, decimal cost)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("update `", _zaupShop.Configuration.Instance.ItemShopTableName, "` set `buyback`='", cost.ToString(), "' where id='", id.ToString(), "';");
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
                return false;
            }
        }

        public decimal GetItemBuyPrice(int id)
        {
            decimal num = new(0);
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("select `buyback` from `", _zaupShop.Configuration.Instance.ItemShopTableName, "` where `id` = '", id.ToString(), "';");
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out num);
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"Database Crashed, reason: {exception.Message}");
            }
            return num;
        }
    }
}
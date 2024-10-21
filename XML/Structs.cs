using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZaupShop.XML
{
    public class Structs
    {
        public struct _Item
        {
            public int Cost;
            public int SellPrice;
            public byte Durability;
        } // <Item id="NaN" buyAmount="1" sellAmount="1" defaultDurability
        public struct _Vehicle
        {
            public int Cost;
            public byte Health;
        } // <Vehicle id="NaN" Guid="Guid" buyAmount="1" sellAmount="1" defaultDurability
        public class Item {
            public Item() { }
            public Item(ushort id, decimal cost=0, decimal sellprice=0, byte durability=100) {
                this.Cost = cost;
                this.SellPrice = sellprice;
                this.Durability = durability;
            }
            [XmlAttribute] public ushort Id;
            [XmlAttribute] public decimal Cost;
            [XmlAttribute] public decimal SellPrice;
            [XmlAttribute] public byte Durability;
        }
        public class Vehicle
        {
            public Vehicle() { }
            public Vehicle(ushort id, decimal cost = 0, decimal sellprice = 0, byte health = 100)
            {
                this.Id = id;
                this.Cost = cost;
                this.SellPrice = sellprice;
                this.Health = health;
            }
            public Vehicle(string guid, decimal cost = 0, decimal sellprice = 0, byte health = 100)
            {
                this.Guid = guid;
                this.Cost = cost;
                this.SellPrice = sellprice;
                this.Health = health;
            }
            [XmlAttribute] public string Guid;
            [XmlAttribute] public ushort Id;
            [XmlAttribute] public decimal Cost;
            [XmlAttribute] public decimal SellPrice;
            [XmlAttribute] public byte Health;
        }
    }
}

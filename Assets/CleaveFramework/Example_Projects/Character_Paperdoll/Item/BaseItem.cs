using System;
using CleaveFramework.Binding;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Item
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Trinket,
    }

    public enum BaseStatType
    {
        Strength,
        Dexterity,
        Intellect,
    }

    public abstract class BaseItem
    {
        public ItemType MyType { get; set; }
        public int ItemLevel { get; set; }
        public string ItemName { get; set; }

        private Binding<BaseStatType, int> _baseItemStats = new Binding<BaseStatType, int>();

        protected BaseItem(int itemLevel, ItemType type, string name)
        {
            ItemName = name;
            MyType = type;
            ItemLevel = itemLevel;
            var stats = Enum.GetValues(typeof(BaseStatType));
            foreach (BaseStatType stat in stats)
            {
                _baseItemStats[stat] = RollStat();
            }
        }

        protected int RollStat()
        {
            return UnityEngine.Random.Range(1, ItemLevel + 1);
        }

        public int GetStat(BaseStatType type)
        {
            return _baseItemStats[type];
        }

    }

    public enum WeaponStatType
    {
        WeaponDamage,
        DamageRange,
    }

    public class WeaponItem : BaseItem
    {
        Binding<WeaponStatType, int> _weaponItemStats = new Binding<WeaponStatType, int>();

        public WeaponItem(int itemLevel, string name)
            : base(itemLevel, ItemType.Weapon, name)
        {
            var stats = Enum.GetValues(typeof(WeaponStatType));
            foreach (WeaponStatType stat in stats)
            {
                _weaponItemStats[stat] = RollStat();
            }
        }

        public int GetStat(WeaponStatType type)
        {
            return _weaponItemStats[type];
        }
    }

    public enum ArmorStatType
    {
        ArmorClass,
    }

    public class ArmorItem : BaseItem
    {

        Binding<ArmorStatType, int> _armorItemStats = new Binding<ArmorStatType, int>();

        public ArmorItem(int itemLevel, string name) : base(itemLevel, ItemType.Armor, name)
        {
            var stats = Enum.GetValues(typeof(ArmorStatType));
            foreach (ArmorStatType stat in stats)
            {
                _armorItemStats[stat] = RollStat();
            }
        }

        public int GetStat(ArmorStatType type)
        {
            return _armorItemStats[type];
        }

    }

    public class TrinketItem : BaseItem
    {
        public TrinketItem(int itemLevel, string name)
            : base(itemLevel, ItemType.Trinket, name)
        {
        }
    }

    

}

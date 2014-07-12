using System;
using CleaveFramework.Binding;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Paperdoll
{
    /// <summary>
    /// Define all slots possible on a paperdoll
    /// </summary>
    public enum PaperdollSlot
    {
        Head,
        Chest,
        Legs,
        Feet,
        Weapon,
        Ring,
    }

    /// <summary>
    /// abstract base class for the Paperdoll
    /// </summary>
    public abstract class Paperdoll : IPaperdoll
    {
        // use a binding to map a slot to a certain item type
        protected Binding<PaperdollSlot, ItemType> _slotRestrictions = new Binding<PaperdollSlot, ItemType>();
        // use a binding to map a slot to a specific item
        protected Binding<PaperdollSlot, BaseItem> _equippedItems = new Binding<PaperdollSlot, BaseItem>();

        // we use a protected constructor, Paperdoll is abstract
        protected Paperdoll()
        {
            // use Binding to restrict a slot to a certain item type
            _slotRestrictions[PaperdollSlot.Head] = ItemType.Armor;
            _slotRestrictions[PaperdollSlot.Chest] = ItemType.Armor;
            _slotRestrictions[PaperdollSlot.Legs] = ItemType.Armor;
            _slotRestrictions[PaperdollSlot.Feet] = ItemType.Armor;
            _slotRestrictions[PaperdollSlot.Weapon] = ItemType.Weapon;
            _slotRestrictions[PaperdollSlot.Ring] = ItemType.Trinket;

            // iterate all slots in the enum
            var slots = Enum.GetValues(typeof(PaperdollSlot));
            foreach (PaperdollSlot slot in slots)
            {
                // use [] operator to bind the slot and assign it to null
                _equippedItems[slot] = null;
            }

            // note you could in a derived type unbind a slot restriction permanently or make that binding dynamic,
            // for example unlock a second ring slot when the player reaches level 10.  i'll leave that as 
            // an exercise for you :)
        }

        /// <summary>
        /// insert item into slot
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="item"></param>
        /// <returns>if swap is successful, the old item.
        ///          if swap fails, the requested item.
        public BaseItem Equip(PaperdollSlot slot, BaseItem item)
        {

            if (!CanEquip(slot, item)) return item;

            // cache old item
            var oldItem = _equippedItems[slot];

            // swap it
            _equippedItems[slot] = item;

            return oldItem;
        }

        /// <summary>
        /// iterate stats and return the sum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int SumStat(BaseStatType type)
        {
            var stat = 0;
            var slots = Enum.GetValues(typeof (PaperdollSlot));
            foreach (PaperdollSlot slot in slots)
            {
                if (_equippedItems[slot] == null) continue;

                stat += _equippedItems[slot].GetStat(type);
            }
            return stat;
        }

        /// <summary>
        /// iterate stats and return the sum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int SumStat(WeaponStatType type)
        {
            var stat = 0;
            var slots = Enum.GetValues(typeof (PaperdollSlot));
            foreach (PaperdollSlot slot in slots)
            {
                if (_equippedItems[slot] == null) continue;
                if (_equippedItems[slot].MyType == ItemType.Weapon)
                {
                    stat += ((WeaponItem) _equippedItems[slot]).GetStat(type);
                }
            }
            return stat;
        }

        /// <summary>
        /// iterate stats and return the sum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int SumStat(ArmorStatType type)
        {
            var stat = 0;
            var slots = Enum.GetValues(typeof (PaperdollSlot));
            foreach (PaperdollSlot slot in slots)
            {
                if (_equippedItems[slot] == null) continue;
                if (_equippedItems[slot].MyType == ItemType.Armor)
                {
                    stat += ((ArmorItem) _equippedItems[slot]).GetStat(type);
                }
            }
            return stat;
        }

        public bool CanEquip(PaperdollSlot slot, BaseItem item)
        {
            // paperdoll doesn't support this slot
            if (!_slotRestrictions.IsBound(slot)) return false;

            // item doesn't fit in this slot
            if (_slotRestrictions[slot] != item.MyType) return false;

            return true;
        }

        public BaseItem HasEquipped(PaperdollSlot slot)
        {
            return _equippedItems[slot];
        }

        public override string ToString()
        {

            var retStr = "";

            retStr += "============= Paperdoll =============\n";

            var baseStats = Enum.GetValues(typeof(BaseStatType));
            foreach (BaseStatType type in baseStats)
            {
                retStr += "(" + type + "): " + SumStat(type) + ", ";
            }
            var weaponStats = Enum.GetValues(typeof(WeaponStatType));
            foreach (WeaponStatType type in weaponStats)
            {
                retStr += "(" + type + "): " + SumStat(type) + ", ";
            }

            return retStr;
        }

    }
}
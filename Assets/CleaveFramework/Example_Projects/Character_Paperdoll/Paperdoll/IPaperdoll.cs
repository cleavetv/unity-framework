namespace CleaveFramework.Example_Projects.Character_Paperdoll.Paperdoll
{
    public interface IPaperdoll
    {
        bool CanEquip(PaperdollSlot slot, Item.BaseItem item);
        Item.BaseItem HasEquipped(PaperdollSlot slot);
        Item.BaseItem Equip(PaperdollSlot slot, Item.BaseItem item);
        int SumStat(Item.ArmorStatType type);
        int SumStat(Item.BaseStatType type);
        int SumStat(Item.WeaponStatType type);
    }
}

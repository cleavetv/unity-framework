using CleaveFramework.Commands;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;
using CleaveFramework.Example_Projects.Character_Paperdoll.Paperdoll;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Commands
{
    class PlayerEquipItemRequestCmd<T> : Command
        where T: BaseItem
    {
        public PaperdollSlot Slot { get; private set; }
        public T Item { get; private set; }
        public PlayerEquipItemRequestCmd(PaperdollSlot slot, T item)
        {
            Slot = slot;
            Item = item;
        }

    }
}

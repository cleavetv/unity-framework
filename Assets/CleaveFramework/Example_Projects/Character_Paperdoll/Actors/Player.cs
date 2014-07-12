using CleaveFramework.Commands;
using CleaveFramework.Core;
using CleaveFramework.Example_Projects.Character_Paperdoll.Commands;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;
using CleaveFramework.Example_Projects.Character_Paperdoll.Paperdoll;
using CleaveFramework.Interfaces;
using UnityEngine;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Actors
{
    /// <summary>
    /// Implement a player controller
    /// </summary>
    public class Player : Actor, IDestroyable, IUpdateable
    {
        public Player()
        {
            CmdBinder.AddBinding<PlayerEquipItemRequestCmd<TrinketItem>>(OnEquipItemRequest<TrinketItem>);
            CmdBinder.AddBinding<PlayerEquipItemRequestCmd<WeaponItem>>(OnEquipItemRequest<WeaponItem>);
            CmdBinder.AddBinding<PlayerEquipItemRequestCmd<ArmorItem>>(OnEquipItemRequest<ArmorItem>);
        }

        /// <summary>
        /// callback for PlayerEquipItemRequestCmds
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        private void OnEquipItemRequest<T>(Command c)
            where T: BaseItem
        {
            var cmd = c as PlayerEquipItemRequestCmd<T>;
            
            var item = Equip(cmd.Slot, cmd.Item);
            if (cmd.Item == item)
            {
                // request failed, do something to tell the player
                UnityEngine.Debug.Log("EquipRequest failure");
            }
            else
            {
                Debug.Log("New item " + cmd.Item.ItemName + " equipped");
            }

        }

        /// <summary>
        /// Implement IDestroyable to remove bindings on scene exit
        /// </summary>
        public void Destroy()
        {
            CmdBinder.RemoveBinding<PlayerEquipItemRequestCmd<TrinketItem>>(OnEquipItemRequest<TrinketItem>);
            CmdBinder.RemoveBinding<PlayerEquipItemRequestCmd<WeaponItem>>(OnEquipItemRequest<WeaponItem>);
            CmdBinder.RemoveBinding<PlayerEquipItemRequestCmd<ArmorItem>>(OnEquipItemRequest<ArmorItem>);
        }

        /// <summary>
        /// Implement IUpdateable to gather input for this player controller
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log(Paperdoll.ToString());
            }

            // equip different weapons
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                Framework.PushCommand(new PlayerEquipItemRequestCmd<WeaponItem>(PaperdollSlot.Weapon, new WeaponItem(1, "lvl 1 sword")));
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                Framework.PushCommand(new PlayerEquipItemRequestCmd<WeaponItem>(PaperdollSlot.Weapon, new WeaponItem(10, "lvl 10 sword")));
            }
            // try to equip an armor in the weapon slot
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                Framework.PushCommand(new PlayerEquipItemRequestCmd<ArmorItem>(PaperdollSlot.Weapon, new ArmorItem(20, "lvl 20 armor")));
            }
            // try to equip a weapon in an armor slot
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                Framework.PushCommand(new PlayerEquipItemRequestCmd<WeaponItem>(PaperdollSlot.Head, new WeaponItem(50, "lvl 50 sword")));
            }
        }
    }
}

using CleaveFramework.Binding;
using CleaveFramework.Core;
using CleaveFramework.DependencyInjection;
using CleaveFramework.Example_Projects.Character_Paperdoll.Actors;
using CleaveFramework.Example_Projects.Character_Paperdoll.Actors.WorldViews;
using CleaveFramework.Example_Projects.Character_Paperdoll.Commands;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;
using CleaveFramework.Example_Projects.Character_Paperdoll.Paperdoll;
using CleaveFramework.Scene;
using UnityEngine;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.SceneViews
{

    public enum SceneTags
    {
        Monster,
    }

    public class PaperdollSceneView : SceneView
    {

        Binding<SceneTags, string> _sceneTags = new Binding<SceneTags,string>();

        public override void Initialize()
        {
            // use Binding to bind tags to a strong enum type
            _sceneTags[SceneTags.Monster] = "Monster";

            // bind IPaperdoll interface to a Playerpaperdoll implementation
            Injector.BindTransient<IPaperdoll, PlayerPaperdoll>();
            // create a new Player and bind it as an injectable singleton to Player implementation
            Injector.BindSingleton<Player>(
                Factory.Factory.Create<Player>(CreateDefaultPlayer, SceneObjects));

            // 3 different ways to create player:
            // 1. If you've got a GameObject in the scene with an attached PlayerWorldView component to it we can construct it at runtime:
            Factory.Factory.ConstructMonoBehaviour<PlayerWorldView>("PlayerWorldView_AddedInEditor", SceneObjects);
            // 2. If you've got a GameObject in the scene without an attached PlayerWorldView component to it we can add it at runtime:
            Factory.Factory.AddComponent<PlayerWorldView>("PlayerWorldView_EmptyContainer", SceneObjects);
            // 3. We can make a new GameObject in code here at runtime and add the component to it (note this could easily be a prefab):
            var playerGameObject = new GameObject("PlayerWorldView_FromCode");
            Factory.Factory.AddComponent<PlayerWorldView>(playerGameObject, SceneObjects);
            // Important note: Since the Player object is a singleton each PlayerWorldView has a reference to the exact same
            // instance of the PlayerData.  We've successful encapsulated Player from it's Scene View representation.
            // We could easily give this exact same Player to another system, like HUD, Combat, or a Web Service via Injection
            // in the same manner.

            // re-bind IPaperdoll interface to MonsterPaperdoll now
            Injector.BindTransient<IPaperdoll, MonsterPaperdoll>();
            // set default constructor for MonsterPaperdolls
            Factory.Factory.SetConstructor<MonsterPaperdoll>(CreateDefaultMonsterPaperdoll);
            // tell Injector to make new instances of Monster
            Injector.BindTransient<Monster>();

            // iterate monsters added in the scene by our bound tag
            var monsters = GameObject.FindGameObjectsWithTag(_sceneTags[SceneTags.Monster]);
            for (var index = 0; index < monsters.Length; index++)
            {
                var monster = monsters[index];
                // construct the MonsterWorldView component pre-attached to the monsters (they pre-exist in the scene)
                Factory.Factory.ConstructMonoBehaviour<MonsterWorldView>(monster, SceneObjects, monster.name + "_" + index);
            }
            // Important note: Even though we only use method 1 here for constructing the MonsterWorldView we could easily be
            // instantiating monsters here from a prefab at runtime and constructing those instead.
        }

        /// <summary>
        /// default constructor for a monster
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object CreateDefaultMonsterPaperdoll(object obj)
        {
            var paperdoll = obj as MonsterPaperdoll;
            paperdoll.Equip(PaperdollSlot.Weapon, new WeaponItem(2, "Wooden Sword"));
            paperdoll.Equip(PaperdollSlot.Legs, new ArmorItem(1, "Tattered Pants"));
            return paperdoll;
        }

        /// <summary>
        /// default constructor for the player
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object CreateDefaultPlayer(object obj)
        {
            var player = obj as Player;
            // equip items with the PlayerEquipItemRequestCmd as an example
            // we could call player.Equip() here and get the same results, however it's important to realize
            // that other systems besides the Player might be listening for this Command and need to act on it as well!
            // We could also consider moving the PushCommand() in to an overloaded Equip in Player instead.  
            // These are design decisions you'll have to determine on your own.
            // Also note that although we are creating new items here we could easily modify the Command
            // to take an ItemID and resolve that through a data library instead.
            // We could also be loading those ItemID's from a save file too.  
            // These are things you'll have to consider when implementing your own game.
            Framework.PushCommand(new PlayerEquipItemRequestCmd<ArmorItem>(PaperdollSlot.Head, new ArmorItem(1, "Tattered Hat")));
            Framework.PushCommand(new PlayerEquipItemRequestCmd<ArmorItem>(PaperdollSlot.Chest,
                new ArmorItem(1, "Tattered Shirt")));
            Framework.PushCommand(new PlayerEquipItemRequestCmd<ArmorItem>(PaperdollSlot.Feet,
                new ArmorItem(1, "Tattered Shoes")));
            Framework.PushCommand(new PlayerEquipItemRequestCmd<ArmorItem>(PaperdollSlot.Legs,
                new ArmorItem(1, "Tattered Pants")));
            Framework.PushCommand(new PlayerEquipItemRequestCmd<WeaponItem>(PaperdollSlot.Weapon,
                new WeaponItem(2, "Wooden Sword")));
            Framework.PushCommand(new PlayerEquipItemRequestCmd<TrinketItem>(PaperdollSlot.Ring,
                new TrinketItem(0, "Brass Ring")));
            return player;
        }


    }

}
using System;
using CleaveFramework.Core;
using CleaveFramework.DependencyInjection;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;
using CleaveFramework.Interfaces;
using CleaveFramework.Tools;
using UnityEngine;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Actors.WorldViews
{
    public class PlayerWorldView : MonoBehaviour, IInitializeable
    {
        [Inject] public Player PlayerData = null;

        public void Initialize()
        {
            CDebug.Assert(PlayerData == null, "PlayerData is null");

            UnityEngine.Debug.Log("============= PlayerData(" + gameObject.name + ") =============");
            var baseStats = Enum.GetValues(typeof (BaseStatType));
            foreach (BaseStatType type in baseStats)
            {
                UnityEngine.Debug.Log("Player's (" + type + "): " + PlayerData.Paperdoll.SumStat(type));
            }
            var weaponStats = Enum.GetValues(typeof(WeaponStatType));
            foreach (WeaponStatType type in weaponStats)
            {
                UnityEngine.Debug.Log("Player's (" + type + "): " + PlayerData.Paperdoll.SumStat(type));
            }

            UnityEngine.Debug.Log("=====================================");
        }
    }
}

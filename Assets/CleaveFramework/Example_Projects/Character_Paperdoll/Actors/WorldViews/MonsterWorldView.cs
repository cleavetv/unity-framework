using System;
using CleaveFramework.Core;
using CleaveFramework.DependencyInjection;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;
using CleaveFramework.Interfaces;
using CleaveFramework.Tools;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Actors.WorldViews
{
    class MonsterWorldView : View, IInitializeable
    {
        [Inject]
        public Monster MonsterData = null;

        public void Initialize()
        {
            CDebug.Assert(MonsterData == null, "MonsterData is null");

            UnityEngine.Debug.Log("============= MonsterData(" + gameObject.name + ") =============");
            var baseStats = Enum.GetValues(typeof(BaseStatType));
            foreach (BaseStatType type in baseStats)
            {
                UnityEngine.Debug.Log("Player's (" + type + "): " + MonsterData.Paperdoll.SumStat(type));
            }
            var weaponStats = Enum.GetValues(typeof(WeaponStatType));
            foreach (WeaponStatType type in weaponStats)
            {
                UnityEngine.Debug.Log("Player's (" + type + "): " + MonsterData.Paperdoll.SumStat(type));
            }

            UnityEngine.Debug.Log("=====================================");
        }
    }
}

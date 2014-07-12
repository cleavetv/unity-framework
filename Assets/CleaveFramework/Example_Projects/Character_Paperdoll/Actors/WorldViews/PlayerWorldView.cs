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
        }
    }
}

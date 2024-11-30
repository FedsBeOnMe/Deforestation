using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;

namespace Deforestation
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool inRoom;
        private List<GameObject> targetObjects = new List<GameObject>();

        void Start()
        {
            if (!PhotonNetwork.InRoom) OnModdedJoined();
            else if (!NetworkSystem.Instance.GameModeString.Contains("MODDED")) OnModdedLeft();
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized()
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.activeInHierarchy &&
                    (obj.name == "forestatlas (combined by EdMeshCombinerSceneProcessor)" ||
                     obj.name.ToLower().Contains("fallleaves")))
                {
                    targetObjects.Add(obj);
                }
            }
        }

        void Update()
        {
        }

        public void OnModdedJoined()
        {
            inRoom = true;

            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }

        public void OnModdedLeft(string gamemode)
        {
            inRoom = false;

            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}

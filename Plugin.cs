using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using Utilla;

namespace Deforestation
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool inRoom;
        private List<GameObject> targetObjects = new List<GameObject>();

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
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

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
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

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
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

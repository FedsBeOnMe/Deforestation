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
        private bool isInModded = false;
        private readonly string[] targetNames =
        {
            "forestatlas (combined by EdMeshCombinerSceneProcessor)",
            "fallleaves",
            "bark (combined by EdMeshCombinerSceneProcessor)",
            "pinetreebranchsmall (combined by EdMeshCombinerSceneProcessor)",
            "Bayou_Tree (combined by EdMeshCombinerSceneProcessor)",
            "Bayou_Tree_Moss (combined by EdMeshCombinerSceneProcessor)",
            "SwampTree (combined by EdMeshCombinerSceneProcessor)",
            "RopeSwings"
        };

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
            Debug.Log("Game initialized.");
        }

        void Update()
        {
            if (isInModded)
            {
                DisableTargetObjects();
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            Debug.Log($"Joined modded gamemode: {gamemode}");
            isInModded = true;
            DisableTargetObjects(); // Initial call to handle existing objects
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            Debug.Log($"Left modded gamemode: {gamemode}");
            isInModded = false;
            EnableTargetObjects(); // Re-enable objects when leaving modded
        }

        private void DisableTargetObjects()
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.activeInHierarchy && MatchesTargetName(obj.name))
                {
                    obj.SetActive(false);
                    Debug.Log($"Disabled object: {obj.name}");
                }
            }
        }

        private void EnableTargetObjects()
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (!obj.activeInHierarchy && MatchesTargetName(obj.name))
                {
                    obj.SetActive(true);
                    Debug.Log($"Enabled object: {obj.name}");
                }
            }
        }

        private bool MatchesTargetName(string name)
        {
            name = name.ToLower();
            foreach (string targetName in targetNames)
            {
                if (name == targetName.ToLower()) // Match exact names
                {
                    return true;
                }
            }
            return false;
        }
    }
}

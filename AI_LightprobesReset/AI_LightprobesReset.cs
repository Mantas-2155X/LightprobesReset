using BepInEx;
using BepInEx.Harmony;
using HarmonyLib;
using UnityEngine;

namespace AI_LightprobesReset
{
    [BepInPlugin(nameof(AI_LightprobesReset), nameof(AI_LightprobesReset), VERSION)][BepInProcess("StudioNEOV2")]
    public class AI_LightprobesReset : BaseUnityPlugin
    {
        public const string VERSION = "1.0.0";

        private void Awake()
        {
            HarmonyWrapper.PatchAll(typeof(AI_LightprobesReset));
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(Studio.Map), "LoadMap")]
        public static void Map_LoadMap_Patch()
        {
            LightmapSettings.lightProbes = null;
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(Studio.Map), "LoadMapCoroutine")]
        public static void Map_LoadMapCoroutine_Patch()
        {
            LightmapSettings.lightProbes = null;
        }
    }
}
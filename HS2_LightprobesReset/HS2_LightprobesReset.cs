using BepInEx;
using HarmonyLib;

using AIChara;

using UnityEngine;
using UnityEngine.Rendering;

namespace HS2_LightprobesReset
{
    [BepInPlugin(nameof(HS2_LightprobesReset), nameof(HS2_LightprobesReset), VERSION)][BepInProcess("StudioNEOV2")]
    public class HS2_LightprobesReset : BaseUnityPlugin
    {
        public const string VERSION = "1.1.0";

        private void Awake() => Harmony.CreateAndPatchAll(typeof(HS2_LightprobesReset));
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Studio.Studio), nameof(Studio.Studio.AddFemale))]
        [HarmonyPatch(typeof(Studio.Studio), nameof(Studio.Studio.AddMale))]
        [HarmonyPatch(typeof(Studio.Studio), nameof(Studio.Studio.LoadScene))]
        public static void Studio_SetupProbes_Patch() => SetupProbes();
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Studio.Map), "LoadMap")]
        [HarmonyPatch(typeof(Studio.Map), "LoadMapCoroutine")]
        public static void Studio_CleanupLightStuff_Patch()
        {
            LightmapSettings.lightProbes = null;
            LightmapSettings.lightmaps = null;
        }
        
        private static void SetupProbes() 
        {
            var charas = FindObjectsOfType<ChaControl>();

            foreach (var cha in charas)
            {
                var t = cha.transform.Find("BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips");
                
                foreach (var renderer in cha.GetComponentsInChildren<Renderer>(true)) 
                {
                    renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
                    renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
                    renderer.probeAnchor = t;
                }
            }
        }
    }
}
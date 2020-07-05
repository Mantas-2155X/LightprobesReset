using System.Collections.Generic;

using BepInEx;
using HarmonyLib;

using AIChara;

using UnityEngine;
using UnityEngine.Rendering;

namespace AI_LightprobesReset
{
    [BepInPlugin(nameof(AI_LightprobesReset), nameof(AI_LightprobesReset), VERSION)][BepInProcess("StudioNEOV2")]
    public class AI_LightprobesReset : BaseUnityPlugin
    {
        public const string VERSION = "1.1.0";

        private void Awake()
        {
            var harmony = new Harmony(nameof(AI_LightprobesReset));
            harmony.PatchAll(typeof(AI_LightprobesReset));
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
        
        [HarmonyPostfix, HarmonyPatch(typeof(Studio.Studio), nameof(Studio.Studio.AddFemale))]
        public static void AddFemale() 
        {
            FixProbe.SetupProbes();
        }
        
        [HarmonyPostfix, HarmonyPatch(typeof(Studio.Studio), nameof(Studio.Studio.AddMale))]
        public static void AddMale() 
        {
            FixProbe.SetupProbes();
        }
        
        [HarmonyPostfix, HarmonyPatch(typeof(Studio.Studio), nameof(Studio.Studio.LoadScene))]
        public static void LoadScene() 
        {
            FixProbe.SetupProbes();
        }
    }
    
    public static class FixProbe 
    {
        private static Transform FindDeepChildPlease(this Transform aParent, string aName) 
        {
            var queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            
            while (queue.Count > 0) 
            {
                var c = queue.Dequeue();
                
                if (c.name == aName)
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }
            
            return null;
        }
        public static void SetupProbes() 
        {
            var charas = Object.FindObjectsOfType<ChaControl>();
            foreach (var cha in charas)
                SetupProbes(cha);
        }
        
        private static void SetupProbes(ChaControl chara) 
        {
            var t = FindDeepChildPlease(chara.transform, "cf_J_Hips");
            foreach (var renderer in chara.GetComponentsInChildren<Renderer>(true)) 
            {
                renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
                renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
                renderer.probeAnchor = t;
            }
        }
    }

}
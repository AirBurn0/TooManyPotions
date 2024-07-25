// #define PATCH_RESOURCES

#if PATCH_RESOURCES

namespace TooManyPotions.Patches
{
    [HarmonyPatch(typeof(Resources))]
    internal class ResourcesPatcher
    {
        [HarmonyPatch("Load", new Type[] { typeof(string), typeof(Type) })]
        [HarmonyPostfix]
        public static void Load(string path, Type systemTypeInstance)
        {
            ModInfo.Log("Tried to load sprite at path:  \t" + path, BepInEx.Logging.LogLevel.Message);
        }


        [HarmonyPatch("LoadAll", new Type[] { typeof(string), typeof(Type) })]
        [HarmonyPostfix]
        public static void LoadAll(string path, Type systemTypeInstance)
        {
            ModInfo.Log("Tried to load ALL sprites at path:  \t" + path, BepInEx.Logging.LogLevel.Message);
        }
    }
}

#endif
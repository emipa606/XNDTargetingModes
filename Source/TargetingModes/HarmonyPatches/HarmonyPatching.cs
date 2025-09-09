using System.Reflection;
using HarmonyLib;
using Verse;

namespace TargetingModes;

[StaticConstructorOnStartup]
public static class HarmonyPatching
{
    static HarmonyPatching()
    {
        new Harmony("XeoNovaDan.TargetingModes").PatchAll(Assembly.GetExecutingAssembly());
    }
}
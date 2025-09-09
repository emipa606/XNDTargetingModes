using System.Linq;
using Verse;

namespace TargetingModes;

public static class ModCompatibilityCheck
{
    public static bool CombatTweaks =>
        ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name.Contains("[XND] Combat Tweaks"));
}
using HarmonyLib;
using UnityEngine;
using Verse;

namespace TargetingModes;

[HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitFactorFromShooter))]
public static class ShotReport_HitFactorFromShooter
{
    public static void Postfix(ref float __result, Thing caster)
    {
        var targetingMode = caster.TryGetComp<CompTargetingMode>();
        if (targetingMode == null)
        {
            return;
        }

        __result = Mathf.Max(__result * targetingMode.GetTargetingMode().HitChanceFactor, 0.0201f);
    }
}
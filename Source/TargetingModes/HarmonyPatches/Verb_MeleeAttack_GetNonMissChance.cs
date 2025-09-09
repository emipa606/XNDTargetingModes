using HarmonyLib;
using RimWorld;
using Verse;

namespace TargetingModes;

[HarmonyPatch(typeof(Verb_MeleeAttack), "GetNonMissChance")]
public static class Verb_MeleeAttack_GetNonMissChance
{
    public static void Postfix(Verb_MeleeAttack __instance, ref float __result)
    {
        var caster = __instance.caster;

        var compTargetingMode = caster?.TryGetComp<CompTargetingMode>();
        if (compTargetingMode != null && __result == caster.GetStatValue(StatDefOf.MeleeHitChance))
        {
            __result *= __result * compTargetingMode.GetTargetingMode().HitChanceFactor;
        }
    }
}
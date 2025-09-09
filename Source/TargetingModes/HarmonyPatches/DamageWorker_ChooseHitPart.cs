using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace TargetingModes;

[HarmonyPatch]
public static class DamageWorker_ChooseHitPart
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(DamageWorker_AddInjury), "ChooseHitPart");
        yield return AccessTools.Method(typeof(DamageWorker_Blunt), "ChooseHitPart");
        yield return AccessTools.Method(typeof(DamageWorker_Bite), "ChooseHitPart");
        yield return AccessTools.Method(typeof(DamageWorker_Cut), "ChooseHitPart");
        yield return AccessTools.Method(typeof(DamageWorker_Scratch), "ChooseHitPart");
    }

    public static void Postfix(ref BodyPartRecord __result, DamageInfo dinfo, Pawn pawn)
    {
        __result = TargetingModesUtility.ResolvePrioritizedPart_External(__result, dinfo, pawn);
    }
}
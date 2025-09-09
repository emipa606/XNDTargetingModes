using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace TargetingModes;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
public static class Pawn_GetGizmos
{
    public static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
    {
        __result = modifiedGizmoEnumerable(__instance, __result);
    }

    private static IEnumerable<Gizmo> modifiedGizmoEnumerable(Pawn pawn, IEnumerable<Gizmo> result)
    {
        foreach (var item in result)
        {
            yield return item;
        }

        if (!pawn.IsPlayerControlledAnimal())
        {
            yield break;
        }

        var targetingMode = pawn.TryGetComp<CompTargetingMode>();
        if (targetingMode == null)
        {
            yield break;
        }

        foreach (var item2 in targetingMode.CompGetGizmosExtra())
        {
            yield return item2;
        }
    }
}
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace TargetingModes;

[HarmonyPatch(typeof(PawnGroupMakerUtility), nameof(PawnGroupMakerUtility.GeneratePawns))]
public static class PawnGroupMakerUtility_GeneratePawns
{
    public static void Postfix(ref IEnumerable<Pawn> __result, PawnGroupMakerParms parms)
    {
        foreach (var item in __result)
        {
            if (item.RaceProps.Humanlike && parms.raidStrategy == TM_RaidStrategyDefOf.ImmediateAttackSmart &&
                item.IsCompetentWithWeapon() || item.RaceProps.IsMechanoid &&
                Rand.Chance(TargetingModesSettings.MechanoidTargModeChance / 100f))
            {
                item.TryAssignRandomTargetingMode();
            }
        }
    }
}
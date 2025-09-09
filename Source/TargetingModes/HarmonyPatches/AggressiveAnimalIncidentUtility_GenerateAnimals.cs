using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace TargetingModes;

[HarmonyPatch(typeof(AggressiveAnimalIncidentUtility), nameof(AggressiveAnimalIncidentUtility.GenerateAnimals),
    typeof(PawnKindDef), typeof(PlanetTile), typeof(float), typeof(int))]
public static class AggressiveAnimalIncidentUtility_GenerateAnimals
{
    public static void Postfix(ref List<Pawn> __result)
    {
        foreach (var item in __result)
        {
            if (item.TryGetComp<CompTargetingMode>() != null &&
                Rand.Chance(TargetingModesUtility.AdjustedChanceForAnimal(item)))
            {
                item.TryAssignRandomTargetingMode();
            }
        }
    }
}
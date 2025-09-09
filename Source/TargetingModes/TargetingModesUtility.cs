using RimWorld;
using Verse;

namespace TargetingModes;

public static class TargetingModesUtility
{
    private const float TargModeChanceFactorOffsetPerTrainabilityOrder = 0.05f;
    public static readonly TargetingModeDef DefaultTargetingMode = TargetingModeDefOf.Standard;

    public static Command_SetTargetingMode SetTargetModeCommand(ITargetModeSettable isSettable)
    {
        return new Command_SetTargetingMode
        {
            defaultDesc = "CommandSetTargetingModeDesc".Translate().Resolve(),
            settable = isSettable
        };
    }

    private static bool canUseTargetingModes(this Thing instigator, ThingDef weapon)
    {
        if (instigator == null || !instigator.def.HasComp(typeof(CompTargetingMode)) || weapon == null)
        {
            return false;
        }

        switch (instigator)
        {
            case Pawn pawn when pawn.CurrentEffectiveVerb.verbProps.CausesExplosion:
            case Building_Turret turret when turret.CurrentEffectiveVerb.verbProps.CausesExplosion:
                return false;
            default:
                return true;
        }
    }

    public static BodyPartRecord ResolvePrioritizedPart(BodyPartRecord part, DamageInfo dinfo, Pawn pawn)
    {
        var result = part;
        if (!dinfo.Instigator.canUseTargetingModes(dinfo.Weapon) ||
            dinfo.Instigator.TryGetComp<CompTargetingMode>() == null)
        {
            return result;
        }

        var compTargetingMode = dinfo.Instigator.TryGetComp<CompTargetingMode>();
        var targetingMode = compTargetingMode.GetTargetingMode();
        if (!part.isPrioritizedPart(targetingMode))
        {
            result = rerollBodyPart(targetingMode, part, dinfo, pawn);
        }

        return result;
    }

    public static BodyPartRecord ResolvePrioritizedPart_External(BodyPartRecord part, DamageInfo dinfo, Pawn pawn)
    {
        var result = part;
        if (!dinfo.Instigator.canUseTargetingModes(dinfo.Weapon) ||
            dinfo.Instigator?.TryGetComp<CompTargetingMode>() == null)
        {
            return result;
        }

        var compTargetingMode = dinfo.Instigator?.TryGetComp<CompTargetingMode>();
        if (compTargetingMode == null)
        {
            return result;
        }

        var targetingMode = compTargetingMode.GetTargetingMode();
        if (!part.isPrioritizedPart(targetingMode))
        {
            result = rerollBodyPart(targetingMode, part, dinfo.Def, dinfo.Height, BodyPartDepth.Outside, pawn,
                dinfo.Instigator);
        }

        return result;
    }

    private static bool isPrioritizedPart(this BodyPartRecord part, TargetingModeDef targetingMode)
    {
        return targetingMode.HasNoSpecifiedPartDetails || targetingMode.PartsListContains(part.def) ||
               targetingMode.PartsOrAnyChildrenListContains(part) || targetingMode.TagsListContains(part.def.tags);
    }

    private static BodyPartRecord rerollBodyPart(TargetingModeDef targetingMode, BodyPartRecord bodyPart,
        DamageInfo dinfo, Pawn pawn)
    {
        return rerollBodyPart(targetingMode, bodyPart, dinfo.Def, dinfo.Height, dinfo.Depth, pawn, dinfo.Instigator);
    }

    private static BodyPartRecord rerollBodyPart(TargetingModeDef targetingMode, BodyPartRecord bodyPart,
        DamageDef damDef, BodyPartHeight height, BodyPartDepth depth, Pawn pawn, Thing instigator)
    {
        for (var i = 0; i < targetingMode.RerollCount(pawn, instigator); i++)
        {
            var randomNotMissingPart = pawn.health.hediffSet.GetRandomNotMissingPart(damDef, height, depth);
            if (randomNotMissingPart.isPrioritizedPart(targetingMode))
            {
                return randomNotMissingPart;
            }
        }

        return bodyPart;
    }

    public static bool IsCompetentWithWeapon(this Pawn pawn)
    {
        if (pawn.skills == null || pawn.equipment == null)
        {
            return false;
        }

        var primary = pawn.equipment.Primary;
        if (primary != null && primary.def.IsRangedWeapon && pawn.skills.GetSkill(SkillDefOf.Shooting).Level >=
            TargetingModesSettings.MinimumSkillForRandomTargetingMode)
        {
            return true;
        }

        return pawn.skills.GetSkill(SkillDefOf.Melee).Level >=
               TargetingModesSettings.MinimumSkillForRandomTargetingMode;
    }

    public static void TryAssignRandomTargetingMode(this Pawn pawn)
    {
        if (!TargetingModesSettings.RaidersUseTargModes || pawn.TryGetComp<CompTargetingMode>() == null)
        {
            return;
        }

        var compTargetingMode = pawn.TryGetComp<CompTargetingMode>();
        var targetingMode =
            DefDatabase<TargetingModeDef>.AllDefsListForReading.RandomElementByWeight(t => t.commonality);
        compTargetingMode.SetTargetingMode(targetingMode);
    }

    public static float AdjustedChanceForAnimal(Pawn animal)
    {
        var num = animal.RaceProps.trainability.intelligenceOrder - TrainabilityDefOf.Intermediate.intelligenceOrder;
        if (num >= 0)
        {
            return TargetingModesSettings.BaseManhunterTargModeChance / 100f *
                   (1f + (num * TargModeChanceFactorOffsetPerTrainabilityOrder));
        }

        return 0f;
    }

    public static bool IsPlayerControlledAnimal(this Pawn pawn)
    {
        return pawn.Spawned && pawn.MentalStateDef == null && pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer;
    }
}
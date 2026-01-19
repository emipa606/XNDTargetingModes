using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace TargetingModes;

public class CompTargetingMode : ThingComp, ITargetModeSettable
{
    private const int TargetModeResetCheckInterval = 60;

    private int resetTargetingModeTick = -1;

    private TargetingModeDef targetingMode = TargetingModesUtility.DefaultTargetingMode;

    private Pawn Pawn => parent as Pawn;

    public TargetingModeDef GetTargetingMode()
    {
        return targetingMode;
    }

    public void SetTargetingMode(TargetingModeDef targetMode)
    {
        targetingMode = targetMode;
        resetTargetingModeTick = Find.TickManager.TicksGame + targModeResetAttemptInterval();
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (Pawn?.def.thingClass.Name.EndsWith("VehiclePawn") == true)
        {
            yield break;
        }

        if (parent.Faction == Faction.OfPlayer && (Pawn == null ||
                                                   Pawn.training != null &&
                                                   Pawn.training.HasLearned(TrainableDefOf.Obedience) || Pawn.Drafted))
        {
            yield return TargetingModesUtility.SetTargetModeCommand(this);
        }
    }

    public override void CompTick()
    {
        base.CompTick();
        if (targetingMode == null || TargetingModesSettings.TargModeResetFrequencyInt == 6 && Pawn?.Drafted == false)
        {
            SetTargetingMode(TargetingModesUtility.DefaultTargetingMode);
            return;
        }

        if (!parent.IsHashIntervalTick(TargetModeResetCheckInterval))
        {
            return;
        }

        if (CanResetTargetingMode())
        {
            SetTargetingMode(TargetingModesUtility.DefaultTargetingMode);
        }
        else
        {
            resetTargetingModeTick = Find.TickManager.TicksGame + targModeResetAttemptInterval();
        }
    }

    private bool CanResetTargetingMode()
    {
        if (parent.Map == null)
        {
            return Find.TickManager.TicksGame < resetTargetingModeTick;
        }

        if (TargetingModesSettings.TargModeResetFrequencyInt == 0 ||
            targetingMode == TargetingModesUtility.DefaultTargetingMode ||
            Find.TickManager.TicksGame < resetTargetingModeTick)
        {
            return false;
        }

        return Pawn == null || !Pawn.Drafted && !GenAI.InDangerousCombat(Pawn);
    }

    private int targModeResetAttemptInterval()
    {
        return (parent.Faction == Faction.OfPlayer ? TargetingModesSettings.TargModeResetFrequencyInt : 3) switch
        {
            1 => 60000,
            2 => 30000,
            3 => 15000,
            4 => 7500,
            _ => 2500
        };
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Defs.Look(ref targetingMode, "targetingMode");
        Scribe_Values.Look(ref resetTargetingModeTick, "resetTargetingModeTick", -1);
    }

    public override string ToString()
    {
        return
            $"CompTargetingMode for {parent} :: _targetingMode={targetingMode.LabelCap}; _resetTargetingModeTick={resetTargetingModeTick}; (TicksGame={Find.TickManager.TicksGame})";
    }
}
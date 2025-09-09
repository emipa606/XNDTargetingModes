using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TargetingModes;

[StaticConstructorOnStartup]
public class Command_SetTargetingMode : Command
{
    private static readonly Texture2D setTargetingModeTex =
        ContentFinder<Texture2D>.Get("UI/TargetingModes/MultipleModes");

    public ITargetModeSettable settable;

    private List<ITargetModeSettable> settables;

    public Command_SetTargetingMode()
    {
        TargetingModeDef targetingModeDef = null;
        var foundTargetingModeComp = false;
        foreach (Thing selectedObject in Find.Selector.SelectedObjects)
        {
            if (selectedObject.TryGetComp<CompTargetingMode>() == null)
            {
                continue;
            }

            if (selectedObject.TryGetComp<CompTargetingMode>().GetTargetingMode() == null)
            {
                foundTargetingModeComp = true;
                break;
            }

            targetingModeDef = selectedObject.TryGetComp<CompTargetingMode>().GetTargetingMode();
        }

        if (targetingModeDef == null)
        {
            return;
        }

        icon = foundTargetingModeComp ? setTargetingModeTex : targetingModeDef.uiIcon;
        defaultLabel = foundTargetingModeComp
            ? "CommandSetTargetingModeMulti".Translate()
            : "CommandSetTargetingMode".Translate(targetingModeDef.LabelCap);
    }

    private static List<TargetingModeDef> TargetingModes => DefDatabase<TargetingModeDef>.AllDefsListForReading;

    public override void ProcessInput(Event ev)
    {
        base.ProcessInput(ev);
        settables ??= [];

        if (!settables.Contains(settable))
        {
            settables.Add(settable);
        }

        TargetingModes.SortBy(t => -t.displayOrder, t => t.LabelCap.Resolve());
        var list = new List<FloatMenuOption>();
        foreach (var targetMode in TargetingModes)
        {
            list.Add(new FloatMenuOption(floatMenuLabel(targetMode), delegate
            {
                foreach (var targetModeSettable in settables)
                {
                    targetModeSettable.SetTargetingMode(targetMode);
                }
            }));
        }

        Find.WindowStack.Add(new FloatMenu(list));
    }

    private static string floatMenuLabel(TargetingModeDef targetingMode)
    {
        string text = targetingMode.LabelCap;
        if (targetingMode.HitChanceFactor != 1f)
        {
            text = $"{text} (x{targetingMode.HitChanceFactor.ToStringPercent()} Acc)";
        }

        return text;
    }

    public override bool InheritInteractionsFrom(Gizmo other)
    {
        settables ??= [];

        settables.Add(((Command_SetTargetingMode)other).settable);
        return false;
    }
}
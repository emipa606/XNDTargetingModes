using Verse;

namespace TargetingModes;

[StaticConstructorOnStartup]
public static class StaticConstructorClass
{
    static StaticConstructorClass()
    {
        foreach (var allDef in DefDatabase<ThingDef>.AllDefs)
        {
            if (!typeof(Pawn).IsAssignableFrom(allDef.thingClass))
            {
                continue;
            }

            if (allDef.comps.NullOrEmpty())
            {
                allDef.comps = [];
            }

            allDef.comps.Add(new CompProperties(typeof(CompTargetingMode)));
        }
    }
}
using Mlie;
using UnityEngine;
using Verse;

namespace TargetingModes;

public class TargetingModes : Mod
{
    public static string CurrentVersion;
    public TargetingModesSettings settings;

    public TargetingModes(ModContentPack content)
        : base(content)
    {
        GetSettings<TargetingModesSettings>();
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "TargetingModesSettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        GetSettings<TargetingModesSettings>().DoWindowContents(inRect);
    }
}
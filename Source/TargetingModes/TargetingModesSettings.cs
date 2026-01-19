using UnityEngine;
using Verse;

namespace TargetingModes;

public class TargetingModesSettings : ModSettings
{
    public static bool AccuracyPenalties = true;

    private static float targModeResetFrequencyInt = 3f;

    public static bool RaidersUseTargModes = true;

    private static float raiderMinSkillForTargMode = 8f;

    public static float MechanoidTargModeChance = 0.35f;

    public static float BaseManhunterTargModeChance = 0.2f;

    public static int TargModeResetFrequencyInt => (int)targModeResetFrequencyInt;

    public static int MinimumSkillForRandomTargetingMode => (int)raiderMinSkillForTargMode;

    public void DoWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        var color = GUI.color;
        listingStandard.Begin(rect);
        GUI.color = color;
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("Settings_AccuracyPenalties".Translate(), ref AccuracyPenalties,
            "Settings_AccuracyPenalties_Tooltip".Translate());
        listingStandard.Gap();
        var key = $"Settings_TargModeResetFrequency_{targModeResetFrequencyInt}";
        listingStandard.Label("Settings_TargModeResetFrequency".Translate() + ": " + key.Translate(), -1f,
            key.Translate());
        targModeResetFrequencyInt = (int)listingStandard.Slider(targModeResetFrequencyInt, 0f, 6f);
        listingStandard.GapLine(24f);
        listingStandard.CheckboxLabeled("Settings_RaidersUseTargetingModes".Translate(), ref RaidersUseTargModes,
            "Settings_RaidersUseTargetingModes_Tooltip".Translate());
        listingStandard.Gap();
        if (!RaidersUseTargModes)
        {
            GUI.color = Color.grey;
        }

        listingStandard.Label(
            (TaggedString)string.Concat("Settings_MinRaiderWeaponSkill".Translate() + ": ",
                raiderMinSkillForTargMode.ToString()), -1f, raiderMinSkillForTargMode.ToString());
        raiderMinSkillForTargMode = (int)listingStandard.Slider(raiderMinSkillForTargMode, 0f, 20f);
        listingStandard.Gap();
        listingStandard.Label((TaggedString)
            $"{"Settings_MechTargModeChance".Translate() + ": "}{MechanoidTargModeChance}%", -1f,
            $"{MechanoidTargModeChance}%");
        MechanoidTargModeChance = (int)listingStandard.Slider(MechanoidTargModeChance, 0f, 100f);
        listingStandard.Gap();
        listingStandard.Label((TaggedString)
            $"{"Settings_BaseManhunterTargModeChance".Translate() + ": "}{BaseManhunterTargModeChance}%", -1f,
            $"{BaseManhunterTargModeChance}%");
        BaseManhunterTargModeChance = (int)listingStandard.Slider(BaseManhunterTargModeChance, 0f, 100f);
        GUI.color = color;
        if (TargetingModes.CurrentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("Settings_CurrentModVersion".Translate(TargetingModes.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        Mod.GetSettings<TargetingModesSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref AccuracyPenalties, "accuracyPenalties", true);
        Scribe_Values.Look(ref targModeResetFrequencyInt, "targModeResetFrequencyInt", 3f);
        Scribe_Values.Look(ref RaidersUseTargModes, "raidersUseTargModes", true);
        Scribe_Values.Look(ref raiderMinSkillForTargMode, "raiderMinSkillForTargMode", 8f);
        Scribe_Values.Look(ref MechanoidTargModeChance, "mechanoidTargModeChance", 0.35f);
        Scribe_Values.Look(ref BaseManhunterTargModeChance, "baseManhunterTargModeChance", 0.2f);
    }
}
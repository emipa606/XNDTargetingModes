using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TargetingModes;

public class TargetingModeDef : Def
{
    public readonly float commonality = 1f;

    private readonly float hitChanceFactor = 1f;

    public int displayOrder;

    public string iconPath;

    public List<BodyPartDef> parts;

    public List<BodyPartDef> partsOrAnyChildren;

    private int rerollCount;

    public List<BodyPartTagDef> tags;

    [Unsaved] public Texture2D uiIcon = BaseContent.BadTex;

    public bool HasNoSpecifiedPartDetails =>
        parts.NullOrEmpty() && partsOrAnyChildren.NullOrEmpty() && tags.NullOrEmpty();

    public float HitChanceFactor => TargetingModesSettings.AccuracyPenalties ? hitChanceFactor : 1f;

    public bool PartsListContains(BodyPartDef def)
    {
        return !parts.NullOrEmpty() && parts.Contains(def);
    }

    public bool PartsOrAnyChildrenListContains(BodyPartRecord part)
    {
        if (partsOrAnyChildren.NullOrEmpty())
        {
            return false;
        }

        if (partsOrAnyChildren.Contains(part.def))
        {
            return true;
        }

        if (part.IsCorePart)
        {
            return partsOrAnyChildren.Contains(part.def);
        }

        var bodyPartRecord = part;
        while (bodyPartRecord.parent != null)
        {
            bodyPartRecord = bodyPartRecord.parent;
            if (partsOrAnyChildren.Contains(bodyPartRecord.def))
            {
                return true;
            }
        }

        return false;
    }

    public bool TagsListContains(List<BodyPartTagDef> partTags)
    {
        if (tags.NullOrEmpty())
        {
            return false;
        }

        foreach (var partTag in partTags)
        {
            if (tags.Contains(partTag))
            {
                return true;
            }
        }

        return false;
    }

    public int RerollCount(Pawn pawn, Thing instigator)
    {
        float num = rerollCount;
        var modExtension = pawn.RaceProps.body.GetModExtension<BodyDefExtension>();
        if (modExtension != null && modExtension.targetModeRerollCountFactors.ContainsKey(this))
        {
            num *= modExtension.targetModeRerollCountFactors[this];
        }

        if (pawn.Downed && (pawn.Position - instigator.Position).LengthHorizontal <= 3.9f)
        {
            num *= 7.5f;
        }

        return GenMath.RoundRandom(num);
    }

    public override void PostLoad()
    {
        LongEventHandler.ExecuteWhenFinished(delegate
        {
            if (!iconPath.NullOrEmpty())
            {
                uiIcon = ContentFinder<Texture2D>.Get(iconPath);
            }
        });
    }
}
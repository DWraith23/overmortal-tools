using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using OvermortalTools.Scripts.Enums;
using static OvermortalTools.Scripts.Enums.Quality;

namespace OvermortalTools.Resources;

[GlobalClass, Tool]
public partial class MyrimonExtractor : Resource
{

    private Classification _quality = Classification.Common;
    private int _expRank = 0;
    private int _qualityRank = 0;
    private int _gushRank = 0;
    private int _highRankRank = 0;

    [Export]
    public Classification Quality
    {
        get => _quality;
        set
        {
            _quality = value;
            EmitChanged();
        }
    }

    [Export(PropertyHint.Range, "0,30")]
    public int ExpRank
    {
        get => _expRank;
        set
        {
            _expRank = Math.Clamp(value, 0, 30);
            EmitChanged();
        }
    }

    [Export(PropertyHint.Range, "0,30")]
    public int QualityRank
    {
        get => _qualityRank;
        set
        {
            _qualityRank = Math.Clamp(value, 0, 30);
            EmitChanged();
        }
    }

    [Export(PropertyHint.Range, "0,30")]
    public int GushRank
    {
        get => _gushRank;
        set
        {
            _gushRank = Math.Clamp(value, 0, 30);
            EmitChanged();
        }
    }

    [Export(PropertyHint.Range, "0,30")]
    public int HighRankRank
    {
        get => _highRankRank;
        set
        {
            _highRankRank = Math.Clamp(value, 0, 30);
            EmitChanged();
        }
    }
    
    public float ExpBonus => ExpRank * 0.04f;

    private float QualityChance(Classification classification)
    {
        switch (classification)
        {
            case Classification.Common:
                if (QualityRank > 5) return 0f;
                return 0.5f - QualityRank * 0.01f;
            case Classification.Uncommon:
                if (QualityRank > 10) return 0f;
                return 0.5f - (QualityRank - 5) * 0.01f;
            case Classification.Rare:
                if (QualityRank > 15) return 0f;
                return 0.5f - (QualityRank - 10) * 0.01f;
            case Classification.Epic:
                if (QualityRank > 20) return 0f;
                return 0.5f - (QualityRank - 15) * 0.01f;
            case Classification.Legendary:
                if (QualityRank > 25) return 0f;
                return 0.5f - (QualityRank - 20) * 0.01f;
            case Classification.Mythic:
                if (QualityRank > 30) return 0f;
                return 0.5f - (QualityRank - 25) * 0.01f;
            default:
                return 0f;
        }
    }

    public Dictionary<Classification, float> QualityOdds()
    {
        var classes = Enum.GetValues<Classification>().ToList();
        var dict = new Dictionary<Classification, float>();
        foreach (var classification in classes)
        {
            dict.Add(classification, QualityChance(classification));
        }
        return dict;
    }

}

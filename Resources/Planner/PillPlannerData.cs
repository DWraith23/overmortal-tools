using System;
using System.Linq;
using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources.Planner;

public partial class PillPlannerData : Resource
{
    private static Pill[] Pills => Pill.GeneratePills();

    private int _pillRealmIndex = 0;
    private int _rarePills = 0;
    private int _epicPills = 0;
    private int _legendaryPills = 0;
    private int _totalPillValue = 100;
    private int _bonusPillValue = 0;

    private float _mythicPills = 0.0f;

    [Export]
    public int PillRealmIndex
    {
        get => _pillRealmIndex;
        set
        {
            _pillRealmIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int RarePills
    {
        get => _rarePills;
        set
        {
            _rarePills = value;
            EmitChanged();
        }
    }

    [Export]
    public int EpicPills
    {
        get => _epicPills;
        set
        {
            _epicPills = value;
            EmitChanged();
        }
    }

    [Export]
    public int LegendaryPills
    {
        get => _legendaryPills;
        set
        {
            _legendaryPills = value;
            EmitChanged();
        }
    }

    [Export]
    public int TotalPillValue
    {
        get => _totalPillValue;
        set
        {
            _totalPillValue = value;
            EmitChanged();
        }
    }

    [Export]
    public int BonusPillValue
    {
        get => _bonusPillValue;
        set
        {
            _bonusPillValue = value;
            EmitChanged();
        }
    }

    [Export]
    public float MythicPills
    {
        get => _mythicPills;
        set
        {
            _mythicPills = value;
            EmitChanged();
        }
    }

    public Realm PillRealm => (Realm)PillRealmIndex;

    public float PillBonusMultiplier => TotalPillValue / ((float)TotalPillValue - BonusPillValue);

    private int RarePillsValue => GetPillValue(Quality.Classification.Rare) * RarePills;
    private int EpicPillsValue => GetPillValue(Quality.Classification.Epic) * EpicPills;
    private int LegendaryPillsValue => GetPillValue(Quality.Classification.Legendary) * LegendaryPills;
    private int MythicPillsValue => (int)Math.Floor(GetPillValue(Quality.Classification.Mythic) * MythicPills);

    public int DailyPillValue => RarePillsValue + EpicPillsValue + LegendaryPillsValue + MythicPillsValue;


    private int GetPillValue(Quality.Classification quality)
    {
        var pill = Pills
            .Where(pill => pill.PillQuality == quality)
            .Where(pill => pill.CultivationRealm == PillRealm)
            .FirstOrDefault();
        if (pill == null) return 0;
        return pill.GetValue(PillBonusMultiplier);
    }
}

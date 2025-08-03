using System;
using System.Linq;
using Godot;
using OvermortalTools.Resources.Cultivation;
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

    public float MythicBonus { get; set; } = 1.0f;

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

    public Realm.MajorRealm PillRealm => (Realm.MajorRealm)PillRealmIndex;

    public float PillBonusMultiplier => TotalPillValue / ((float)TotalPillValue - BonusPillValue);

    private long RarePillsValue => GetPillValue(Quality.Classification.Rare) * RarePills;
    private long EpicPillsValue => GetPillValue(Quality.Classification.Epic) * EpicPills;
    private long LegendaryPillsValue => GetPillValue(Quality.Classification.Legendary) * LegendaryPills;
    private long MythicPillsValue => (int)Math.Floor(GetPillValue(Quality.Classification.Mythic) * MythicBonus * MythicPills);

    public long DailyPillValue => RarePillsValue + EpicPillsValue + LegendaryPillsValue + MythicPillsValue;


    private long GetPillValue(Quality.Classification quality)
    {
        var pill = Pills
            .Where(pill => pill.PillQuality == quality)
            .Where(pill => pill.CultivationRealm == PillRealm)
            .FirstOrDefault();
        if (pill == null) return 0;
        return pill.GetValue(PillBonusMultiplier);
    }
}

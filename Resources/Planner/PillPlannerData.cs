using System;
using System.Linq;
using Godot;

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

    public Pill.Realm PillRealm => (Pill.Realm)PillRealmIndex;

    public float PillBonusMultiplier => TotalPillValue / ((float)TotalPillValue - BonusPillValue);

    private int RarePillsValue => GetPillValue(Pill.Quality.Rare) * RarePills;
    private int EpicPillsValue => GetPillValue(Pill.Quality.Epic) * EpicPills;
    private int LegendaryPillsValue => GetPillValue(Pill.Quality.Legendary) * LegendaryPills;
    private int MythicPillsValue => (int)Math.Floor(GetPillValue(Pill.Quality.Mythic) * MythicPills);

    public int DailyPillValue => RarePillsValue + EpicPillsValue + LegendaryPillsValue + MythicPillsValue;


    private int GetPillValue(Pill.Quality quality)
    {
        var pill = Pills
            .Where(pill => pill.PillQuality == quality)
            .Where(pill => pill.CultivationRealm == PillRealm)
            .FirstOrDefault();
        if (pill == null) return 0;
        return pill.GetValue(PillBonusMultiplier);
    }
}

using System;
using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources.Planner;

public partial class ElixirPlannerData : Resource
{
    private int _realmIndex = 0;
    private int _usedElixirs = 0;
    private int _dailyElixirs = 0;
    private int _usedMonspiritia = 0;
    private int _plannedMonspiritia = 0;

    public int RealmIndex
    {
        get => _realmIndex;
        set
        {
            _realmIndex = value;
            EmitChanged();
        }
    }

    public int UsedElixirs
    {
        get => _usedElixirs;
        set
        {
            _usedElixirs = value;
            EmitChanged();
        }
    }

    public int DailyElixirs
    {
        get => _dailyElixirs;
        set
        {
            _dailyElixirs = value;
            EmitChanged();
        }
    }

    public int UsedMonspiritia
    {
        get => _usedMonspiritia;
        set
        {
            _usedMonspiritia = value;
            EmitChanged();
        }
    }

    public int PlannedMonspiritia
    {
        get => _plannedMonspiritia;
        set
        {
            _plannedMonspiritia = value;
            EmitChanged();
        }
    }

    private Realm.Classification Realm => (Realm.Classification)RealmIndex;

    // Monspiritia
    public bool MonspiritiaAvailable => Elixir.MonspiritiaElixirsCount.ContainsKey(Realm);
    public int MaxMonspiritia => MonspiritiaAvailable ? Elixir.MonspiritiaElixirsCount[Realm] : 0;
    public int MonspiritiaRemaining => MaxMonspiritia - UsedMonspiritia;
    public int PlannedMonspiritiaValue => Elixir.GetMonspiritiaTotalValue(Realm, UsedMonspiritia, PlannedMonspiritia);

    // Main Path
    public int UsedValue => Elixir.GetMainPathTotalValue(Realm, 0, UsedElixirs);
    public int CurrentValue => Elixir.GetMainPathElixirValue(Realm, UsedElixirs);
    public int DailyValue => CurrentValue * DailyElixirs;

    // Outputs
    public int GetValueAfterTime(int days) => Elixir.GetMainPathTotalValue(Realm, UsedElixirs, days * DailyElixirs) + PlannedMonspiritiaValue;
    public int GetDailyValue(int days) => days == 0 ? 0 : (int)Math.Floor(GetValueAfterTime(days) / days + 0d);
}

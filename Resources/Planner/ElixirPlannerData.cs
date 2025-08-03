using System;
using Godot;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources.Planner;

public partial class ElixirPlannerData : Resource
{
    private int _realmIndex = 0;
    private int _usedElixirs = 0;
    private int _dailyElixirs = 0;
    private int _usedMonspiritia = 0;
    private int _plannedMonspiritia = 0;

    [Export]
    public int RealmIndex
    {
        get => _realmIndex;
        set
        {
            _realmIndex = value;
            EmitChanged();
        }
    }
    
    [Export]
    public int UsedElixirs
    {
        get => _usedElixirs;
        set
        {
            _usedElixirs = value;
            EmitChanged();
        }
    }

    [Export]
    public int DailyElixirs
    {
        get => _dailyElixirs;
        set
        {
            _dailyElixirs = value;
            EmitChanged();
        }
    }

    [Export]
    public int UsedMonspiritia
    {
        get => _usedMonspiritia;
        set
        {
            _usedMonspiritia = value;
            EmitChanged();
        }
    }

    [Export]
    public int PlannedMonspiritia
    {
        get => _plannedMonspiritia;
        set
        {
            _plannedMonspiritia = value;
            EmitChanged();
        }
    }

    private Realm.MajorRealm Realm => (Realm.MajorRealm)RealmIndex;

    // Monspiritia
    public bool MonspiritiaAvailable => Elixir.MonspiritiaElixirsCount.ContainsKey(Realm);
    public int MaxMonspiritia => MonspiritiaAvailable ? Elixir.MonspiritiaElixirsCount[Realm] : 0;
    public int MonspiritiaRemaining => MaxMonspiritia - UsedMonspiritia;
    public long PlannedMonspiritiaValue => Elixir.GetMonspiritiaTotalValue(Realm, UsedMonspiritia, PlannedMonspiritia);

    // Main Path
    public long UsedValue => Elixir.GetMainPathTotalValue(Realm, 0, UsedElixirs);
    public long CurrentValue => Elixir.GetMainPathElixirValue(Realm, UsedElixirs);
    public long DailyValue => CurrentValue * DailyElixirs;

    // Outputs
    public long GetValueAfterTime(int days) => Elixir.GetMainPathTotalValue(Realm, UsedElixirs, days * DailyElixirs) + PlannedMonspiritiaValue;
    public long GetDailyValue(int days) => days == 0 ? 0 : (int)Math.Floor(GetValueAfterTime(days) / days + 0d);
}

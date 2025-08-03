using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Scripts.Enums;
using System;

namespace OvermortalTools.Scenes.Planner.Components;

public partial class RealmElixirData : HBoxContainer
{
    [Export] private Label RealmName { get; set; }
    [Export] private SpinBox FlatCount { get; set; }
    [Export] private SpinBox DailyCount { get; set; }
    [Export] private SpinBox UsedCount { get; set; }

    public int FlatElixirAmount => (int)FlatCount.Value;
    public int DailyElixirAmount => (int)DailyCount.Value;
    public int UsedElixirAmount => (int)UsedCount.Value;

    public long ElixirValue => GetElixirValue();

    private int DaysInRealm { get; set; } = 0;

    public void Setup(string realmName, int daysInRealm)
    {
        RealmName.Text = $"{realmName} ({daysInRealm} days)";
        DaysInRealm = daysInRealm;
    }

    private long GetElixirValue()
    {
        var realm = (Realm.MajorRealm)Realm.NamesList.IndexOf(RealmName.Text);
        var flat = Elixir.GetMainPathTotalValue(realm, UsedElixirAmount, FlatElixirAmount);
        var daily = Elixir.GetMainPathTotalValue(realm, UsedElixirAmount + FlatElixirAmount, DailyElixirAmount * DaysInRealm);

        return flat + daily;
    }
}

using Godot;
using System;
using System.Collections.Generic;

namespace OvermortalTools.Scenes.Planner;

public partial class VaseStatus : VBoxContainer
{
    private static Dictionary<int, float> RechargeValues => new()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
        { 3, 2f },
        { 4, 2.4f },
        { 5, 3f }
    };

    [Signal] public delegate void ValuesChangedEventHandler();

    [Export] private CheckBox HasArtifactCheck { get; set; }
    [Export] private HBoxContainer StarsContainer { get; set; }
    [Export] private SpinBox StarsBox { get; set; }

    public int Stars => StarsBox == null ? 0 : (int)StarsBox.Value;

    public float DailyMythicPills => GetDailyMythicPills();

    private float GetDailyMythicPills()
    {
        if (!HasArtifactCheck.ButtonPressed) return 0f;

        var energy = 100f + (96f * RechargeValues[Stars]);
        var basic = energy / 100f;
        return Stars == 5 ? basic * 1.15f : basic;
    }

    public float XpMultiplier =>
        Stars == 0 ? 1f
            : Stars < 3 ? 1.1f
            : 1.2f;

    private void OnToggled(bool on)
    {
        StarsContainer.Visible = on;

        if (!on) StarsBox.Value = 0f;
        EmitSignal(SignalName.ValuesChanged);
    }

    private void OnValueChanged(double value) => EmitSignal(SignalName.ValuesChanged);
}

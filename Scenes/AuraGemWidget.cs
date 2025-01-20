using Godot;
using System;

namespace OvermortalTools.Scenes;

public partial class AuraGemWidget : PanelContainer
{
    [Signal] public delegate void ChangedEventHandler();

    [ExportGroup("Nodes")]
    [Export] public OptionButton SavedSelect { get; set; }
    [Export] private LineEdit BonusField { get; set; }
    [ExportSubgroup("Timings")]
    [Export] private LineEdit PerSecondField { get; set; }
    [Export] private LineEdit PerMinuteField { get; set; }
    [Export] private LineEdit PerHourField { get; set; }
    [Export] private LineEdit PerDayField { get; set; }
    [Export] private LineEdit PerWeekField { get; set; }

    private float Bonus { get; set; } = 0f;
    public float Cosmoapsis { get; set; } = 0f;

    public float ValuePerDay { get; set; } = 0;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

        SavedSelect.AddItem("0%");
        SavedSelect.AddItem("10%");
        SavedSelect.AddItem("13%");
        SavedSelect.AddItem("16%");
        SavedSelect.AddItem("20%");
        SavedSelect.AddItem("24%");
        SavedSelect.AddItem("28%");

        SavedSelect.Select(0);
        SavedSelect.ItemSelected += OnSavedSelectSelected;
    }

    public void OnSavedSelectSelected(long value)
    {
        var index = (int)value;
        Bonus = index switch
        {
            0 => 0,
            1 => 0.1f,
            2 => 0.13f,
            3 => 0.16f,
            4 => 0.2f,
            5 => 0.24f,
            6 => 0.28f,
            _ => 0,
        };

        Update(Cosmoapsis);
    }

    public void Update(float cosmoapsis)
    {
        Cosmoapsis = cosmoapsis;
        var bonus = Cosmoapsis * Bonus;
        var perSecond = bonus / 8;
        BonusField.Text = bonus.ToString("N2");

        PerSecondField.Text = perSecond.ToString("N2");
        PerMinuteField.Text = (perSecond * 60).ToString("N0");
        PerHourField.Text = (perSecond * 60 * 60).ToString("N0");
        PerDayField.Text = (perSecond * 60 * 60 * 24).ToString("N0");
        PerWeekField.Text = (perSecond * 60 * 60 * 24 * 7).ToString("N0");

        ValuePerDay = perSecond * 60 * 60 * 24;
        EmitSignal(SignalName.Changed);
    }
}

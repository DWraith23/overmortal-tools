using Godot;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;
using System;
using System.Collections.Generic;

namespace OvermortalTools.Scenes.Planner;

public partial class PassiveCultivation : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    #region Exports
    [ExportGroup("Nodes")]
    [ExportSubgroup("Input")]
    [Export] private SpinBox Cosmoapsis { get; set; }
    [Export] private OptionButton AuraGem { get; set; }
    [ExportSubgroup("Ouput")]
    [Export] private LineEdit PerMinuteNode { get; set; }
    [Export] private LineEdit HourlyNode { get; set; }
    [Export] private LineEdit DailyNode { get; set; }

    #endregion

    private PassiveCultivationData _data = new();
    public PassiveCultivationData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
            ValidateValues();
            Update();
        }
    }

    public override void _Ready()
    {
        PopulateAuraGemList();
        Data.Changed += Update;
    }

    private void PopulateAuraGemList()
    {
        AuraGem.AddItem("None");
        AuraGem.AddItem("Common");
        AuraGem.AddItem("Uncommon");
        AuraGem.AddItem("Rare");
        AuraGem.AddItem("Epic");
        AuraGem.AddItem("Legendary");
        AuraGem.AddItem("Mythic");

        AuraGem.Select(0);
    }

    #region Events

    private void OnCosmoapsisValueChanged(double value) => Data.Cosmoapsis = (float)value;
    private void OnAuraGemOptionSelected(int index) => Data.AuraGemIndex = index;

    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating PassiveCultivation.");

        PerMinuteNode.Text = Data.CosmoPerMinute.ToString("N0");
        HourlyNode.Text = Data.CosmoPerHour.ToString("N0");
        DailyNode.Text = Data.CosmoPerDay.ToString("N0");

        Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);
    }

    private void ValidateValues()
    {
        Cosmoapsis.Value = Data.Cosmoapsis;
        AuraGem.Select(Data.AuraGemIndex);
    }
}

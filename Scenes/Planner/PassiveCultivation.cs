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
    [Export] private LineEdit Cosmoapsis { get; set; }
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

    private void OnCosmoapsisChanged(string text) => ValidateCosmoapsisText(text);
    private void OnAuraGemOptionSelected(int index) => Data.AuraGemIndex = index;

    #endregion

    #region Actions

    private void ValidateCosmoapsisText(string text)
    {
        if (text == "")
        {
            Data.Cosmoapsis = 0f;
            Cosmoapsis.Text = Data.Cosmoapsis.ToString("N2");
        }
        if (!text.IsValidFloat())
        {
            Cosmoapsis.DeleteLastCharacter();
            return;
        }
        Data.Cosmoapsis = float.Parse(text);
    }
    
    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating PassiveCultivation.");

        PerMinuteNode.Text = Data.CosmoPerMinute.ToString("N0");
        HourlyNode.Text = Data.CosmoPerHour.ToString("N0");
        DailyNode.Text = Data.CosmoPerDay.ToString("N0");

        ValidateValues();

        EmitSignal(SignalName.ValuesChanged);
    }

    private void ValidateValues()
    {
        Cosmoapsis.Text = $"{Data.Cosmoapsis:N2}";
        AuraGem.Select(Data.AuraGemIndex);
    }
}

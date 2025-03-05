using Godot;
using OvermortalTools.Scripts;
using System;
using System.Collections.Generic;

namespace OvermortalTools.Scenes.Planner;

public partial class PassiveCultivation : VBoxContainer
{
    /// <summary>
    /// Dictionary mapping the AuraGem.Selected value to the mulitiplier for Cosmoapsis.
    /// </summary>
    private static Dictionary<int, float> AuraGemValues => new()
    {
        { -1, 0f },
        { 0, 0f },
        { 1, 0.1f },
        { 2, 0.13f },
        { 3, 0.16f },
        { 4, 0.2f },
        { 5, 0.24f },
        { 6, 0.28f },
    };


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
    private float _cosmoapsisValue = 0f;
    public float CosmoapsisValue
    {
        get => _cosmoapsisValue;
        set
        {
            _cosmoapsisValue = value;
            MinuteValue = CosmoPerSecond * 60f;
            HourValue = MinuteValue * 60f;
            DayValue = HourValue * 24f;
        }
    }
    private float AuraGemMultiplier =>
        AuraGem == null ? 0 : AuraGemValues[AuraGem.Selected];
    private float CosmoPerSecond => CosmoapsisValue / 8f;
    public float MinuteValue { get; set; } = 0;
    public float HourValue { get; set; } = 0;
    public float DayValue { get; set; } = 0;

    public override void _Ready()
    {
        PopulateAuraGemList();
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

    private void CosmoapsisChanged(string text) => UpdateValues(text);
    private void AuraGemChanged(long index) => UpdateValues(CosmoapsisValue.ToString("N2"));

    #endregion

    #region Actions

    private void UpdateValues(string text)
    {
        if (text == "")
        {
            CosmoapsisValue = 0f;
            Cosmoapsis.Text = CosmoapsisValue.ToString("N2");
        }
        if (!text.IsValidFloat())
        {
            Cosmoapsis.DeleteLastCharacter();
            return;
        }
        CosmoapsisValue = float.Parse(text);
        PerMinuteNode.Text = (MinuteValue + (MinuteValue * AuraGemMultiplier)).ToString("N0");
        HourlyNode.Text = (HourValue + (HourValue * AuraGemMultiplier)).ToString("N0");
        DailyNode.Text = (DayValue + (DayValue * AuraGemMultiplier)).ToString("N0");
    }
    
    #endregion
}

using Godot;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes;

[GlobalClass]
public partial class BreakthroughCalculator : HBoxContainer
{
    [Export] public CosmoapsisCalculator CosmoapsisCalculator { get; set; }
    [Export] public PillCalculator PillCalculator { get; set; }

    private int PassiveDailyCultivation => (int) Math.Floor(CosmoapsisCalculator.PerDay + CosmoapsisCalculator.AuraGem.ValuePerDay);
    private int ExpToTarget => (int) CosmoapsisCalculator.XPRemaining;


    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        CosmoapsisCalculator.Changed += OnCosmoapsisChanged;
    }

    private void OnCosmoapsisChanged()
    {
        // PillCalculator.DailyExp = PassiveDailyCultivation;
        // PillCalculator.TargetExp = ExpToTarget;
    }

}

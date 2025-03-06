using Godot;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class CultivationPlanner : VBoxContainer
{

    #region Exports
    [ExportGroup("Nodes")]
    [ExportSubgroup("Scenes")]
    [Export] private BasicInformation BasicInformation { get; set; }
    [Export] private PillPlanner PillPlanner { get; set; }
    [Export] private MyrimonPlanner MyrimonPlanner { get; set; }
    [Export] private TargetCalculation TargetCalculation { get; set; }
    [ExportSubgroup("Nodes")]
    [Export] private TabContainer AdvancedTabs { get; set; }

    #endregion

    private int DailyXp => GetDailyXp();

    private int GetDailyXp()
    {
        var result = 1;
        result += (int)BasicInformation.PassiveCultivation.Data.CosmoPerDay;
        result += PillPlanner.Data.DailyPillValue;

        return result;
    }

    #region Events

    private void OnValuesChanged() => UpdateTargetCalculation();

    #endregion

    #region Actions

    private void UpdateTargetCalculation()
    {
        TargetCalculation.CurrentXp = BasicInformation.StageCalculator.Data.CurrentXp;
        TargetCalculation.TargetXp = BasicInformation.StageCalculator.Data.TargetXp;
        TargetCalculation.DailyXp = DailyXp;
        TargetCalculation.MyrimonAverageXp = MyrimonPlanner.Calculator.GetAverageXP();
    }

    #endregion
}

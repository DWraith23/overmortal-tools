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
        result += (int)BasicInformation.PassiveCultivation.DayValue;
        result += PillPlanner.DailyPillValue;

        return result;
    }

    #region Events

    private void OnBasicInformationChanged() => UpdateAdvancedInformation();
    private void OnAdvancedInformationChanged() => UpdateTargetCalculation();

    #endregion

    #region Actions

    private void UpdateAdvancedInformation()
    {
        PillPlanner.SetMythicPills(BasicInformation.VaseStatus.DailyMythicPills + BasicInformation.MirrorStatus.Data.DailyMythicPills);

        UpdateTargetCalculation();
    }

    private void UpdateTargetCalculation()
    {
        TargetCalculation.CurrentXp = BasicInformation.StageCalculator.CurrentXp;
        TargetCalculation.TargetXp = BasicInformation.StageCalculator.TargetXp;
        TargetCalculation.DailyXp = DailyXp;
        TargetCalculation.MyrimonAverageXp = MyrimonPlanner.Calculator.GetAverageXP();
    }

    #endregion
}

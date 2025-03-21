using Godot;
using OvermortalTools.Resources.Planner;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class CultivationPlanner : VBoxContainer
{
    [Signal] public delegate void RequestSaveEventHandler();

    #region Exports
    [ExportGroup("Nodes")]
    [ExportSubgroup("Scenes")]
    [Export] public BasicInformation BasicInformation { get; set; }
    [Export] public PillPlanner PillPlanner { get; set; }
    [Export] public RespiraPlanner RespiraPlanner { get; set; }
    [Export] public MyrimonPlanner MyrimonPlanner { get; set; }
    [Export] public ElixirPlanner ElixirPlanner { get; set; }
    [Export] private TargetCalculation TargetCalculation { get; set; }
    [ExportSubgroup("Nodes")]
    [Export] private TabContainer AdvancedTabs { get; set; }

    #endregion

    #region Events

    private void OnValuesChanged() => UpdateTargetCalculation();

    #endregion

    #region Actions

    private void UpdateTargetCalculation()
    {
        TargetCalculation.CurrentXp = BasicInformation.StageCalculator.Data.CurrentXp;
        TargetCalculation.TargetXp = BasicInformation.StageCalculator.Data.TargetXp;
        TargetCalculation.PassiveXp = (int)BasicInformation.PassiveCultivation.Data.CosmoPerDay;
        TargetCalculation.RespiraXp = RespiraPlanner.Data.DailyRespiraValue;
        TargetCalculation.PillXp = PillPlanner.Data.DailyPillValue;
        TargetCalculation.MyrimonAverageXp = MyrimonPlanner.Data.AverageXp;
        TargetCalculation.ElixirData = ElixirPlanner.Data;

        EmitSignal(SignalName.RequestSave);
    }

    #endregion
}

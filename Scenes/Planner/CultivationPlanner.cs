using Godot;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;
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
        var stageData = BasicInformation.StageCalculator.Data;
        TargetCalculation.XpNeeded = RealmList.GetXpToTarget(stageData.CurrentRealm, stageData.TargetRealm);
        TargetCalculation.PassiveXp = (int)BasicInformation.PassiveCultivation.Data.CosmoPerDay;
        TargetCalculation.RespiraXp = RespiraPlanner.Data.DailyRespiraValue;
        TargetCalculation.PillXp = PillPlanner.Data.DailyPillValue;
        TargetCalculation.MyrimonAverageXp = MyrimonPlanner.Data.AverageXp;
        TargetCalculation.ElixirData = ElixirPlanner.Data;

        Tools.EmitLoggedSignal(this, SignalName.RequestSave);
    }

    #endregion
}

using Godot;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scenes.Planner;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources;

public partial class SaveState : Resource
{
    #region Saved Properties (Exports)
    [Export] public StageCalculatorData StageCalculatorData { get; set; }
    [Export] public PassiveCultivationData PassiveCultivationData { get; set; }
    [Export] public VaseData VaseData { get; set; }
    [Export] public MirrorData MirrorData { get; set; }
    [Export] public PillPlannerData PillPlannerData { get; set; }
    [Export] public MyrimonPlannerData MyrimonPlannerData { get; set; }

    #endregion

    public static SaveState GenerateSaveState(CultivationPlanner planner)
    {
        var result = new SaveState
        {
            StageCalculatorData = planner.BasicInformation.StageCalculator.Data,
            PassiveCultivationData = planner.BasicInformation.PassiveCultivation.Data,
            VaseData = planner.BasicInformation.VaseStatus.Data,
            MirrorData = planner.BasicInformation.MirrorStatus.Data,
            PillPlannerData = planner.PillPlanner.Data,
            MyrimonPlannerData = planner.MyrimonPlanner.Data
        };

        return result;
    }

    public static void LoadSaveState(CultivationPlanner planner, SaveState state)
    {
        planner.BasicInformation.StageCalculator.Data = state.StageCalculatorData;
        planner.BasicInformation.PassiveCultivation.Data = state.PassiveCultivationData;
        planner.BasicInformation.VaseStatus.Data = state.VaseData;
        planner.BasicInformation.MirrorStatus.Data = state.MirrorData;
        planner.PillPlanner.Data = state.PillPlannerData;
        planner.MyrimonPlanner.Data = state.MyrimonPlannerData;
    }

    public static SaveState GenerateFreshState()
    {
        var result = new SaveState
        {
            StageCalculatorData = new StageCalculatorData(),
            PassiveCultivationData = new PassiveCultivationData(),
            VaseData = new VaseData(),
            MirrorData = new MirrorData(),
            PillPlannerData = new PillPlannerData(),
            MyrimonPlannerData = new MyrimonPlannerData()
        };
        return result;
    }
}

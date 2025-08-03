using Godot;
using OvermortalTools.Resources.Laws;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scenes.Laws;
using OvermortalTools.Scenes.Planner;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources;

public partial class SaveState : Resource
{
    #region Saved Properties (Exports)
    [Export] public string ProfileName { get; set; } = "";
    [Export] public StageCalculatorData StageCalculatorData { get; set; }
    [Export] public PassiveCultivationData PassiveCultivationData { get; set; }
    [Export] public VaseData VaseData { get; set; }
    [Export] public MirrorData MirrorData { get; set; }
    [Export] public PillPlannerData PillPlannerData { get; set; }
    [Export] public MyrimonPlannerData MyrimonPlannerData { get; set; }
    [Export] public RespiraPlannerData RespiraPlannerData { get; set; }
    [Export] public ElixirPlannerData ElixirPlannerData { get; set; }
    [Export] public LawsData LawsData { get; set; }

    #endregion

    public static SaveState GenerateSaveState(CultivationPlanner planner, LawSimulator simulator, string name)
    {
        var result = new SaveState
        {
            ProfileName = name,
            StageCalculatorData = planner.BasicInformation.StageCalculator.Data,
            PassiveCultivationData = planner.BasicInformation.PassiveCultivation.Data,
            VaseData = planner.BasicInformation.VaseStatus.Data,
            MirrorData = planner.BasicInformation.MirrorStatus.Data,
            PillPlannerData = planner.PillPlanner.Data,
            MyrimonPlannerData = planner.MyrimonPlanner.Data,
            RespiraPlannerData = planner.RespiraPlanner.Data,
            ElixirPlannerData = planner.ElixirPlanner.Data,
            LawsData = simulator.Data
        };

        return result;
    }

    public static void LoadSaveState(CultivationPlanner planner, LawSimulator simulator, SaveState state)
    {
        planner.BasicInformation.StageCalculator.Data = state.StageCalculatorData ?? new StageCalculatorData();
        planner.BasicInformation.PassiveCultivation.Data = state.PassiveCultivationData ?? new PassiveCultivationData();
        planner.PillPlanner.Data = state.PillPlannerData ?? new PillPlannerData();
        planner.MyrimonPlanner.Data = state.MyrimonPlannerData ?? new MyrimonPlannerData();
        planner.RespiraPlanner.Data = state.RespiraPlannerData ?? new RespiraPlannerData();
        planner.ElixirPlanner.Data = state.ElixirPlannerData ?? new ElixirPlannerData();
        planner.BasicInformation.VaseStatus.Data = state.VaseData ?? new VaseData();
        planner.BasicInformation.MirrorStatus.Data = state.MirrorData ?? new MirrorData();
        simulator.Data = state.LawsData ?? new LawsData();
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
            MyrimonPlannerData = new MyrimonPlannerData(),
            RespiraPlannerData = new RespiraPlannerData(),
            ElixirPlannerData = new ElixirPlannerData(),
            LawsData = new LawsData()
        };
        return result;
    }
}

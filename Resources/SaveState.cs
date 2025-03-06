using Godot;
using OvermortalTools.Scenes.Planner;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources;

public partial class SaveState : Resource
{
    #region Saved Properties (Exports)
    [ExportGroup("Basic")]
    [ExportSubgroup("Stage Calculator")]
    [Export] public int CurrentMajorRealmIndex { get; set; }
    [Export] public int CurrentMinorRealmIndex { get; set; }
    [Export] public int CurrentStageIndex { get; set; }
    [Export] public float CurrentPercent { get; set; }
    [Export] public int TargetMajorRealmIndex { get; set; }
    [Export] public int TargetMinorRealmIndex { get; set; }
    [Export] public int TargetStageIndex { get; set; }
    [ExportSubgroup("Passive Cultivation")]
    [Export] public float Cosmoapsis { get; set; }
    [Export] public int AuraGemIndex { get; set; }
    [ExportSubgroup("Artifacts")]
    [Export] public bool HasVase { get; set; }
    [Export] public int VaseStars { get; set; }
    [Export] public bool HasMirror { get; set; }
    [Export] public int MirrorStars { get; set; }
    [ExportGroup("Advanced")]
    [ExportSubgroup("Pill Planner")]
    [Export] public int PillRealmIndex { get; set; }
    [Export] public int RarePills { get; set; }
    [Export] public int EpicPills { get; set; }
    [Export] public int LegendaryPills { get; set; }
    [Export] public int TotalPillValue { get; set; }
    [Export] public int BonusPillValue { get; set; }
    [ExportSubgroup("Myrimon Planner")]
    [Export] public int ExpLevel { get; set; }
    [Export] public int QualityLevel { get; set; }
    [Export] public int GushLevel { get; set; }
    [Export] public int HighRankLevel { get; set; }
    [Export] public int FruitTypeIndex { get; set; }
    [Export] public int ExtractorQualityIndex { get; set; }
    [Export] public int FruitQuantity { get; set; }
    [Export] public bool RealmMatchCheckBox { get; set; }

    #endregion

    public static SaveState GenerateSaveState(CultivationPlanner planner)
    {
        var result = new SaveState
        {
            
        };

        return result;
    }

    public static void LoadSaveState(CultivationPlanner planner, SaveState state)
    {
        // tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentCosmoapsisValue = state.Cosmoapsis;
        // tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentRealmSelection.Select(state.CurrentRealmIndex);
        // tools.BreakthroughCalculator.CosmoapsisCalculator.OnCurrentRealmSelect(state.CurrentRealmIndex);
        // tools.BreakthroughCalculator.CosmoapsisCalculator.TargetRealmSelection.Select(state.TargetRealmIndex);
        // tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentXPTowards.Text = state.CurrentXPTowards.ToString();
        // tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentXpTowardsValue = state.CurrentXPTowards;
        // tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.Cosmoapsis = state.Cosmoapsis;
        // tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.SavedSelect.Select(state.AuraGemSavedIndex);
        // tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.OnSavedSelectSelected(state.AuraGemSavedIndex);
        // tools.BreakthroughCalculator.PillCalculator.PillBonusValue = state.PillBonusPercent;
        // tools.BreakthroughCalculator.PillCalculator.CurrentRealmSelection.Select(state.CurrentRealmPillsIndex);
        // tools.BreakthroughCalculator.PillCalculator.OnCurrentRealmChanged(state.CurrentRealmPillsIndex);
        // tools.BreakthroughCalculator.PillCalculator.TargetDaysSpinbox.Value = state.TargetDays;
        // tools.BreakthroughCalculator.PillCalculator.DailyCommonsSpinbox.Value = state.DailyCommonPills;
        // tools.BreakthroughCalculator.PillCalculator.DailyUncommonsSpinbox.Value = state.DailyUncommonPills;
        // tools.BreakthroughCalculator.PillCalculator.DailyRaresSpinbox.Value = state.DailyRarePills;
        // tools.BreakthroughCalculator.PillCalculator.DailyEpicsSpinbox.Value = state.DailyEpicPills;
        // tools.BreakthroughCalculator.PillCalculator.DailyLegendarySpinbox.Value = state.DailyLegendaryPills;
        // tools.BreakthroughCalculator.PillCalculator.DailyMythicSpinbox.Value = state.DailyMythicPills;
        // // tools.MyrimonCalculator.Calculator = state.Myrimon;
        // tools.BreakthroughCalculator.CosmoapsisCalculator.Update();
    }
}

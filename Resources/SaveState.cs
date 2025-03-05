using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources;

public partial class SaveState : Resource
{
    [Export] public float Cosmoapsis { get; set; }
    [Export] public int CurrentRealmIndex { get; set; }
    [Export] public int TargetRealmIndex { get; set; }
    [Export] public int CurrentXPTowards { get; set; }
    [Export] public int AuraGemSavedIndex { get; set; }
    [Export] public double PillBonusPercent { get; set; }
    [Export] public int CurrentRealmPillsIndex { get; set; }
    [Export] public int TargetDays { get; set; }
    [Export] public int DailyCommonPills { get; set; }
    [Export] public int DailyUncommonPills { get; set; }
    [Export] public int DailyRarePills { get; set; }
    [Export] public int DailyEpicPills { get; set; }
    [Export] public int DailyLegendaryPills { get; set; }
    [Export] public int DailyMythicPills { get; set; }
    // [Export] public MyrimonCalculation Myrimon { get; set; }

    public static SaveState GenerateSaveState(ToolSelection tools)
    {
        var result = new SaveState
        {
            Cosmoapsis = tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentCosmoapsisValue,
            CurrentRealmIndex = tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentRealmSelection.GetSelectedId(),
            TargetRealmIndex = tools.BreakthroughCalculator.CosmoapsisCalculator.TargetRealmSelection.GetSelectedId(),
            CurrentXPTowards = (int) tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentXpTowards,
            AuraGemSavedIndex = tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.SavedSelect.GetSelectedId(),
            PillBonusPercent = tools.BreakthroughCalculator.PillCalculator.PillBonusValue,
            CurrentRealmPillsIndex = tools.BreakthroughCalculator.PillCalculator.CurrentRealmSelection.GetSelectedId(),
            TargetDays = (int) tools.BreakthroughCalculator.PillCalculator.TargetDaysSpinbox.Value,
            DailyCommonPills = (int) tools.BreakthroughCalculator.PillCalculator.DailyCommonsSpinbox.Value,
            DailyUncommonPills = (int) tools.BreakthroughCalculator.PillCalculator.DailyUncommonsSpinbox.Value,
            DailyRarePills = (int) tools.BreakthroughCalculator.PillCalculator.DailyRaresSpinbox.Value,
            DailyEpicPills = (int) tools.BreakthroughCalculator.PillCalculator.DailyEpicsSpinbox.Value,
            DailyLegendaryPills = (int) tools.BreakthroughCalculator.PillCalculator.DailyLegendarySpinbox.Value,
            DailyMythicPills = (int) tools.BreakthroughCalculator.PillCalculator.DailyMythicSpinbox.Value,
            // Myrimon = tools.MyrimonCalculator.Calculator
        };

        return result;
    }

    public static void LoadSaveState(ToolSelection tools, SaveState state)
    {
        tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentCosmoapsisValue = state.Cosmoapsis;
        tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentRealmSelection.Select(state.CurrentRealmIndex);
        tools.BreakthroughCalculator.CosmoapsisCalculator.OnCurrentRealmSelect(state.CurrentRealmIndex);
        tools.BreakthroughCalculator.CosmoapsisCalculator.TargetRealmSelection.Select(state.TargetRealmIndex);
        tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentXPTowards.Text = state.CurrentXPTowards.ToString();
        tools.BreakthroughCalculator.CosmoapsisCalculator.CurrentXpTowardsValue = state.CurrentXPTowards;
        tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.Cosmoapsis = state.Cosmoapsis;
        tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.SavedSelect.Select(state.AuraGemSavedIndex);
        tools.BreakthroughCalculator.CosmoapsisCalculator.AuraGem.OnSavedSelectSelected(state.AuraGemSavedIndex);
        tools.BreakthroughCalculator.PillCalculator.PillBonusValue = state.PillBonusPercent;
        tools.BreakthroughCalculator.PillCalculator.CurrentRealmSelection.Select(state.CurrentRealmPillsIndex);
        tools.BreakthroughCalculator.PillCalculator.OnCurrentRealmChanged(state.CurrentRealmPillsIndex);
        tools.BreakthroughCalculator.PillCalculator.TargetDaysSpinbox.Value = state.TargetDays;
        tools.BreakthroughCalculator.PillCalculator.DailyCommonsSpinbox.Value = state.DailyCommonPills;
        tools.BreakthroughCalculator.PillCalculator.DailyUncommonsSpinbox.Value = state.DailyUncommonPills;
        tools.BreakthroughCalculator.PillCalculator.DailyRaresSpinbox.Value = state.DailyRarePills;
        tools.BreakthroughCalculator.PillCalculator.DailyEpicsSpinbox.Value = state.DailyEpicPills;
        tools.BreakthroughCalculator.PillCalculator.DailyLegendarySpinbox.Value = state.DailyLegendaryPills;
        tools.BreakthroughCalculator.PillCalculator.DailyMythicSpinbox.Value = state.DailyMythicPills;
        // tools.MyrimonCalculator.Calculator = state.Myrimon;
        tools.BreakthroughCalculator.CosmoapsisCalculator.Update();
    }
}

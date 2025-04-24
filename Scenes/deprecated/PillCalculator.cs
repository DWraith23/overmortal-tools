using Godot;
using OvermortalTools.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OvermortalTools.Scenes;

public partial class PillCalculator : PanelContainer
{
    // #region Exports
    // [ExportGroup("Nodes")]
    // [ExportSubgroup("Pill Values")]
    // [Export] private LineEdit PillBonusInput { get; set; }
    // [Export] private GridContainer PillValuesContainer { get; set; }
    // [Export] private PillBonusCalculatorPopup Popup { get; set; }
    // [ExportSubgroup("Selections")]
    // [Export] public OptionButton CurrentRealmSelection { get; set; }
    // [Export] public SpinBox TargetDaysSpinbox { get; set; }
    // [Export] private SpinBox TargetHoursSpinbox { get; set; }
    // [Export] private OptionButton MinimumQualitySelection { get; set; }
    // [Export] private SpinBox DailyMythicsSpinbox { get; set; }
    // [ExportSubgroup("Calculations")]
    // [Export] private LineEdit PassiveCultivationExp { get; set; }
    // [Export] private LineEdit TargetExpLine { get; set; }
    // [Export] private LineEdit MissingExpLine { get; set; }
    // [ExportSubgroup("Daily Pills")]
    // [Export] public SpinBox DailyCommonsSpinbox { get; set; }
    // [Export] public SpinBox DailyUncommonsSpinbox { get; set; }
    // [Export] public SpinBox DailyRaresSpinbox { get; set; }
    // [Export] public SpinBox DailyEpicsSpinbox { get; set; }
    // [Export] public SpinBox DailyLegendarySpinbox { get; set; }
    // [Export] public SpinBox DailyMythicSpinbox { get; set; }
    // [Export] private LineEdit CommonValueLine { get; set; }
    // [Export] private LineEdit UncommonValueLine { get; set; }
    // [Export] private LineEdit RareValueLine { get; set; }
    // [Export] private LineEdit EpicValueLine { get; set; }
    // [Export] private LineEdit LegendaryValueLine { get; set; }
    // [Export] private LineEdit MythicValueLine { get; set; }
    // [ExportSubgroup("Final Calculations")]
    // [Export] private LineEdit CurrentPercentLine { get; set; }
    // [Export] private LineEdit XPPerDayLine { get; set; }
    // [Export] private LineEdit PercentDailyLine { get; set; }
    // [Export] private LineEdit FinalExpLine { get; set; }
    // [Export] private LineEdit FinalMissingExpLine { get; set; }

    // #endregion

    // #region Properties
    // private double _pillBonusValue = 1d;
    // public double PillBonusValue
    // {
    //     get => _pillBonusValue;
    //     set
    //     {
    //         _pillBonusValue = value;
    //         PillBonusInput.Text = $"{value:P0}";
    //         UpdatePillValues();
    //         DailyPillsChanged(0);
    //     }
    // }

    // private Pill.Realm _currentRealm = Pill.Realm.Connection;
    // public Pill.Realm CurrentRealm
    // {
    //     get => _currentRealm;
    //     set
    //     {
    //         _currentRealm = value;
    //         DailyPillsChanged(0);
    //     }
    // }

    // private Dictionary<Pill, Label> PillValues { get; set; } = [];

    // private long CommonPillValue => PillValues
    //     .Where(kvp => kvp.Key.CultivationRealm == CurrentRealm)
    //     .Where(kvp => kvp.Key.PillQuality == Pill.Quality.Common)
    //     .First().Key.GetValue((float)PillBonusValue);

    // private long UncommonPillValue => PillValues
    //     .Where(kvp => kvp.Key.CultivationRealm == CurrentRealm)
    //     .Where(kvp => kvp.Key.PillQuality == Pill.Quality.Uncommon)
    //     .First().Key.GetValue((float)PillBonusValue);

    // private long RarePillValue => PillValues
    //     .Where(kvp => kvp.Key.CultivationRealm == CurrentRealm)
    //     .Where(kvp => kvp.Key.PillQuality == Pill.Quality.Rare)
    //     .First().Key.GetValue((float)PillBonusValue);

    // private long EpicPillValue => PillValues
    //     .Where(kvp => kvp.Key.CultivationRealm == CurrentRealm)
    //     .Where(kvp => kvp.Key.PillQuality == Pill.Quality.Epic)
    //     .First().Key.GetValue((float)PillBonusValue);

    // private long LegendaryPillValue => PillValues
    //     .Where(kvp => kvp.Key.CultivationRealm == CurrentRealm)
    //     .Where(kvp => kvp.Key.PillQuality == Pill.Quality.Legendary)
    //     .First().Key.GetValue((float)PillBonusValue);

    // private long MythicPillValue => PillValues
    //     .Where(kvp => kvp.Key.CultivationRealm == CurrentRealm)
    //     .Where(kvp => kvp.Key.PillQuality == Pill.Quality.Mythic)
    //     .First().Key.GetValue((float)PillBonusValue);

    // private int CommonPillsPerDay => (int) Math.Floor(DailyCommonsSpinbox.Value);
    // private int UncommonPillsPerDay => (int) Math.Floor(DailyUncommonsSpinbox.Value);
    // private int RarePillsPerDay => (int) Math.Floor(DailyRaresSpinbox.Value);
    // private int EpicPillsPerDay => (int) Math.Floor(DailyEpicsSpinbox.Value);
    // private int LegendaryPillsPerDay => (int) Math.Floor(DailyLegendarySpinbox.Value);
    // private int MythicsPillsPerDay { get; set; } = 0;

    // // Set from BreakthroughCalculator
    // private long _dailyExp = 0;
    // private long _targetExp = 0;
    // public long DailyExp
    // {
    //     get => _dailyExp;
    //     set
    //     {
    //         _dailyExp = value;
    //         PassiveCultivationExp.Text = value.ToString("N0");
    //         UpdateFinalCalculations();
    //     }
    // }
    // public long TargetExp
    // {
    //     get => _targetExp;
    //     set
    //     {
    //         _targetExp = value;
    //         TargetExpLine.Text = value.ToString("N0");
    //         UpdateFinalCalculations();
    //     }
    // }

    // #endregion

    // // Called when the node enters the scene tree for the first time.
    // public override void _Ready()
    // {
    //     PopulatePillValues();
    //     PopulateRealmList();
    // }

    // private void PopulatePillValues()
    // {
    //     var pills = Pill.GeneratePills();
    //     foreach (var value in Enum.GetValues<Pill.Realm>())
    //     {
    //         var name = Enum.GetName(Pill.Realm.Connection.GetType(), value);
    //         var columnLabel = new Label() { Text = name };
    //         PillValuesContainer.AddChild(columnLabel);
    //         foreach (var pill in pills)
    //         {
    //             if (pill.CultivationRealm == value)
    //             {
    //                 var pillLabel = new Label() { 
    //                     Text = pill.GetValue((float) PillBonusValue).ToString("N0"),
    //                     HorizontalAlignment = HorizontalAlignment.Center };
    //                 PillValuesContainer.AddChild(pillLabel);
    //                 PillValues.Add(pill, pillLabel);
    //             }
    //         }
    //     }

    // }

    // private void UpdatePillValues()
    // {
    //     foreach (var kvp in PillValues)
    //     {
    //         kvp.Value.Text = kvp.Key.GetValue((float) PillBonusValue).ToString("N0");
    //     }
    // }

    // private void PopulateRealmList()
    // {
    //     foreach (var value in Enum.GetValues<Pill.Realm>())
    //     {
    //         CurrentRealmSelection.AddItem(Enum.GetName(Pill.Realm.Connection.GetType(), value));
    //     }
    // }

    // private void OnPopupDisplayButtonPressed() => Popup.Popup();

    // private void OnPercentageApplied(double value)
    // {
    //     PillBonusValue = value;
    //     UpdatePillValues();
    // }

    // public void OnCurrentRealmChanged(int index) => CurrentRealm = Enum.GetValues<Pill.Realm>()[index];

    // private void DailyPillsChanged(double value)
    // {
    //     CommonValueLine.Text = (CommonPillsPerDay * CommonPillValue).ToString("N0");
    //     UncommonValueLine.Text = (UncommonPillsPerDay * UncommonPillValue).ToString("N0");
    //     RareValueLine.Text = (RarePillsPerDay * RarePillValue).ToString("N0");
    //     EpicValueLine.Text = (EpicPillsPerDay * EpicPillValue).ToString("N0");
    //     LegendaryValueLine.Text = (LegendaryPillsPerDay * LegendaryPillValue).ToString("N0");
    //     MythicValueLine.Text = (MythicsPillsPerDay * MythicPillValue).ToString("N0");
    //     UpdateFinalCalculations();
    //     _ = value;
    // }

    // private void MythicDailyChanged(double value)
    // {
    //     MythicsPillsPerDay = (int) Math.Floor(value);
    //     DailyMythicSpinbox.Value = value;
    //     DailyPillsChanged(0);
    // }

    // private void UpdateFinalCalculations()
    // {
    //     var dailyXP = 
    //         CommonPillsPerDay * CommonPillValue +
    //         UncommonPillsPerDay * UncommonPillValue +
    //         RarePillsPerDay * RarePillValue +
    //         EpicPillsPerDay * EpicPillValue +
    //         LegendaryPillsPerDay * LegendaryPillValue +
    //         MythicsPillsPerDay * MythicPillValue +
    //         DailyExp;

    //     XPPerDayLine.Text = dailyXP.ToString("N0");
    //     FinalExpLine.Text = (TargetDaysSpinbox.Value * dailyXP).ToString("N0");
    //     FinalMissingExpLine.Text = (TargetExp - (TargetDaysSpinbox.Value * dailyXP)).ToString("N0");
    // }

}

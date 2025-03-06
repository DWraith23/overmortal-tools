using System;
using System.Linq;
using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Scripts;

namespace OvermortalTools.Scenes.Planner;

public partial class PillPlanner : VBoxContainer
{

    private static Pill[] Pills => Pill.GeneratePills();

    [Signal] public delegate void ValuesChangedEventHandler();

    #region Exports

    [ExportGroup("Nodes")]
    [ExportSubgroup("Inputs")]
    [Export] private OptionButton RealmSelect { get; set; }
    [Export] private SpinBox RarePills { get; set; }
    [Export] private SpinBox EpicPills { get; set; }
    [Export] private SpinBox LegendaryPills { get; set; }
    [Export] private LineEdit TotalPillInput { get; set; }
    [Export] private LineEdit BonusPillInput { get; set; }
    [ExportSubgroup("Outputs")]
    [Export] private LineEdit MythicPills { get; set; }
    [Export] private LineEdit DailyValue { get; set; }
    [Export] private LineEdit PillCount { get; set; }
    [Export] private LineEdit BonusPercentOutput { get; set; }

    #endregion

    private float TotalTestPillValue { get; set; } = 100;
    private float BonusTestPillValue { get; set; } = 0;
    public float Multiplier => TotalTestPillValue / (TotalTestPillValue - BonusTestPillValue);
    private Pill.Realm Realm { get; set; } = Pill.Realm.Connection;


    private int RarePillsCount => (int)RarePills.Value;
    private int EpicPillsCount => (int)EpicPills.Value;
    private int LegendaryPillsCount => (int)LegendaryPills.Value;
    private float MythicPillsCount => float.Parse(MythicPills.Text);

    private int RarePillsValue => GetPillValue(Pill.Quality.Rare) * RarePillsCount;
    private int EpicPillsValue => GetPillValue(Pill.Quality.Epic) * EpicPillsCount;
    private int LegendaryPillsValue => GetPillValue(Pill.Quality.Legendary) * LegendaryPillsCount;
    private int MythicPillsValue => (int)Math.Floor(GetPillValue(Pill.Quality.Mythic) * MythicPillsCount);

    public int DailyPillValue => RarePillsValue + EpicPillsValue + LegendaryPillsValue + MythicPillsValue;


    private int GetPillValue(Pill.Quality quality)
    {
        var pill = Pills
            .Where(pill => pill.PillQuality == quality)
            .Where(pill => pill.CultivationRealm == Realm)
            .FirstOrDefault();
        if (pill == null) return 0;
        return pill.GetValue(Multiplier);
    }

    public override void _Ready()
    {
        UpdateTextBoxes();
        CultivationStage.MajorRealms.ForEach(realm => RealmSelect.AddItem(realm));
    }

    #region Events
    private void SpinBoxChanged(double value) => UpdateTextBoxes();
    private void RealmSelected(int index) => SetRealm(index);

    private void TotalPillInputChanged(string text) => UpdateTotalPillsInputText(text);
    private void BonusPillInputChanged(string text) => UpdatePillBonusInputText(text);

    #endregion

    public void SetMythicPills(float dailyMythics)
    {
        MythicPills.Text = dailyMythics.ToString("N2");
        UpdateTextBoxes();
    }

    public void SetRealm(int index)
    {
        Realm = (Pill.Realm)index;
        UpdateTextBoxes();
    }



    private void UpdateTotalPillsInputText(string text)
    {
        if (!text.IsValidInt())
        {
            TotalPillInput.DeleteLastCharacter();
            return;
        }
        TotalTestPillValue = float.Parse(text);
        UpdateTextBoxes();
    }

    private void UpdatePillBonusInputText(string text)
    {
        if (!text.IsValidFloat())
        {
            BonusPillInput.DeleteLastCharacter();
            return;
        }
        BonusTestPillValue = float.Parse(text);
        UpdateTextBoxes();
    }

    private void UpdateTextBoxes()
    {
        DailyValue.Text = DailyPillValue.ToString("N0");
        PillCount.Text = $"{RarePillsCount + EpicPillsCount + LegendaryPillsCount:N0} Pills"; 
        BonusPercentOutput.Text = $"{Multiplier:P2}";
        EmitSignal(SignalName.ValuesChanged);
    }
}

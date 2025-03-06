using System;
using System.Linq;
using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;

namespace OvermortalTools.Scenes.Planner;

public partial class PillPlanner : VBoxContainer
{
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
    
    private PillPlannerData _data = new();
    public PillPlannerData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
            TotalPillInput.Text = Data.TotalPillValue.ToString("N0");
            BonusPillInput.Text = Data.BonusPillValue.ToString("N0");
            Update();
        }
    }
    

    public override void _Ready()
    {
        Data.Changed += Update;
        UpdateTextBoxes();
        CultivationStage.MajorRealms.ForEach(realm => RealmSelect.AddItem(realm));
    }

    #region Events
    private void OnPillRealmOptionSelected(int index) => Data.PillRealmIndex = index;
    private void OnRarePillsSpinBoxChanged(double value) => Data.RarePills = (int)value;
    private void OnEpicPillsSpinBoxChanged(double value) => Data.EpicPills = (int)value;
    private void OnLegendaryPillsSpinBoxChanged(double value) => Data.LegendaryPills = (int)value;
    private void OnTotalPillInputChanged(string text) => UpdateTotalPillsInputText(text);
    private void OnBonusPillInputChanged(string text) => UpdatePillBonusInputText(text);
    private void OnArtifactsUpdated(float value) => Data.MythicPills = value;

    #endregion

    #region Actions

    private void UpdateTotalPillsInputText(string text)
    {
        if (!text.IsValidInt())
        {
            TotalPillInput.DeleteLastCharacter();
            return;
        }
        Data.TotalPillValue = int.Parse(text);
    }

    private void UpdatePillBonusInputText(string text)
    {
        if (!text.IsValidFloat())
        {
            BonusPillInput.DeleteLastCharacter();
            return;
        }
        Data.BonusPillValue = int.Parse(text);
    }

    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating PillPlanner.");

        UpdateTextBoxes();
        ValidateInputs();
        EmitSignal(SignalName.ValuesChanged);
    }

    private void ValidateInputs()
    {
        RealmSelect.Select(Data.PillRealmIndex);
        RarePills.Value = Data.RarePills;
        EpicPills.Value = Data.EpicPills;
        LegendaryPills.Value = Data.LegendaryPills;
    }

    private void UpdateTextBoxes()
    {
        DailyValue.Text = Data.DailyPillValue.ToString("N0");
        PillCount.Text = $"{Data.RarePills + Data.EpicPills + Data.LegendaryPills:N0} Pills"; 
        BonusPercentOutput.Text = $"{Data.PillBonusMultiplier:P2}";
        MythicPills.Text = Data.MythicPills.ToString("N2");
    }
}

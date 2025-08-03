using System;
using System.Linq;
using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Resources.Cultivation;
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
    [Export] private SpinBox TotalPillInput { get; set; }
    [Export] private SpinBox BonusPillInput { get; set; }
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
            TotalPillInput.Value = Data.TotalPillValue;
            BonusPillInput.Value = Data.BonusPillValue;
            Update();
        }
    }
    

    public override void _Ready()
    {
        Data.Changed += Update;
        UpdateTextBoxes();
        Enum.GetNames<Realm.MajorRealm>().ToList().ForEach(realm => RealmSelect.AddItem(realm));
    }

    #region Events
    private void OnPillRealmOptionSelected(int index) => Data.PillRealmIndex = index;
    private void OnRarePillsSpinBoxChanged(double value) => Data.RarePills = (int)value;
    private void OnEpicPillsSpinBoxChanged(double value) => Data.EpicPills = (int)value;
    private void OnLegendaryPillsSpinBoxChanged(double value) => Data.LegendaryPills = (int)value;
    private void OnTotalPillValueChanged(double value) => Data.TotalPillValue = (int)value;
    private void OnBonusPillValueChanged(double value) => Data.BonusPillValue = (int)value;
    private void OnArtifactsUpdated(float value) => Data.MythicPills = value;

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

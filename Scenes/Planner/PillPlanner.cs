using System;
using System.Linq;
using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scenes.Components;
using OvermortalTools.Scripts;
using static OvermortalTools.Scripts.Enums.Quality;

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
    [Export] private LabeledSpinbox TechniquesPercentInput { get; set; }
    [Export] private LabeledSpinbox EpicCurioPercentInput { get; set; }
    [Export] private LabeledSpinbox ImmortalFriendsPercentInput { get; set; }
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
            TechniquesPercentInput.Value = Data.TechniquesPercent;
            EpicCurioPercentInput.Value = Data.EpicCurioPercent;
            ImmortalFriendsPercentInput.Value = Data.ImmortalFriendsPercent;
            Update();
        }
    }

    private StarMarksData _starMarks = new();
    public StarMarksData StarMarks
    {
        get => _starMarks;
        set
        {
            _starMarks = value;
        }
    }
    

    public override void _Ready()
    {
        Data.Changed += Update;
        UpdateTextBoxes();
        Enum.GetNames<Realm.MajorRealm>().ToList().ForEach(realm => RealmSelect.AddItem(realm));
    }

    public long DailyPillXp => GetDailyPillXp();
    private long GetDailyPillXp()
    {
        var rare = Data.GetPillValue(Classification.Rare, StarMarks.RarePills / 100f) * Data.RarePills;
        var epic = Data.GetPillValue(Classification.Epic, StarMarks.EpicPills / 100f) * Data.EpicPills;
        var legendary = Data.GetPillValue(Classification.Legendary, StarMarks.LegendaryPills / 100f) * Data.LegendaryPills;
        return rare + epic + legendary + Data.MythicPillsValue;
    }

    #region Events
    private void OnPillRealmOptionSelected(int index) => Data.PillRealmIndex = index;
    private void OnRarePillsSpinBoxChanged(double value) => Data.RarePills = (int)value;
    private void OnEpicPillsSpinBoxChanged(double value) => Data.EpicPills = (int)value;
    private void OnLegendaryPillsSpinBoxChanged(double value) => Data.LegendaryPills = (int)value;
    private void OnTechniquePercentValueChanged(double value) => Data.TechniquesPercent = (float)value;
    private void OnEpicCurioPercentValueChanged(double value) => Data.EpicCurioPercent = (float)value;
    private void OnImmortalFriendsPercentValueChanged(double value) => Data.ImmortalFriendsPercent = (float)value;
    private void OnArtifactsUpdated(float value, float bonus)
    {
        Data.MythicPills = value;
        Data.MythicBonus = bonus;
    }

    #endregion


    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating PillPlanner.");

        UpdateTextBoxes();
        ValidateInputs();
        Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);
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

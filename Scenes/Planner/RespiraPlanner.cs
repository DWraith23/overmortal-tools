using Godot;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Scripts;
using OvermortalTools.Scripts.Enums;
using System;

namespace OvermortalTools.Resources.Planner;

public partial class RespiraPlanner : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    #region Exports
    [ExportGroup("Nodes")]
    [Export] private VBoxContainer ImmortalFriendsData { get; set; }
    [ExportSubgroup("Inputs")]
    [Export] private OptionButton RealmSelector { get; set; }
    [Export] private SpinBox RespiraAttemptsFromTechniques { get; set; }
    [Export] private SpinBox RespiraBonusFromTechniques { get; set; }
    [Export] private SpinBox RespiraAttemptsFromFriends { get; set; }
    [Export] private SpinBox RespiraBonusFromFriends { get; set; }
    [Export] private SpinBox RespiraAttemptsFromCurios { get; set; }
    [Export] private SpinBox RespiraBonusFromCurios { get; set; }
    [ExportSubgroup("Outputs")]
    [Export] private LineEdit TotalRespiraAttempts { get; set; }
    [Export] private LineEdit TotalRespiraBonus { get; set; }
    [Export] private LineEdit TotalRespiraValue { get; set; }
    [Export] private LineEdit DailyRespiraValue { get; set; }

    #endregion

    private RespiraPlannerData _data = new();
    public RespiraPlannerData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
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

    public long DailyRespiraExp => Data.GetDailyRespiraValue(StarMarks.RespiraExp);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Realm.NamesList.ForEach(realm => RealmSelector.AddItem(realm));
        Data.Changed += Update;
    }

    #region Events
    private void OnRealmSelectorOptionSelected(int index) => Data.RealmIndex = index;
    private void OnRespiraAttemptsFromTechniquesValueChanged(double value) => Data.RespiraAttemptsFromTechniques = (int)value;
    private void OnRespiraBonusFromTechniquesValueChanged(double value) => Data.RespiraBonusFromTechniques = (float)value;
    private void OnRespiraAttemptsFromFriendsValueChanged(double value) => Data.RespiraAttemptsFromFriends = (int)value;
    private void OnRespiraBonusFromFriendsValueChanged(double value) => Data.RespiraBonusFromFriends = (float)value;
    private void OnRespiraAttemptsFromCuriosValueChanged(double value) => Data.RespiraAttemptsFromCurios = (int)value;
    private void OnRespiraBonusFromCuriosValueChanged(double value) => Data.RespiraBonusFromCurios = (float)value;

    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating RespiraPlanner.");

        CheckImmortalFriendsVisibility();
        UpdateOutputs();
        ValidateInputs();
        Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);  
    }

    private void CheckImmortalFriendsVisibility()
    {
        ImmortalFriendsData.Visible = Data.RealmIndex >= 6;
        if (Data.RealmIndex < 6)
        {
            if (Data.RespiraAttemptsFromFriends != 0) Data.RespiraAttemptsFromFriends = 0;
            if (Data.RespiraBonusFromFriends != 0.0f) Data.RespiraBonusFromFriends = 0.0f;
        }
    }

    private void UpdateOutputs()
    {
        TotalRespiraAttempts.Text = Data.TotalRespiraAttempts.ToString("N0");
        TotalRespiraBonus.Text = Data.TotalRespiraBonus.ToString("P0");
        TotalRespiraValue.Text = Data.RespiraValue.ToString("N0");
        DailyRespiraValue.Text = Data.DailyRespiraValue.ToString("N0");
    }

    private void ValidateInputs()
    {
        RealmSelector.Select(Data.RealmIndex);
        RespiraAttemptsFromTechniques.Value = Data.RespiraAttemptsFromTechniques;
        RespiraBonusFromTechniques.Value = Data.RespiraBonusFromTechniques;
        RespiraAttemptsFromFriends.Value = Data.RespiraAttemptsFromFriends;
        RespiraBonusFromFriends.Value = Data.RespiraBonusFromFriends;
        RespiraAttemptsFromCurios.Value = Data.RespiraAttemptsFromCurios;
        RespiraBonusFromCurios.Value = Data.RespiraBonusFromCurios;
    }


}

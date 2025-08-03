using System;
using Godot;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;

namespace OvermortalTools.Scenes.Planner;

public partial class MyrimonPlanner : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    #region Exports
    [ExportGroup("Nodes")]
    [ExportSubgroup("Inputs")]
    [Export] private SpinBox ExpSpinBox { get; set; }
    [Export] private SpinBox QualitySpinBox { get; set; }
    [Export] private SpinBox GushSpinBox { get; set; }
    [Export] private SpinBox HighRankSpinBox { get; set; }
    [Export] private OptionButton FruitTypeSelect { get; set; }
    [Export] private OptionButton ExtractorQualitySelect { get; set; }
    [Export] private SpinBox FruitQuantitySpinBox { get; set; }
    [Export] private CheckBox RealmMatchCheckBox { get; set; }
    [ExportSubgroup("Outputs")]
    [Export] private LineEdit AverageXpValue { get; set; }
    [Export] private LineEdit AverageTechValue { get; set; }

    #endregion

    private MyrimonPlannerData _data = new();
    public MyrimonPlannerData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
            Update();
        }
    }

    public override void _Ready()
    {
        Data.Changed += Update;
    }

    #region Events
    private void OnExpSpinBoxChanged(double value) => Data.ExpLevel = (int)value;
    private void OnQualitySpinBoxChanged(double value) => Data.QualityLevel = (int)value;
    private void OnGushSpinBoxChanged(double value) => Data.GushLevel = (int)value;
    private void OnHighRankSpinBoxChanged(double value) => Data.HighRankLevel = (int)value;
    private void OnFruitQuantitySpinBoxChanged(double value) => Data.FruitQuantity = (int)value;
    private void OnFruitTypeOptionSelected(int index) => Data.FruitTypeIndex = index;
    private void OnExtractorQualityOptionSelected(int index) => Data.ExtractorQualityIndex = index;
    private void OnRealmMatchCheckBoxToggled(bool toggledOn) => Data.IsRealmMatch = toggledOn;
    
    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating MyrimonPlanner.");

        ValidateInputs();

        AverageTechValue.Text = Data.AverageTechPts.ToString("N0");
        AverageXpValue.Text = Data.AverageXp.ToString("N0");

        Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);
    }

    private void ValidateInputs()
    {
        ExpSpinBox.Value = Data.ExpLevel;
        QualitySpinBox.Value = Data.QualityLevel;
        GushSpinBox.Value = Data.GushLevel;
        HighRankSpinBox.Value = Data.HighRankLevel;
        FruitQuantitySpinBox.Value = Data.FruitQuantity;
        FruitTypeSelect.Select(Data.FruitTypeIndex);
        ExtractorQualitySelect.Select(Data.ExtractorQualityIndex);
        RealmMatchCheckBox.ButtonPressed = Data.IsRealmMatch;
    }


}

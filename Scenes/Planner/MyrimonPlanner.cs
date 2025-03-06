using Godot;
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
    [Export] private SpinBox FruitQuanitySpinBox { get; set; }
    [Export] private CheckBox RealmMatchCheckBox { get; set; }
    [ExportSubgroup("Outputs")]
    [Export] private LineEdit AverageXpValue { get; set; }
    [Export] private LineEdit AverageTechValue { get; set; }

    #endregion

    private MyrimonCalculation _calculator = new();
    public MyrimonCalculation Calculator
    {
        get => _calculator;
        set
        {
            _calculator = value;
            Update();
        }
    }

    private int ExpValue => (int)ExpSpinBox.Value;
    private int QualityValue => (int)QualitySpinBox.Value;
    private int GushValue => (int)GushSpinBox.Value;
    private int HighRankValue => (int)HighRankSpinBox.Value;
    private int FruitQuanity => (int)FruitQuanitySpinBox.Value;


    public override void _Ready()
    {
        
    }

    #region Events

    private void OnSpinboxChanged(double value) => Update();
    private void OnOptionSelected(int index) => Update();
    private void OnRealmMatchToggled(bool on) => Update();
    #endregion

    #region Actions

    private void Update()
    {
        Calculator.FruitType = FruitTypeSelect.Text;
        Calculator.ExtractorQuality = (MyrimonCalculation.Quality) ExtractorQualitySelect.Selected;
        Calculator.XPLevel = ExpValue;
        Calculator.QualityLevel = QualityValue;
        Calculator.GushLevel = GushValue;
        Calculator.TechLevel = HighRankValue;
        Calculator.FruitQuantity = FruitQuanity;
        Calculator.MatchesServerLevel = RealmMatchCheckBox.ButtonPressed;

        var techPts = Calculator.FruitQuantity * Calculator.TechChance * Calculator.TechPoints;
        var averageXp = Calculator.GetAverageXP();

        AverageTechValue.Text = techPts.ToString("N0");
        AverageXpValue.Text = averageXp.ToString("N0");


        EmitSignal(SignalName.ValuesChanged);
    }

    #endregion
}

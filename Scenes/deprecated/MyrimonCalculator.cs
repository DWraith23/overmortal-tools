using Godot;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes;

[GlobalClass]
public partial class MyrimonCalculator : Control
{
//     private MyrimonCalculation _calculator = new();
//     public MyrimonCalculation Calculator
//     {
//         get => _calculator;
//         set
//         {
//             _calculator = value;
//             NumberOfFruits = Calculator.FruitQuantity;
//             Update();
//         }
//     }

    [ExportGroup("Nodes")]
    [ExportSubgroup("Input")]
    [Export] private SpinBox EXPSpinBox { get; set; }
    [Export] private SpinBox QualitySpinBox { get; set; }
    [Export] private SpinBox GushSpinBox { get; set; }
    [Export] private SpinBox TechSpinBox { get; set; }
    [Export] private OptionButton FruitTypeButton { get; set; }
    [Export] private OptionButton QualityButton { get; set; }
    [Export] private SpinBox FruitQuantity { get; set; }
    [ExportSubgroup("Output")]
    [Export] private LineEdit BaseFruitValue { get; set; }
    [Export] private LineEdit CommonValue { get; set; }
    [Export] private LineEdit CommonGush { get; set; }
    [Export] private LineEdit CommonOdds { get; set; }
    [Export] private LineEdit UncommonValue { get; set; }
    [Export] private LineEdit UncommonGush { get; set; }
    [Export] private LineEdit UncommonOdds { get; set; }
    [Export] private LineEdit RareValue { get; set; }
    [Export] private LineEdit RareGush { get; set; }
    [Export] private LineEdit RareOdds { get; set; }
    [Export] private LineEdit EpicValue { get; set; }
    [Export] private LineEdit EpicGush { get; set; }
    [Export] private LineEdit EpicOdds { get; set; }
    [Export] private LineEdit LegendaryValue { get; set; }
    [Export] private LineEdit LegendaryGush { get; set; }
    [Export] private LineEdit LegendaryOdds { get; set; }
    [Export] private LineEdit MythicValue { get; set; }
    [Export] private LineEdit MythicGush { get; set; }
    [Export] private LineEdit MythicOdds { get; set; }
    [Export] private LineEdit MinimumXP { get; set; }
    [Export] private LineEdit AverageXP { get; set; }
    [Export] private LineEdit MaximumXP { get; set; }
    [Export] private LineEdit AverageTech { get; set; }

    private int NumberOfFruits { get; set; } = 0;

    public override void _Ready()
    {
        QualitySelected(0);
    }

    private void SpinboxUpdated(double value)
    {
        // Calculator.XPLevel = (int) EXPSpinBox.Value;
        // Calculator.QualityLevel = (int) QualitySpinBox.Value;
        // Calculator.GushLevel = (int) GushSpinBox.Value;
        // Calculator.TechLevel = (int) TechSpinBox.Value;
        Update();
    }

    private void TypeSelected (int index)
    {
        // Calculator.FruitType = FruitTypeButton.Text;
        Update();
    }

    private void QualitySelected(int index)
    {
        // Calculator.ExtractorQuality = (MyrimonCalculation.Quality) index;

        EXPSpinBox.MaxValue = (index + 1) * 5;

        if (index > 0) QualitySpinBox.MaxValue = (index + 1) * 5;
        else QualitySpinBox.MaxValue = 0;

        if (index > 2) GushSpinBox.MaxValue = (index + 1) * 5;
        else GushSpinBox.MaxValue = 0;

        if (index > 4) TechSpinBox.MaxValue = (index + 1) * 5;
        else TechSpinBox.MaxValue = 0;

        if (index == 2)
        {
            EXPSpinBox.MinValue = 6;
            QualitySpinBox.MinValue = 6;
        }
        else if (index == 3)
        {
            EXPSpinBox.MinValue = 11;
            QualitySpinBox.MinValue = 11;
        }
        else if (index == 4)
        {
            EXPSpinBox.MinValue = 16;
            QualitySpinBox.MinValue = 16;
        }
        else if (index == 5)
        {
            EXPSpinBox.MinValue = 21;
            QualitySpinBox.MinValue = 21;
        }

        Update();
    }

    private void QuantityUpdated(double value)
    {
        NumberOfFruits = (int) FruitQuantity.Value;
        // Calculator.FruitQuantity = NumberOfFruits;
        Update();
    }

    private void Update()
    {
        // CheckNodes();

        // BaseFruitValue.Text = MyrimonCalculation.BaseFruitValues[FruitTypeButton.Text].ToString("N0");

        // CommonValue.Text = Calculator.GetFruitValue(MyrimonCalculation.Quality.Common).ToString("N0");
        // CommonGush.Text = (Calculator.GetFruitValue(MyrimonCalculation.Quality.Common) * Calculator.GushMultiplier).ToString("N0");
        // CommonOdds.Text = Calculator.QualityChances[MyrimonCalculation.Quality.Common].ToString("N2") + "%";

        // UncommonValue.Text = Calculator.GetFruitValue(MyrimonCalculation.Quality.Uncommon).ToString("N0");
        // UncommonGush.Text = (Calculator.GetFruitValue(MyrimonCalculation.Quality.Uncommon) * Calculator.GushMultiplier).ToString("N0");
        // UncommonOdds.Text = Calculator.QualityChances[MyrimonCalculation.Quality.Uncommon].ToString("N2") + "%";

        // RareValue.Text = Calculator.GetFruitValue(MyrimonCalculation.Quality.Rare).ToString("N0");
        // RareGush.Text = (Calculator.GetFruitValue(MyrimonCalculation.Quality.Rare) * Calculator.GushMultiplier).ToString("N0");
        // RareOdds.Text = Calculator.QualityChances[MyrimonCalculation.Quality.Rare].ToString("N2") + "%";

        // EpicValue.Text = Calculator.GetFruitValue(MyrimonCalculation.Quality.Epic).ToString("N0");
        // EpicGush.Text = (Calculator.GetFruitValue(MyrimonCalculation.Quality.Epic) * Calculator.GushMultiplier).ToString("N0");
        // EpicOdds.Text = Calculator.QualityChances[MyrimonCalculation.Quality.Epic].ToString("N2") + "%";

        // LegendaryValue.Text = Calculator.GetFruitValue(MyrimonCalculation.Quality.Legendary).ToString("N0");
        // LegendaryGush.Text = (Calculator.GetFruitValue(MyrimonCalculation.Quality.Legendary) * Calculator.GushMultiplier).ToString("N0");
        // LegendaryOdds.Text = Calculator.QualityChances[MyrimonCalculation.Quality.Legendary].ToString("N2") + "%";

        // MythicValue.Text = Calculator.GetFruitValue(MyrimonCalculation.Quality.Mythic).ToString("N0");
        // MythicGush.Text = (Calculator.GetFruitValue(MyrimonCalculation.Quality.Mythic) * Calculator.GushMultiplier).ToString("N0");
        // MythicOdds.Text = Calculator.QualityChances[MyrimonCalculation.Quality.Mythic].ToString("N2") + "%";


        // MinimumXP.Text = Calculator.GetMinimumXP().ToString("N0");
        // AverageXP.Text = Calculator.GetAverageXP().ToString("N0");
        // MaximumXP.Text = Calculator.GetMaximumXP().ToString("N0");

        // var techPts = NumberOfFruits * Calculator.TechChance * Calculator.TechPoints;
        // AverageTech.Text = techPts.ToString("N0");
    }

    private void CheckNodes()
    {
        
        // int extractorQualityID = Calculator.ExtractorQuality switch
        // {
        //     MyrimonCalculation.Quality.Common => 0,
        //     MyrimonCalculation.Quality.Uncommon => 1,
        //     MyrimonCalculation.Quality.Rare => 2,
        //     MyrimonCalculation.Quality.Epic => 3,
        //     MyrimonCalculation.Quality.Legendary => 4,
        //     MyrimonCalculation.Quality.Mythic => 5,
        //     _ => 0,
        // };
        // QualityButton.Select(extractorQualityID);
        // QualitySelected(extractorQualityID);
        // if (EXPSpinBox.Value != Calculator.XPLevel) EXPSpinBox.Value = Calculator.XPLevel;
        // if (QualitySpinBox.Value != Calculator.QualityLevel) QualitySpinBox.Value = Calculator.QualityLevel;
        // if (GushSpinBox.Value != Calculator.GushLevel) GushSpinBox.Value = Calculator.GushLevel;
        // if (TechSpinBox.Value != Calculator.TechLevel) TechSpinBox.Value = Calculator.TechLevel;
        // if (FruitQuantity.Value != Calculator.FruitQuantity) FruitQuantity.Value = Calculator.FruitQuantity;
        // int fruitTypeID = Calculator.FruitType switch
        // {
        //     "Mortal" => 0,
        //     "Voidbreak" => 1,
        //     _ => 0,
        // };
        // FruitTypeButton.Select(fruitTypeID);
    }
}

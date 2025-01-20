using Godot;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes;

public partial class PillPanel : PanelContainer
{
    // [ExportGroup("Nodes")]
    // [Export] private Button PopupDisplayButton { get; set; }
    // [Export] private PillBonusCalculatorPopup Popup { get; set; }
    // [Export] private LineEdit PillBonusInput { get; set; }
    // [Export] private OptionButton SetRankOptions { get; set; }
    // [ExportSubgroup("Widgets")]
    // [Export] private PillWidget WidgetOne { get; set; }
    // [Export] private PillWidget WidgetTwo { get; set; }
    // [Export] private PillWidget WidgetThree { get; set; }
    // [Export] private PillWidget WidgetFour { get; set; }
    // [Export] private PillWidget WidgetFive { get; set; }
    // [ExportSubgroup("Needed")]
    // [Export] private Label NeededOneLabel { get; set; }
    // [Export] private Label NeededTwoLabel { get; set; }
    // [Export] private Label NeededThreeLabel { get; set; }
    // [Export] private Label NeededFourLabel { get; set; }
    // [Export] private Label NeededFiveLabel { get; set; }


    // private double _pillBonusValue = 1d;
    // public double PillBonusValue
    // {
    //     get => _pillBonusValue;
    //     set
    //     {
    //         _pillBonusValue = value;
    //         PillBonusInput.Text = $"{value:P2}";
    //         SetWidgetMultipliers();
    //     }
    // }

    // private int XPNeeded { get; set; } = 0;

    // // Called when the node enters the scene tree for the first time.
    // public override void _Ready()
    // {
    //     if (Engine.IsEditorHint()) return;
    //     PopupDisplayButton.Pressed += OnPopupDisplayButtonPressed;
    //     Popup.ApplyPercentage += OnPercentageApplied;
    //     WidgetOne.Changed += OnWidgetChanged;
    //     WidgetTwo.Changed += OnWidgetChanged;
    //     WidgetThree.Changed += OnWidgetChanged;
    //     WidgetFour.Changed += OnWidgetChanged;
    //     WidgetFive.Changed += OnWidgetChanged;


    //     for (int i = 0; i < 5; i++)
    //     {
    //         SetRankOptions.AddItem($"R{i + 1}");
    //     }

    //     SetRankOptions.ItemSelected += OnRankOptionsSelected;
    // }

    // private void OnPopupDisplayButtonPressed() => Popup.Popup();

    // private void OnPercentageApplied(double value) => PillBonusValue = value;

    // private void OnRankOptionsSelected(long value)
    // {
    //     var index = (int)value;
    //     if (index <= 0) return;
    //     GD.Print(index);
    //     WidgetOne.SetRank(index);
    //     WidgetTwo.SetRank(index);
    //     WidgetThree.SetRank(index);
    //     WidgetFour.SetRank(index);
    //     WidgetFive.SetRank(index);

    //     WidgetOne.SetQuality(Scripts.Enums.Quality.Classification.Common);
    //     WidgetTwo.SetQuality(Scripts.Enums.Quality.Classification.Uncommon);
    //     WidgetThree.SetQuality(Scripts.Enums.Quality.Classification.Rare);
    //     WidgetFour.SetQuality(Scripts.Enums.Quality.Classification.Epic);
    //     WidgetFive.SetQuality(Scripts.Enums.Quality.Classification.Legendary);
    // }

    // private void SetWidgetMultipliers()
    // {
    //     WidgetOne.Multiplier = (float)PillBonusValue;
    //     WidgetTwo.Multiplier = (float)PillBonusValue;
    //     WidgetThree.Multiplier = (float)PillBonusValue;
    //     WidgetFour.Multiplier = (float)PillBonusValue;
    //     WidgetFive.Multiplier = (float)PillBonusValue;
    // }

    // public void CalculateAllNeeded(int xp)
    // {
    //     NeededOneLabel.Text = CalculateNeeded(WidgetOne, xp).ToString("N0");
    //     NeededTwoLabel.Text = CalculateNeeded(WidgetTwo, xp).ToString("N0");
    //     NeededThreeLabel.Text = CalculateNeeded(WidgetThree, xp).ToString("N0");
    //     NeededFourLabel.Text = CalculateNeeded(WidgetFour, xp).ToString("N0");
    //     NeededFiveLabel.Text = CalculateNeeded(WidgetFive, xp).ToString("N0");     

    //     XPNeeded = xp;   
    // }

    // private static int CalculateNeeded(PillWidget widget, int xp)
    // {
    //     if (widget.Value == 0) return 0;
    //     return ((float)xp / widget.Value).RoundUp();
    // }

    // private void OnWidgetChanged() => CalculateAllNeeded(XPNeeded);
}

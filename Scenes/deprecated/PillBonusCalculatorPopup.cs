using Godot;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes;

public partial class PillBonusCalculatorPopup : PopupPanel
{
    [Signal] public delegate void ApplyPercentageEventHandler(double percentage);

    [Export] private LineEdit ValueInput { get; set; }
    [Export] private LineEdit PlusInput { get; set; }
    [Export] private Label PercentageLabel { get; set; }
    [Export] private Button CloseButton { get; set; }
    [Export] private Button ApplyButton { get; set; }

    private int Value { get; set; } = 0;
    private int Additional { get; set; } = 0;
    private string PercentageText => $"{CalculatePercentage():P2}";

    public override void _Ready()
    {
        CloseButton.Pressed += OnClosePressed;
        ApplyButton.Pressed += OnApplyButtonPressed;
        ValueInput.TextChanged += OnValueInputChanged;
        PlusInput.TextChanged += OnPlusInputChanged;
    }

    private void Update()
    {
        PercentageLabel.Text = PercentageText;
    }

    private void OnClosePressed() => Hide();

    private void OnValueInputChanged(string text)
    {
        if (!text.IsValidInt())
        {
            ValueInput.DeleteLastCharacter();
            return;
        }
        Value = int.Parse(text);
        Update();
    }

    private void OnPlusInputChanged(string text)
    {
        if (!text.IsValidInt())
        {
            PlusInput.DeleteLastCharacter();
            return;
        }
        Additional = int.Parse(text);
        Update();
    }

    private double CalculatePercentage()
    {
        var value = Value - Additional;
        var percent = (double)Value / value;
        return percent;
    }

    private void OnApplyButtonPressed() => Tools.EmitLoggedSignal(this, SignalName.ApplyPercentage, CalculatePercentage());
}

using Godot;
using OvermortalTools.Scenes.Components;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class PassiveDataSelection : VBoxContainer
{

    #region Nodes
    private VBoxContainer CosmoapsisContainer => GetNode<VBoxContainer>("Cosmoapsis");
    private LabeledSpinbox AbodeAuraSpinBox => CosmoapsisContainer.GetNode<LabeledSpinbox>("Abode Aura");
    private LabeledSpinbox ViryaSpinBox => CosmoapsisContainer.GetNode<LabeledSpinbox>("Virya");
    private LabeledSpinbox StriveSpinBox => CosmoapsisContainer.GetNode<LabeledSpinbox>("Strive");

    private VBoxContainer AuraGemContainer => GetNode<VBoxContainer>("Aura Gem");
    private OptionButton AuraGemQualitySelect => AuraGemContainer.GetNode<OptionButton>("Quality");
    private LabeledSpinbox AuraseepBonusSpinBox => AuraGemContainer.GetNode<LabeledSpinbox>("Auraseep Bonus");


    #endregion

    private PassiveData _data = new();
    public PassiveData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            Update();
            if (value != null)
            {
                _data.Changed += Update;
            }
        }
    }

    public override void _Ready()
    {
        ConnectSignals();
        AddAuraGemOptions();
    }

    private void AddAuraGemOptions()
    {
        AuraGemQualitySelect.Clear();
        foreach (var quality in Enum.GetNames<Quality>())
        {
            AuraGemQualitySelect.AddItem(quality);
        }
    }

    private void Update()
    {
        if (Data == null) return;

        GD.Print("PassiveDataSelection Update() called");

        AbodeAuraSpinBox.SetValueNoSignal(Data.AbodeAura);
        ViryaSpinBox.SetValueNoSignal(Data.ViryaPercent * 100);
        StriveSpinBox.SetValueNoSignal(Data.StrivePercent * 100);

        AuraGemQualitySelect.Select(Data.AuraGemIndex);
        AuraseepBonusSpinBox.SetValueNoSignal(Data.AuraseepBonus);
    }

    private void ConnectSignals()
    {
        _data.Changed += Update;

        AbodeAuraSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.AbodeAura = (float)value;
        };

        ViryaSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.ViryaPercent = (float)value / 100f;
        };

        StriveSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.StrivePercent = (float)value / 100f;
        };

        AuraGemQualitySelect.ItemSelected += value =>
        {
            if (Data == null) return;
            Data.AuraGemIndex = (int)value;
        };

        AuraseepBonusSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.AuraseepBonus = (float)value;
        };
    }


}

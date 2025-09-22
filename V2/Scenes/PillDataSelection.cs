using Godot;
using OvermortalTools.Scenes.Components;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class PillDataSelection : VBoxContainer
{
    #region Nodes
    private GridContainer RaritiesGrid => GetNode<GridContainer>("Daily Pills/Rarities");
    private SpinBox RareQtySpinBox => RaritiesGrid.GetNode<SpinBox>("Rare/SpinBox");
    private SpinBox EpicQtySpinBox => RaritiesGrid.GetNode<SpinBox>("Epic/SpinBox");
    private SpinBox LegendaryQtySpinBox => RaritiesGrid.GetNode<SpinBox>("Legendary/SpinBox");
    private LineEdit MythicQtyInput => RaritiesGrid.GetNode<LineEdit>("Mythic/LineEdit");

    private VBoxContainer BonusesContainer => GetNode<VBoxContainer>("Daily Pills/Bonus Section/Bonuses");
    private LabeledSpinbox TechniquesSpinBox => BonusesContainer.GetNode<LabeledSpinbox>("Techniques");
    private LabeledSpinbox EpicCurioSpinBox => BonusesContainer.GetNode<LabeledSpinbox>("Epic Curio");
    private LabeledSpinbox ImmortalFriendsSpinBox => BonusesContainer.GetNode<LabeledSpinbox>("Immortal Friends");

    private LineEdit TotalBonusOutput => GetNode<LineEdit>("Daily Pills/Bonus Section/Total Bonus/LineEdit");

    #endregion

    private PillData _data = new();
    public PillData Data
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
    }

    private void Update()
    {
        if (Data == null) return;

        RareQtySpinBox.SetValueNoSignal(Data.DailyRare);
        EpicQtySpinBox.SetValueNoSignal(Data.DailyEpic);
        LegendaryQtySpinBox.SetValueNoSignal(Data.DailyLegendary);

        MythicQtyInput.Text = Data.DailyMythic.ToString("N2");

        TechniquesSpinBox.SetValueNoSignal(Data.TechniquesBonus);
        EpicCurioSpinBox.SetValueNoSignal(Data.EpicCurioBonus);
        ImmortalFriendsSpinBox.SetValueNoSignal(Data.ImmortalFriendsBonus);

        TotalBonusOutput.Text = Data.TotalBonus.ToString("N2");
    }

    private void ConnectSignals()
    {
        RareQtySpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.DailyRare = (int)value;
        };

        EpicQtySpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.DailyEpic = (int)value;
        };

        LegendaryQtySpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.DailyLegendary = (int)value;
        };

        TechniquesSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.TechniquesBonus = (float)value;
        };

        EpicCurioSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.EpicCurioBonus = (float)value;
        };

        ImmortalFriendsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.ImmortalFriendsBonus = (float)value;
        };
    }

}

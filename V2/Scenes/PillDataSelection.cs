using Godot;
using OvermortalTools.V2.Resources;

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

    private ProfileData _profile = new();
    public ProfileData Profile
    {
        get => _profile;
        set
        {
            if (_profile == value) return;
            _profile = value;
            Update();
            if (value != null)
            {
                _profile.Changed += Update;
            }
        }
    }

    private PillData Data => Profile.PillData;

    public override void _Ready()
    {
        ConnectSignals();
    }

    private void Update()
    {
        if (Profile == null) return;

        GD.Print("PillDataSelection Update() called");

        RareQtySpinBox.SetValueNoSignal(Data.DailyRare);
        EpicQtySpinBox.SetValueNoSignal(Data.DailyEpic);
        LegendaryQtySpinBox.SetValueNoSignal(Data.DailyLegendary);

        Data.DailyMythic = Profile.CreationArtifacts.DailyMythicPills;

        MythicQtyInput.Text = Data.DailyMythic.ToString("N2");

        TechniquesSpinBox.SetValueNoSignal(Data.TechniquesBonus);
        EpicCurioSpinBox.SetValueNoSignal(Data.EpicCurioBonus);
        ImmortalFriendsSpinBox.SetValueNoSignal(Data.ImmortalFriendsBonus);

        TotalBonusOutput.Text = Data.TotalBonus.ToString("P2");
    }

    private void ConnectSignals()
    {
        _profile.Changed += Update;

        RareQtySpinBox.ValueChanged += value =>
        {
            if (Profile == null) return;
            Data.DailyRare = (int)value;
        };

        EpicQtySpinBox.ValueChanged += value =>
        {
            if (Profile == null) return;
            Data.DailyEpic = (int)value;
        };

        LegendaryQtySpinBox.ValueChanged += value =>
        {
            if (Profile == null) return;
            Data.DailyLegendary = (int)value;
        };

        TechniquesSpinBox.ValueChanged += value =>
        {
            if (Profile == null) return;
            Data.TechniquesBonus = (float)value;
        };

        EpicCurioSpinBox.ValueChanged += value =>
        {
            if (Profile == null) return;
            Data.EpicCurioBonus = (float)value;
        };

        ImmortalFriendsSpinBox.ValueChanged += value =>
        {
            if (Profile == null) return;
            Data.ImmortalFriendsBonus = (float)value;
        };
    }

}

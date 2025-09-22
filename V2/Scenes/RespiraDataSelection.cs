using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class RespiraDataSelection : VBoxContainer
{

    #region Nodes

    private HBoxContainer BonusesContainer => GetNode<HBoxContainer>("Bonuses");

    private VBoxContainer TechniquesContainer => BonusesContainer.GetNode<VBoxContainer>("Techniques");
    private SpinBox TechniqueAttemptsSpinBox => TechniquesContainer.GetNode<SpinBox>("Attempts");
    private SpinBox TechniqueBonusSpinBox => TechniquesContainer.GetNode<SpinBox>("Bonus");

    private VBoxContainer CuriosContainer => BonusesContainer.GetNode<VBoxContainer>("Curios");
    private SpinBox CurioAttemptsSpinBox => CuriosContainer.GetNode<SpinBox>("Attempts");
    private SpinBox CurioBonusSpinBox => CuriosContainer.GetNode<SpinBox>("Bonus");

    private VBoxContainer FriendsContainer => BonusesContainer.GetNode<VBoxContainer>("Friends");
    private SpinBox FriendAttemptsSpinBox => FriendsContainer.GetNode<SpinBox>("Attempts");
    private SpinBox FriendBonusSpinBox => FriendsContainer.GetNode<SpinBox>("Bonus");


    private VBoxContainer OutputsContainer => GetNode<VBoxContainer>("Outputs");
    private LineEdit TotalAttemptsOutput => OutputsContainer.GetNode<LineEdit>("Attempts/LineEdit");
    private LineEdit AverageValueOutput => OutputsContainer.GetNode<LineEdit>("Avg Value/LineEdit");

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

    private RespiraData Data => Profile.RespiraData;

    public override void _Ready()
    {
        ConnectSignals();
    }

    private void Update()
    {
        if (Data == null) return;

        GD.Print("RespiraDataSelection Update() called");

        TechniqueAttemptsSpinBox.SetValueNoSignal(Data.RespiraAttemptsFromTechniques);
        TechniqueBonusSpinBox.SetValueNoSignal(Data.RespiraBonusFromTechniques);

        CurioAttemptsSpinBox.SetValueNoSignal(Data.RespiraAttemptsFromCurios);
        CurioBonusSpinBox.SetValueNoSignal(Data.RespiraBonusFromCurios);

        FriendAttemptsSpinBox.SetValueNoSignal(Data.RespiraAttemptsFromFriends);
        FriendBonusSpinBox.SetValueNoSignal(Data.RespiraBonusFromFriends);

        TotalAttemptsOutput.Text = Data.TotalRespiraAttempts.ToString("N0");
        AverageValueOutput.Text = Data.GetTotalAverageRespiraValue(Profile.GetHighestRealmInformation().Item1).ToString("N2");
    }

    private void ConnectSignals()
    {
        _profile.Changed += Update;

        TechniqueAttemptsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.RespiraAttemptsFromTechniques = (int)value;
        };

        TechniqueBonusSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.RespiraBonusFromTechniques = (float)value;
        };

        CurioAttemptsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.RespiraAttemptsFromCurios = (int)value;
        };

        CurioBonusSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.RespiraBonusFromCurios = (float)value;
        };

        FriendAttemptsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.RespiraAttemptsFromFriends = (int)value;
        };

        FriendBonusSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.RespiraBonusFromFriends = (float)value;
        };
    }

}

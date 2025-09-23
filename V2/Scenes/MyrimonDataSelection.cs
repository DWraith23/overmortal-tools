using Godot;
using OvermortalTools.Scenes.Components;
using OvermortalTools.Scripts;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class MyrimonDataSelection : VBoxContainer
{
    #region Nodes
    private GridContainer UpgradesContainer => GetNode<GridContainer>("Upgrades");
    private LabeledSpinbox ExpSpinBox => UpgradesContainer.GetNode<LabeledSpinbox>("Exp");
    private LabeledSpinbox QualitySpinBox => UpgradesContainer.GetNode<LabeledSpinbox>("Quality");
    private LabeledSpinbox GushSpinBox => UpgradesContainer.GetNode<LabeledSpinbox>("Gush");
    private LabeledSpinbox TechPtsSpinBox => UpgradesContainer.GetNode<LabeledSpinbox>("Tech Pts");

    private OptionButton RankSelect => GetNode<OptionButton>("Rank Select");

    private LabeledSpinbox FruitSpinBox => GetNode<LabeledSpinbox>("Fruit");

    private VBoxContainer WeeklyFruitOptions => GetNode<VBoxContainer>("Weekly Fruit Options");
    private CheckBox FruitPacksCheckBox => WeeklyFruitOptions.GetNode<CheckBox>("Fruit Packs");
    private CheckBox TokenPacksCheckBox => WeeklyFruitOptions.GetNode<CheckBox>("Token Packs");

    private CheckBox RealmMatchCheckBox => GetNode<CheckBox>("Realm Match");

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

    private MyrimonData Data => Profile.MyrmimonData;

    public override void _Ready()
    {
        PopulateOptionButton();
        ConnectSignals();
    }

    private void PopulateOptionButton()
    {
        foreach (var name in Enum.GetNames<Quality>())
        {
            RankSelect.AddItem(name);
        }
    }

    private void Update()
    {
        GD.Print("MyrimonDataSelection Update() called");

        ExpSpinBox.SetValueNoSignal(Data.ExpLevel);
        QualitySpinBox.SetValueNoSignal(Data.QualityLevel);
        GushSpinBox.SetValueNoSignal(Data.GushLevel);
        TechPtsSpinBox.SetValueNoSignal(Data.TechLevel);

        RankSelect.Select((int)Data.ExtractorRank + 1);
        FruitSpinBox.SetValueNoSignal(Data.CurrentFruitQuantity);

        FruitPacksCheckBox.ButtonPressed = Data.BuysFruitPacks;
        TokenPacksCheckBox.ButtonPressed = Data.BuysTokenPacks;
        RealmMatchCheckBox.ButtonPressed = Data.MatchesServerRealm;
    }

    private void ConnectSignals()
    {
        _profile.Changed += Update;

        ExpSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.ExpLevel = (int)value;
        };

        QualitySpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.QualityLevel = (int)value;
        };

        GushSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.GushLevel = (int)value;
        };

        TechPtsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.TechLevel = (int)value;
        };

        RankSelect.ItemSelected += idx =>
        {
            if (Data == null) return;
            if (idx == 0)
            {
                Data.ExtractorRank = Quality.Common;
                return;
            }
            Data.ExtractorRank = (Quality)idx - 1;
        };

        FruitSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.CurrentFruitQuantity = (int)value;
        };

        FruitPacksCheckBox.Toggled += toggled =>
        {
            if (Data == null) return;
            Data.BuysFruitPacks = toggled;
        };

        TokenPacksCheckBox.Toggled += toggled =>
        {
            if (Data == null) return;
            Data.BuysTokenPacks = toggled;
        };

        RealmMatchCheckBox.Toggled += toggled =>
        {
            if (Data == null) return;
            Data.MatchesServerRealm = toggled;
        };
    }


}

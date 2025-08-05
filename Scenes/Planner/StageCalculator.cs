using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class StageCalculator : VBoxContainer
{

    [Signal] public delegate void ValuesChangedEventHandler();

    // Current
    private GridContainer CurrentContents => GetNode<GridContainer>("Current Level");
    private OptionButton CurrentMajorRealmSelect => CurrentContents.GetNode<OptionButton>("Major Realm Select");
    private OptionButton CurrentMinorRealmSelect => CurrentContents.GetNode<OptionButton>("Minor Realm Select");
    private SpinBox CurrentPercentInput => CurrentContents.GetNode<SpinBox>("SpinBox");

    // Target
    private GridContainer TargetContents => GetNode<GridContainer>("Target Level");
    private OptionButton TargetMajorRealmSelect => TargetContents.GetNode<OptionButton>("Major Realm Select");
    private OptionButton TargetMinorRealmSelect => TargetContents.GetNode<OptionButton>("Minor Realm Select");

    // Output
    private LineEdit XpRemainingOutput => GetNode<LineEdit>("Details/LineEdit");

    private StageCalculatorData _data = new();
    public StageCalculatorData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            FullUpdate();
        }
    }

    public override void _Ready()
    {
        FullUpdate();
        ConnectSignals();
    }

    private void ConnectSignals()
    {
        CurrentMajorRealmSelect.ItemSelected += OnCurrentMajorRealmItemSelected;
        CurrentMinorRealmSelect.ItemSelected += OnCurrentMinorRealmItemSelected;
        CurrentPercentInput.ValueChanged += OnCurrentPercentValueChanged;
        TargetMajorRealmSelect.ItemSelected += OnTargetMajorRealmItemSelected;
        TargetMinorRealmSelect.ItemSelected += OnTargetMinorRealmItemSelected;
    }

    #region Update

    private void FullUpdate()
    {
        if (Data == null) return;
        SetOptionButtons();
        SetRemainingXp();
    }

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating StageCalculator.");
        GD.Print($"{Data.CurrentRealm} -> {Data.TargetRealm}");
        SetTargetRealmOptionButtons();
        SetRemainingXp();
        Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);
    }

    private void SetRemainingXp() =>
        XpRemainingOutput.Text = RealmList.GetXpToTarget(Data.CurrentRealm, Data.TargetRealm).ToString("N0");

    #region Option Buttons

    private void SetOptionButtons()
    {
        SetCurrentRealmOptionButtons();
        SetTargetRealmOptionButtons();
    }

    private void SetCurrentRealmOptionButtons()
    {
        CurrentMajorRealmSelect.Clear();
        CurrentMinorRealmSelect.Clear();

        foreach (var realm in Realm.NamesList)
        {
            CurrentMajorRealmSelect.AddItem(realm);
        }

        foreach (var stage in Enum.GetNames<Realm.MinorRealm>())
        {
            CurrentMinorRealmSelect.AddItem(stage);
        }

        CurrentMajorRealmSelect.Select(RealmList.RealmIndex(Data.CurrentMajorRealm));
        CurrentMinorRealmSelect.Select((int)Data.CurrentMinorRealm);
        CurrentPercentInput.Value = Data.CurrentPercent * 100;
    }

    private void SetTargetRealmOptionButtons()
    {
        TargetMajorRealmSelect.Clear();
        TargetMinorRealmSelect.Clear();

        int idx = 0;
        foreach (var realm in Realm.NamesList)
        {
            TargetMajorRealmSelect.AddItem(realm);
            TargetMajorRealmSelect.SetItemDisabled(idx, idx < RealmList.RealmIndex(Data.CurrentMajorRealm));
            idx++;
        }

        idx = 0;
        foreach (var stage in Enum.GetNames<Realm.MinorRealm>())
        {
            TargetMinorRealmSelect.AddItem(stage);
            if (Data.CurrentMajorRealm == Data.TargetMajorRealm)
                TargetMinorRealmSelect.SetItemDisabled(idx, idx < (int)Data.CurrentMinorRealm);
            idx++;
        }

        var targetIdx = RealmList.RealmIndex(Data.TargetMajorRealm);
        if (TargetMajorRealmSelect.IsItemDisabled(targetIdx))
        {
            targetIdx = RealmList.RealmIndex(Data.CurrentMajorRealm);
            Data.TargetMajorRealm = (Realm.MajorRealm)targetIdx;
        }
        TargetMajorRealmSelect.Select(targetIdx);

        targetIdx = (int)Data.TargetMinorRealm;
        if (TargetMinorRealmSelect.IsItemDisabled(targetIdx))
        {
            targetIdx = (int)Data.CurrentMinorRealm;
            Data.TargetMinorRealm = (Realm.MinorRealm)targetIdx;
        }
        TargetMinorRealmSelect.Select(targetIdx);
    }

    #endregion

    #endregion


    #region Events

    private void OnCurrentMajorRealmItemSelected(long idx)
    {
        Data.CurrentMajorRealm = (Realm.MajorRealm)idx;
        Update();
    }

    private void OnCurrentMinorRealmItemSelected(long idx)
    {
        Data.CurrentMinorRealm = (Realm.MinorRealm)idx;
        Update();
    }

    private void OnCurrentPercentValueChanged(double value)
    {
        Data.CurrentPercent = (float)value / 100;
        Update();
    }

    private void OnTargetMajorRealmItemSelected(long idx)
    {
        Data.TargetMajorRealm = (Realm.MajorRealm)idx;
        Update();
    }

    private void OnTargetMinorRealmItemSelected(long idx)
    {
        Data.TargetMinorRealm = (Realm.MinorRealm)idx;
        Update();
    }

    #endregion


}

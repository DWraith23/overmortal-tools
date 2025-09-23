using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scripts;

public partial class TargetRealmCalculation : VBoxContainer
{

    #region Nodes

    private OptionButton MajorRealmSelect => GetNode<OptionButton>("Target/Major Realm");
    private OptionButton MinorRealmSelect => GetNode<OptionButton>("Target/Minor Realm");

    private LineEdit NumberOfDaysOutput => GetNode<LineEdit>("Output/Days/LineEdit");

    #endregion


    private ProfileData _data = new();
    public ProfileData Data
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
        PopulateOptionButtons();
        MajorRealmSelect.ItemSelected += item => OnTargetChanged((int)item);
        MinorRealmSelect.ItemSelected += item => OnTargetChanged((int)item);
        Update();
    }

    private void PopulateOptionButtons()
    {
        MajorRealmSelect.Clear();
        foreach (var realm in PathData.RealmNames)
        {
            MajorRealmSelect.AddItem(realm.Value, (int)realm.Key);
        }
        MajorRealmSelect.Selected = (int)PathData.Realm.Novice;

        MinorRealmSelect.Clear();
        foreach (var minorRealm in PathData.MinorRealmNames)
        {
            MinorRealmSelect.AddItem(minorRealm.Value, (int)minorRealm.Key);
        }
        MinorRealmSelect.Selected = (int)PathData.MinorRealm.Early;
    }

    private void Update()
    {
        if (Data == null) return;
        GD.Print("TargetRealmCalculation Update() called");

        if (Data.TargetMajorRealm < Data.Path1.CurrentRealm) Data.TargetMajorRealm = Data.Path1.CurrentRealm;
        if (Data.TargetMajorRealm == Data.Path1.CurrentRealm && Data.TargetMinorRealm < Data.Path1.CurrentMinorRealm) Data.TargetMinorRealm = Data.Path1.CurrentMinorRealm;

        MajorRealmSelect.Select((int)Data.TargetMajorRealm);
        MinorRealmSelect.Select((int)Data.TargetMinorRealm);

        for (int i = 1; i < Enum.GetValues<PathData.Realm>().Length; i++)
        {
            var disabled = i < (int)Data.HighestRealm.Item1;
            MajorRealmSelect.SetItemDisabled(i, disabled);
        }

        for (int i = 1; i < Enum.GetValues<PathData.MinorRealm>().Length; i++)
        {
            var disabled = MajorRealmSelect.Selected <= (int)Data.HighestRealm.Item1 && i < (int)Data.HighestRealm.Item2;
            MinorRealmSelect.SetItemDisabled(i, disabled);
        }

        var days = CultivationTimeSimulation.CountDaysToTargetRealm(
            Data.Path1,
            Data.TotalDailyExp,
            Data.TargetMajorRealm,
            Data.TargetMinorRealm
        );
        NumberOfDaysOutput.Text = days.ToString();

    }

    private void OnTargetChanged(int item)
    {
        if (Data == null) return;

        Data.TargetMajorRealm = (PathData.Realm)MajorRealmSelect.Selected;
        Data.TargetMinorRealm = (PathData.MinorRealm)MinorRealmSelect.Selected;

        // GD.Print($"Target changed to {Data.TargetMajorRealm} ({Data.TargetMajorRealm}) {Data.TargetMinorRealm} ({Data.TargetMinorRealm})");
        
        Update();
    }

}

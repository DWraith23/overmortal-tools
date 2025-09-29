using Godot;
using OvermortalTools.V2.Resources;
using System;
using System.Linq;

namespace OvermortalTools.V2.Scenes;

public partial class PathTabContainer : TabContainer
{
    private PathDataSelection Path1Selection => GetNode<PathDataSelection>("Main");
    private PathDataSelection Path2Selection => GetNode<PathDataSelection>("Aux 1");
    private PathDataSelection Path3Selection => GetNode<PathDataSelection>("Aux 2");
    private PathDataSelection Path4Selection => GetNode<PathDataSelection>("Aux 3");

    private Label Path1Exclamation => GetNode<Label>("Exclamations/Main");
    private Label Path2Exclamation => GetNode<Label>("Exclamations/Aux 1");
    private Label Path3Exclamation => GetNode<Label>("Exclamations/Aux 2");
    private Label Path4Exclamation => GetNode<Label>("Exclamations/Aux 3");

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

    private void Update()
    {
        Path1Selection.Data = Data.Path1;
        Path2Selection.Data = Data.Path2;
        Path3Selection.Data = Data.Path3;
        Path4Selection.Data = Data.Path4;

        bool path2Disabled = Data.HighestRealm.Item1 < PathData.Realm.NascentSoul;
        bool path3Disabled = Data.GetPathsInOrder()
            .Where(p => p.CurrentRealm != PathData.Realm.None)
            .Any(p => p.CurrentRealm < PathData.Realm.Voidbreak) ||
            Data.Path1.CurrentRealm == PathData.Realm.None ||
            Data.Path2.CurrentRealm == PathData.Realm.None;
        bool path4Disabled = Data.GetPathsInOrder()
            .Where(p => p.CurrentRealm != PathData.Realm.None)
            .Any(p => p.CurrentRealm < PathData.Realm.Voidbreak) ||
            Data.Path1.CurrentRealm == PathData.Realm.None ||
            Data.Path2.CurrentRealm == PathData.Realm.None ||
            Data.Path3.CurrentRealm == PathData.Realm.None;

        SetTabDisabled(1, path2Disabled);
        SetTabDisabled(2, path3Disabled);
        SetTabDisabled(3, path4Disabled);

        Path1Exclamation.Visible = Data.Path1.MainNeedsAttention;
        Path2Exclamation.Visible = Data.Path2.NeedsAttention && !path2Disabled;
        Path3Exclamation.Visible = Data.Path3.NeedsAttention && !path3Disabled;
        Path4Exclamation.Visible = Data.Path4.NeedsAttention && !path4Disabled;
    }
}

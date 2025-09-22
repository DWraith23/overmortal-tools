using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class PathTabContainer : TabContainer
{
    private PathDataSelection Path1Selection => GetNode<PathDataSelection>("Main");
    private PathDataSelection Path2Selection => GetNode<PathDataSelection>("Aux 1");
    private PathDataSelection Path3Selection => GetNode<PathDataSelection>("Aux 2");
    private PathDataSelection Path4Selection => GetNode<PathDataSelection>("Aux 3");

    private ProfileData _data = new();
    public ProfileData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            Update();
        }
    }

    private void Update()
    {
        Path1Selection.Data = Data.Path1;
        Path2Selection.Data = Data.Path2;
        Path3Selection.Data = Data.Path3;
        Path4Selection.Data = Data.Path4;
    }
}

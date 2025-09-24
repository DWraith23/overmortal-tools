using Godot;
using OvermortalTools.V2.Resources;
using OvermortalTools.V2.Scripts;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class TargetsTabContainer : TabContainer
{
    private TargetRealmCalculation Target => GetNode<TargetRealmCalculation>("Target");
    private ViryaCalculation Virya => GetNode<ViryaCalculation>("Virya");

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
        Target.Data = Data;
        Virya.Data = Data;
    }
}

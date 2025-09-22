using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class DailyExpTabContainer : TabContainer
{
    #region Nodes

    private PassiveDataSelection Passive => GetNode<PassiveDataSelection>("Passive");
    private PillDataSelection Pills => GetNode<PillDataSelection>("Pills");
    private RespiraDataSelection Respira => GetNode<RespiraDataSelection>("Respira");

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
        Update();
    }

    private void Update()
    {
        GD.Print("DailyExpTabContainer Update() called");

        Passive.Data = Data.PassiveCultivation;
        Pills.Data = Data.PillData;
        Respira.Profile = Data;
    }

}

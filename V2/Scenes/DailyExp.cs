using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class DailyExp : VBoxContainer
{

    #region Nodes
    private LineEdit PassiveExpOutput => GetNode<LineEdit>("Passive/LineEdit");
    private LineEdit PillExpOutput => GetNode<LineEdit>("Pills/LineEdit");
    private LineEdit RespiraExpOutput => GetNode<LineEdit>("Respira/LineEdit");
    private LineEdit TotalExpOutput => GetNode<LineEdit>("Total/LineEdit");

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

    private void Update()
    {
        GD.Print("DailyExp Update() called");

        PassiveExpOutput.Text = Data.DailyPassiveExp.ToString("N0");
        PillExpOutput.Text = Data.DailyPillExp.ToString("N0");
        RespiraExpOutput.Text = Data.DailyRespiraExp.ToString("N0");
        TotalExpOutput.Text = Data.TotalDailyExp.ToString("N0");
    }
}

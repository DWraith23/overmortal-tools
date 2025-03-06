using Godot;
using OvermortalTools.Resources.Planner;
using System;
using System.Collections.Generic;
using System.Data;

namespace OvermortalTools.Scenes.Planner;

public partial class MirrorStatus : VBoxContainer
{
    /// <summary>
    /// Signal sent when anything is changed in the UI.
    /// </summary>
    [Signal] public delegate void ValuesChangedEventHandler();

    [Export] private CheckBox HasArtifactCheck { get; set; }
    [Export] private HBoxContainer StarsContainer { get; set; }
    [Export] private SpinBox StarsBox { get; set; }

    private MirrorData _data = new();
    public MirrorData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
        }
    }

    public MirrorStatus()
    {
        _data.Changed += Update;
    }


    #region Events

    private void OnHasMirrorCheckboxToggled(bool toggledOn) => Data.HasMirror = toggledOn;
    private void OnStarsSpinBoxValueChanged(double value) => Data.Stars = (int)value;

    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating MirrorStatus.");
        StarsContainer.Visible = Data.HasMirror;
        EmitSignal(SignalName.ValuesChanged);
    }
}

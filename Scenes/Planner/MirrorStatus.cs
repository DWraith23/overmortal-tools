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
            Update();
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

    /// <summary>
    /// Updates the UI.  This is called any time the Data resource changes.
    /// </summary>
    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating MirrorStatus.");

        CheckValues();

        StarsContainer.Visible = Data.HasMirror;
        EmitSignal(SignalName.ValuesChanged);
    }

    /// <summary>
    /// Verifies that the UI matches the Data container values.
    /// </summary>
    private void CheckValues()
    {
        if (HasArtifactCheck.ButtonPressed != Data.HasMirror) HasArtifactCheck.ButtonPressed = Data.HasMirror;
        if (StarsBox.Value != Data.Stars) StarsBox.Value = Data.Stars;
    }
}

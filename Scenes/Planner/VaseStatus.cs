using Godot;
using OvermortalTools.Resources.Planner;
using System;
using System.Collections.Generic;

namespace OvermortalTools.Scenes.Planner;

public partial class VaseStatus : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    [Export] private CheckBox HasArtifactCheck { get; set; }
    [Export] private HBoxContainer StarsContainer { get; set; }
    [Export] private SpinBox StarsBox { get; set; }

    private VaseData _data = new();
    public VaseData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
            Update();
        }
    }

    #region Events

    private void OnHasVaseCheckboxToggled(bool toggledOn) => Data.HasVase = toggledOn;
    private void OnStarsSpinBoxValueChange(double value) => Data.Stars = (int)value;

    #endregion

    public VaseStatus()
    {
        _data.Changed += Update;
    }

    /// <summary>
    /// Updates the UI.  This is called any time the Data resource changes.
    /// </summary>
    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating VaseStatus.");
        CheckValues();

        StarsContainer.Visible = Data.HasVase;
        EmitSignal(SignalName.ValuesChanged);
    }

    /// <summary>
    /// Verifies that the UI matches the Data container values.
    /// </summary>
    private void CheckValues()
    {
        if (HasArtifactCheck.ButtonPressed != Data.HasVase) HasArtifactCheck.ButtonPressed = Data.HasVase;
        if (StarsBox.Value != Data.Stars) StarsBox.Value = Data.Stars;
    }
}

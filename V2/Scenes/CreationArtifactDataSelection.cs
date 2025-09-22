using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class CreationArtifactDataSelection : VBoxContainer
{
    private CheckBox HasVaseCheckBox => GetNode<CheckBox>("Artifacts/Vase/CheckBox");
    private CheckBox HasMirrorCheckBox => GetNode<CheckBox>("Artifacts/Mirror/CheckBox");
    private CheckBox HasVaseTransmogCheckBox => GetNode<CheckBox>("Artifacts/Vase/Panel/Info/CheckBox");

    private SpinBox VaseStarsSpinBox => GetNode<SpinBox>("Artifacts/Vase/Panel/Info/Stars/SpinBox");
    private SpinBox MirrorStarsSpinBox => GetNode<SpinBox>("Artifacts/Mirror/Panel/Info/Stars/SpinBox");

    private PanelContainer VasePanel => GetNode<PanelContainer>("Artifacts/Vase/Panel");
    private PanelContainer MirrorPanel => GetNode<PanelContainer>("Artifacts/Mirror/Panel");

    private ProfileData _profile = new();
    public ProfileData Profile
    {
        get => _profile;
        set
        {
            if (_profile == value) return;
            _profile = value;
            Update();
            if (value != null)
            {
                _profile.Changed += Update;
            }
        }
    }

    private CreationArtifactData Data => Profile.CreationArtifacts;

    public override void _Ready()
    {
        ConnectSignals();
        Update();
    }

    private void ConnectSignals()
    {
        _profile.Changed += Update;

        HasVaseCheckBox.Toggled += pressed =>
        {
            if (Data == null) return;
            Data.HasVase = pressed;
        };

        HasMirrorCheckBox.Toggled += pressed =>
        {
            if (Data == null) return;
            Data.HasMirror = pressed;
        };

        VaseStarsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.VaseStars = (int)value;
        };

        MirrorStarsSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.MirrorStars = (int)value;
        };

        HasVaseTransmogCheckBox.Toggled += pressed =>
        {
            if (Data == null) return;
            Data.HasVaseTransmog = pressed;
        };
    }


    private void Update()
    {
        if (Data == null) return;

        GD.Print("CreationArtifactDataSelection Update() called");

        HasVaseCheckBox.ButtonPressed = Data.HasVase;
        HasMirrorCheckBox.ButtonPressed = Data.HasMirror;

        VasePanel.Visible = Data.HasVase;
        MirrorPanel.Visible = Data.HasMirror;

        VaseStarsSpinBox.Value = Data.VaseStars;
        MirrorStarsSpinBox.Value = Data.MirrorStars;

        HasVaseTransmogCheckBox.ButtonPressed = Data.HasVaseTransmog;
    }

}

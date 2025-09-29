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
    private CreationArtifactDataSelection Artifacts => GetNode<CreationArtifactDataSelection>("Artifacts");

    private Label PassiveExclamation => GetNode<Label>("Exclamations/Passive");
    private Label PillExclamation => GetNode<Label>("Exclamations/Pills");
    private Label RespiraExclamation => GetNode<Label>("Exclamations/Respira");
    private Label ArtifactExclamation => GetNode<Label>("Exclamations/Artifacts");


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

        Passive.Profile = Data;
        Pills.Profile = Data;
        Respira.Profile = Data;
        Artifacts.Profile = Data;

        PassiveExclamation.Visible = Data.PassiveCultivation.NeedsAttention;
        PillExclamation.Visible = Data.PillData.NeedsAttention;
        RespiraExclamation.Visible = Data.RespiraData.NeedsAttention;
        ArtifactExclamation.Visible = Data.CreationArtifacts.NeedsAttention;

    }


}

using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class StageCalculator : VBoxContainer
{

    [Signal] public delegate void ValuesChangedEventHandler();

    #region Exports
    [ExportGroup("Nodes")]
    [ExportSubgroup("Current")]
    [Export] private OptionButton CurrentMajorRealm { get; set; }
    [Export] private OptionButton CurrentMinorRealm { get; set; }
    [Export] private OptionButton CurrentStage { get; set; }
    [Export] private LineEdit CurrentPercent { get; set; }
    [ExportSubgroup("Target")]
    [Export] private OptionButton TargetMajorRealm { get; set; }
    [Export] private OptionButton TargetMinorRealm { get; set; }
    [Export] private OptionButton TargetStage { get; set; }
    [ExportSubgroup("Details")]
    [Export] private LineEdit RemainingXp { get; set; }

    #endregion

    private StageCalculatorData _data = new();
    public StageCalculatorData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
        }
    }

    private int PreviousCurrentMajorRealmIndex = -1;
    private int PreviousCurrentMinorRealmIndex = -1;
    private int PreviousCurrentStageIndex = -1;
    private int PreviousTargetMajorRealmIndex = -1;
    private int PreviousTargetMinorRealmIndex = -1;
    private int PreviousTargetStageIndex = -1;


    public StageCalculator()
    {
        _data.Changed += Update;
    }

    public float CurrentPercentValue { get; set; } = 0;

    public override void _Ready()
    {
        AddCurrentMajorRealmOptions();
    }

    /// <summary>
    /// Adds all Major Realms to the CurrentMajorRealm OptionButton.
    /// </summary>
    private void AddCurrentMajorRealmOptions()
    {
        CurrentMajorRealm.Clear();

        foreach (var realm in CultivationStage.MajorRealms)
        {
            CurrentMajorRealm.AddItem(realm);
        }
        SetCurrentMajorRealm();
        ValidateData();
    }

    #region Events

    private void OnCurrentMajorRealmSelected(int index) => SetCurrentMajorRealm();
    private void OnCurrentMinorRealmSelected(int index) => SetCurrentMinorRealm();
    private void OnCurrentStageSelected(int index) => SetCurrentStage();
    private void OnCurrentPercentChanged(string text) => SetCurrentPercent(text);
    private void OnTargetMajorRealmSelected(int index) => SetTargetMajorRealm();
    private void OnTargetMinorRealmSelected(int index) => SetTargetMinorRealm();
    private void OnTargetStageSelected(int index) => SetTargetStage();

    #endregion

    #region Actions

    private void SetCurrentMajorRealm()
    {
        Data.CurrentMajorRealmIndex = CurrentMajorRealm.Selected;
        Data.CurrentMajorRealm = CurrentMajorRealm.Text;
        HandleTargetMajorRealmOptions();
        HandleCurrentMinorRealmOptions();
    }

    private void SetCurrentMinorRealm()
    {
        Data.CurrentMinorRealmIndex = CurrentMinorRealm.Selected;
        Data.CurrentMinorRealm = CurrentMinorRealm.Text;
        HandleTargetMinorRealmOptions();
        HandleCurrentStageOptions();
    }

    private void SetCurrentStage()
    {
        Data.CurrentStageIndex = CurrentStage.Selected;
        Data.CurrentStage = CurrentStage.Text;
        HandleTargetStageOptions();
    }

    private void SetCurrentPercent(string text)
    {
        if (text == "")
        {
            Data.CurrentPercent = 0f;
            CurrentPercent.Text = (CurrentPercentValue / 100f).ToString("P2");
        }
        if (!text.IsValidFloat())
        {
            CurrentPercent.DeleteLastCharacter();
            return;
        }
        Data.CurrentPercent = float.Parse(text) / 100f; // Converts the full % number into an actual percent value.
    }

    private void SetTargetMajorRealm()
    {
        Data.TargetMajorRealmIndex = TargetMajorRealm.Selected;
        Data.TargetMajorRealm = TargetMajorRealm.Text;
        HandleTargetMinorRealmOptions();
    }

    private void SetTargetMinorRealm()
    {
        Data.TargetMinorRealmIndex = TargetMinorRealm.Selected;
        Data.TargetMinorRealm = TargetMinorRealm.Text;
        HandleTargetStageOptions();
    }

    private void SetTargetStage()
    {
        Data.TargetStageIndex = TargetStage.Selected;
        Data.TargetStage = TargetStage.Text;
    }
    
    #endregion
    
    /// <summary>
    /// Updates the UI.
    /// </summary>
    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating StageCalculator.");

        SetRemainingXpText();
    }

    private void ValidateIndices()
    {
        if (CurrentMajorRealm.Selected != Data.CurrentMajorRealmIndex) Data.CurrentMajorRealmIndex = CurrentMajorRealm.Selected;
        if (CurrentMinorRealm.Selected != Data.CurrentMinorRealmIndex) Data.CurrentMinorRealmIndex = CurrentMinorRealm.Selected;
        if (CurrentStage.Selected != Data.CurrentStageIndex) Data.CurrentStageIndex = CurrentStage.Selected;
        if (TargetMajorRealm.Selected != Data.TargetMajorRealmIndex) Data.TargetMajorRealmIndex = TargetMajorRealm.Selected;
        if (TargetMinorRealm.Selected != Data.TargetMinorRealmIndex) Data.TargetMinorRealmIndex = TargetMinorRealm.Selected;
        if (TargetStage.Selected != Data.TargetStageIndex) Data.TargetStageIndex = TargetStage.Selected;
    }

    private void ValidateNames()
    {
        if (CurrentMajorRealm.Text != Data.CurrentMajorRealm) Data.CurrentMajorRealm = CurrentMajorRealm.Text;
        if (CurrentMinorRealm.Text != Data.CurrentMinorRealm) Data.CurrentMinorRealm = CurrentMinorRealm.Text;
        if (CurrentStage.Text != Data.CurrentStage) Data.CurrentStage = CurrentStage.Text;
        if (TargetMajorRealm.Text != Data.TargetMajorRealm) Data.TargetMajorRealm = TargetMajorRealm.Text;
        if (TargetMinorRealm.Text != Data.TargetMinorRealm) Data.TargetMinorRealm = TargetMinorRealm.Text;
        if (TargetStage.Text != Data.TargetStage) Data.TargetStage = TargetStage.Text;
    }

    private void ValidateData()
    {
        ValidateIndices();
        ValidateNames();
    }

    private void SetRemainingXpText()
    {
        RemainingXp.Text =
        Data.TargetXp == 0
            ? "---"
            : (Data.TargetXp - Data.CurrentXp).ToString("N0");
    }

    #region OptionButton Handling
    private void HandleCurrentMinorRealmOptions()
    {
        CurrentMinorRealm.Clear();

        if (Data.CurrentMajorRealmIndex == -1) return;

        foreach (var realm in CultivationStage.MinorRealms(Data.CurrentMajorRealm))
        {
            CurrentMinorRealm.AddItem(realm);
        }
        ValidateData();
        HandleCurrentStageOptions();
        HandleTargetMinorRealmOptions();
    }

    private void HandleCurrentStageOptions()
    {
        CurrentStage.Clear();

        if (Data.CurrentMinorRealmIndex == -1) return;

        foreach (var realm in CultivationStage.Stages(Data.CurrentMajorRealm, Data.CurrentMinorRealm))
        {
            CurrentStage.AddItem(realm);
        }
        ValidateData();
        HandleTargetStageOptions();
    }

    private void HandleTargetMajorRealmOptions()
    {
        TargetMajorRealm.Clear();

        for (int i = 0; i < CultivationStage.MajorRealms.Count; i++)
        {
            if (i < Data.CurrentMajorRealmIndex) continue;    // Don't include target realms lower than current.

            TargetMajorRealm.AddItem(CultivationStage.MajorRealms[i]);
        }
        ValidateData();
        HandleTargetMinorRealmOptions();
    }

    private void HandleTargetMinorRealmOptions()
    {
        TargetMinorRealm.Clear();

        // if (Data.TargetMajorRealmIndex == -1) return;

        if (!Data.CurrentMajorRealm.Equals(Data.TargetMajorRealm))
        {
            for (int i = 0; i < CultivationStage.MinorRealms(Data.TargetMajorRealm).Count; i++)
            {
                TargetMinorRealm.AddItem(CultivationStage.MinorRealms(Data.TargetMajorRealm)[i]);
            }
        }
        else
        {
            for (int i = 0; i < CultivationStage.MinorRealms(Data.TargetMajorRealm).Count; i++)
            {
                if (i < Data.CurrentMinorRealmIndex) continue;
                TargetMinorRealm.AddItem(CultivationStage.MinorRealms(Data.TargetMajorRealm)[i]);
            }
        }
        ValidateData();
        HandleTargetStageOptions();
    }

    private void HandleTargetStageOptions()
    {
        TargetStage.Clear();

        // if (Data.TargetMajorRealmIndex == -1 || Data.TargetMinorRealmIndex == -1) return;

        if (Data.CurrentMajorRealm.Equals(Data.TargetMajorRealm) && Data.CurrentMinorRealm.Equals(Data.TargetMinorRealm))
        {
            for (int i = 0; i < CultivationStage.Stages(Data.TargetMajorRealm, Data.TargetMinorRealm).Count; i++)
            {
                if (i < Data.CurrentStageIndex) continue;

                TargetStage.AddItem(CultivationStage.Stages(Data.TargetMajorRealm, Data.TargetMinorRealm)[i]);
            }
        }
        else
        {
            for (int i = 0; i < CultivationStage.Stages(Data.TargetMajorRealm, Data.TargetMinorRealm).Count; i++)
            {
                TargetStage.AddItem(CultivationStage.Stages(Data.TargetMajorRealm, Data.TargetMinorRealm)[i]);
            }
        }
        ValidateData();
    }

    private bool NeedsToUpdate(string type)
    {
        var currentMajorIdx = CultivationStage.MajorRealms.Contains(Data.CurrentMajorRealm)
            ? CultivationStage.MajorRealms.IndexOf(Data.CurrentMajorRealm)
            : -1;
        var targetMajorIdx = CultivationStage.MajorRealms.Contains(Data.TargetMajorRealm)
            ? CultivationStage.MajorRealms.IndexOf(Data.TargetMajorRealm)
            : -1;

        var currentMinorIdx = CultivationStage.MinorRealms(Data.CurrentMajorRealm).Contains(Data.CurrentMinorRealm)
            ? CultivationStage.MinorRealms(Data.CurrentMajorRealm).IndexOf(Data.CurrentMinorRealm)
            : -1;
        var targetMinorIdx = CultivationStage.MinorRealms(Data.TargetMajorRealm).Contains(Data.TargetMinorRealm)
            ? CultivationStage.MinorRealms(Data.TargetMajorRealm).IndexOf(Data.TargetMinorRealm)
            : -1;

        var currentStageIdx = CultivationStage.Stages(Data.CurrentMajorRealm, Data.CurrentMinorRealm).Contains(Data.CurrentStage)
            ? CultivationStage.Stages(Data.CurrentMajorRealm, Data.CurrentMinorRealm).IndexOf(Data.CurrentStage)
            : -1;
        var targetStageIdx = CultivationStage.Stages(Data.TargetMajorRealm, Data.TargetMinorRealm).Contains(Data.CurrentStage)
            ? CultivationStage.Stages(Data.TargetMajorRealm, Data.TargetMinorRealm).IndexOf(Data.CurrentStage)
            : -1;
            
        if (type == "Major")
        {
            
        }
        else if (type == "Minor")
        {
            

        }
        else if (type == "Stage")
        {


        }
        return false;
    }

    #endregion
}

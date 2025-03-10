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
            IntializeLoadedData();
            Update();
        }
    }

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
        ValidateNames();
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
        HandleCurrentMinorRealmOptions();
        HandleTargetMajorRealmOptions();
    }

    private void SetCurrentMinorRealm()
    {
        Data.CurrentMinorRealmIndex = CurrentMinorRealm.Selected;
        Data.CurrentMinorRealm = CurrentMinorRealm.Text;
        HandleCurrentStageOptions();
        HandleTargetMinorRealmOptions();
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
            return;
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

        ValidateNames();
        SetRemainingXpText();

        EmitSignal(SignalName.ValuesChanged);
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

    private void SetRemainingXpText()
    {
        RemainingXp.Text =
        Data.TargetXp == 0
            ? "---"
            : (Data.TargetXp - Data.CurrentXp).ToString("N0");
    }

    /// <summary>
    /// Sets the OptionButtons from loaded data.
    /// </summary>
    private void IntializeLoadedData()
    {
        var cMajIdx = Data.CurrentMajorRealmIndex;
        var cMinIdx = Data.CurrentMinorRealmIndex;
        var cStaIdx = Data.CurrentStageIndex;
        var tMajIdx = Data.TargetMajorRealmIndex;
        var tMinIdx = Data.TargetMinorRealmIndex;
        var tStaIdx = Data.TargetStageIndex;

        CurrentMajorRealm.Select(cMajIdx);
        SetCurrentMajorRealm();
        CurrentMinorRealm.Select(cMinIdx);
        SetCurrentMinorRealm();
        CurrentStage.Select(cStaIdx);
        SetCurrentStage();
        TargetMajorRealm.Select(tMajIdx);
        SetTargetMajorRealm();
        TargetMinorRealm.Select(tMinIdx);
        SetTargetMinorRealm();
        TargetStage.Select(tStaIdx);
        SetTargetStage();
        CurrentPercent.Text = $"{Data.CurrentPercent * 100f:N2}";
    }

    #region OptionButton Handling

    private void HandleOptionButtons()
    {
        
    }

    
    private void HandleCurrentMinorRealmOptions()
    {
        CurrentMinorRealm.Clear();

        if (CurrentMajorRealm.Selected == -1) return;

        foreach (var realm in CultivationStage.MinorRealms(Data.CurrentMajorRealm))
        {
            CurrentMinorRealm.AddItem(realm);
        }
        Data.CurrentMinorRealmIndex = 0;
        HandleCurrentStageOptions();
    }

    private void HandleCurrentStageOptions()
    {
        CurrentStage.Clear();

        if (CurrentMinorRealm.Selected == -1) return;

        foreach (var realm in CultivationStage.Stages(Data.CurrentMajorRealm, Data.CurrentMinorRealm))
        {
            CurrentStage.AddItem(realm);
        }
        Data.CurrentStageIndex = 0;
    }

    private void HandleTargetMajorRealmOptions()
    {
        TargetMajorRealm.Clear();

        for (int i = 0; i < CultivationStage.MajorRealms.Count; i++)
        {
            if (i < Data.CurrentMajorRealmIndex) continue;    // Don't include target realms lower than current.

            TargetMajorRealm.AddItem(CultivationStage.MajorRealms[i]);
        }
        Data.TargetMajorRealmIndex = 0;
        HandleTargetMinorRealmOptions();
    }

    private void HandleTargetMinorRealmOptions()
    {
        TargetMinorRealm.Clear();

        if (TargetMajorRealm.Selected == -1) return;

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
        Data.TargetMinorRealmIndex = 0;
        HandleTargetStageOptions();
    }

    private void HandleTargetStageOptions()
    {
        TargetStage.Clear();

        if (TargetMinorRealm.Selected == -1) return;

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
        Data.TargetStageIndex = 0;
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

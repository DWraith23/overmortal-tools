using Godot;
using OvermortalTools.Resources;
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

    public float CurrentPercentValue { get; set; } = 0;

    public override void _Ready()
    {
        SetCurrentMajorRealms();
        CurrentMajorRealmChanged(0);
        SetRemainingXpText();
    }

    private void SetCurrentMajorRealms()
    {
        CurrentMajorRealm.Clear();

        foreach (var realm in CultivationStage.MajorRealms)
        {
            CurrentMajorRealm.AddItem(realm);
        }
    }

    private void SetCurrentMinorRealms()
    {
        CurrentMinorRealm.Clear();

        if (CurrentMajorRealm.Selected == -1) return;

        foreach (var realm in CultivationStage.MinorRealms(CurrentMajorRealm.GetItemText(CurrentMajorRealm.Selected)))
        {
            CurrentMinorRealm.AddItem(realm);
        }
    }

    private void SetCurrentStages()
    {
        CurrentStage.Clear();

        if (CurrentMajorRealm.Selected == -1 || CurrentMinorRealm.Selected == -1) return;

        var cMajor = CurrentMajorRealm.GetItemText(CurrentMajorRealm.Selected);
        var cMinor = CurrentMinorRealm.GetItemText(CurrentMinorRealm.Selected);

        foreach (var realm in CultivationStage.Stages(cMajor, cMinor))
        {
            CurrentStage.AddItem(realm);
        }
    }

    #region Data

    public int CurrentXp =>
        CurrentMajorRealm.Selected == -1 || CurrentMinorRealm.Selected == -1 || CurrentStage.Selected == -1
            ? 0
            : CultivationStage.GetStage(
                CurrentMajorRealm.GetItemText(CurrentMajorRealm.Selected),
                CurrentMinorRealm.GetItemText(CurrentMinorRealm.Selected),
                CurrentStage.GetItemText(CurrentStage.Selected)
            ).GetTotalXp(CurrentPercentValue);

    public int TargetXp =>
        TargetMajorRealm.Selected == -1 || TargetMinorRealm.Selected == -1 || TargetStage.Selected == -1
            ? 0
            : CultivationStage.GetXpToComplete(CultivationStage.GetStage(
                TargetMajorRealm.GetItemText(TargetMajorRealm.Selected),
                TargetMinorRealm.GetItemText(TargetMinorRealm.Selected),
                TargetStage.GetItemText(TargetStage.Selected)
            ));

    public int RemainingXpValue => TargetXp - CurrentXp;

    public string MajorRealm => CurrentMajorRealm.GetItemText(CurrentMajorRealm.Selected);
    public string MinorRealm => CurrentMinorRealm.GetItemText(CurrentMinorRealm.Selected);
    public string Stage => CurrentStage.GetItemText(CurrentStage.Selected);

    #endregion

    #region Events

    private void CurrentMajorRealmChanged(long index) => SetTargetButtons(0);
    private void CurrentMinorRealmChanged(long index) => SetTargetButtons(1);
    private void CurrentStageChanged(long index) => SetTargetButtons(2);
    private void CurrentPercentChanged(string text) => SetCurrentPercent(text);
    private void TargetMajorRealmChanged(long index) => CheckTargetRealms(0);
    private void TargetMinorRealmChanged(long index) => CheckTargetRealms(1);
    private void TargetStageChanged(long index) => CheckTargetRealms(2);


    #endregion

    #region Actions

    private void SetTargetButtons(int code)
    {
        int majorIndex = CurrentMajorRealm.Selected;
        int minorIndex = CurrentMinorRealm.Selected;
        int stageIndex = CurrentStage.Selected;

        if (code == 0) SetCurrentMinorRealms();
        if (code == 0 || code == 1) SetCurrentStages();

        SetTargetMajorRealms(majorIndex);
        SetTargetMinorRealms(minorIndex);
        SetTargetStages(stageIndex);

        SetRemainingXpText();
    }

    private void CheckTargetRealms(int code)
    {
        if (code == 0) SetTargetMinorRealms(CurrentMinorRealm.Selected);
        if (code == 0 || code == 1) SetTargetStages(CurrentStage.Selected);
        SetRemainingXpText();
    }

    private void SetTargetMajorRealms(int index)
    {

        TargetMajorRealm.Clear();

        for (int i = 0; i < CultivationStage.MajorRealms.Count; i++)
        {
            if (i < index) continue;    // Don't include target realms lower than current.

            TargetMajorRealm.AddItem(CultivationStage.MajorRealms[i]);
        }
    }

    private void SetTargetMinorRealms(int index)
    {
        if (TargetMajorRealm.Selected == -1) return;

        TargetMinorRealm.Clear();

        if (CurrentMajorRealm.Text != TargetMajorRealm.Text)
        {
            for (int i = 0; i < CultivationStage.MinorRealms(TargetMajorRealm.GetItemText(TargetMajorRealm.Selected)).Count; i++)
            {
                TargetMinorRealm.AddItem(CultivationStage.MinorRealms(TargetMajorRealm.GetItemText(TargetMajorRealm.Selected))[i]);
            }
        }
        else
        {
            for (int i = 0; i < CultivationStage.MinorRealms(TargetMajorRealm.GetItemText(TargetMajorRealm.Selected)).Count; i++)
            {
                if (i < index) continue;

                TargetMinorRealm.AddItem(CultivationStage.MinorRealms(TargetMajorRealm.GetItemText(TargetMajorRealm.Selected))[i]);
            }
        }
    }

    private void SetTargetStages(int index)
    {
        if (TargetMajorRealm.Selected == -1 || TargetMinorRealm.Selected == -1) return;

        var cMajorText = CurrentMajorRealm.GetItemText(CurrentMajorRealm.Selected);
        var cMinorText = CurrentMinorRealm.GetItemText(CurrentMinorRealm.Selected);
        var tMajorText = TargetMajorRealm.GetItemText(TargetMajorRealm.Selected);
        var tMinorText = TargetMinorRealm.GetItemText(TargetMinorRealm.Selected);

        TargetStage.Clear();

        if (cMajorText == tMajorText && cMinorText == tMinorText)
        {
            for (int i = 0; i < CultivationStage.Stages(cMajorText, cMinorText).Count; i++)
            {
                if (i < index) continue;

                TargetStage.AddItem(CultivationStage.Stages(cMajorText, cMinorText)[i]);
            }
        }
        else
        {
            for (int i = 0; i < CultivationStage.Stages(tMajorText, tMinorText).Count; i++)
            {
                TargetStage.AddItem(CultivationStage.Stages(tMajorText, tMinorText)[i]);
            }
        }
    }

    private void SetCurrentPercent(string text)
    {
        if (text == "")
        {
            CurrentPercentValue = 0;
            CurrentPercent.Text = (CurrentPercentValue / 100f).ToString("P2");
        }
        if (!text.IsValidFloat())
        {
            CurrentPercent.DeleteLastCharacter();
            return;
        }
        CurrentPercentValue = float.Parse(text) / 100f;
        GD.Print($"Value: {CurrentPercentValue}");
        SetRemainingXpText();
    }

    private void SetRemainingXpText()
    {
        RemainingXp.Text =
        TargetXp == 0
            ? "---"
            : (TargetXp - CurrentXp).ToString("N0");

        EmitSignal(SignalName.ValuesChanged);
    }


    #endregion
}

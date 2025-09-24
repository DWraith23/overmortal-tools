using Godot;
using OvermortalTools.Scenes.Planner;
using OvermortalTools.V2.Resources;
using OvermortalTools.V2.Scenes;
using OvermortalTools.V2.Scripts;
using System;

public partial class MainPanel : PanelContainer
{
    #region Nodes
    private VBoxContainer Contents => GetNode<VBoxContainer>("Contents");

    private TabContainer InfoTabs => Contents.GetNode<TabContainer>("Info Tabs");
    private DailyExp DailyExp => InfoTabs.GetNode<DailyExp>("Daily");
    private TargetsTabContainer TargetRealms => InfoTabs.GetNode<TargetsTabContainer>("Targets");
    private CalculatedValues MiscValues => InfoTabs.GetNode<CalculatedValues>("Misc");
    

    private TabContainer SectionTabs => Contents.GetNode<TabContainer>("Section Tabs");
    private PathTabContainer Paths => SectionTabs.GetNode<PathTabContainer>("Paths");
    private DailyExpTabContainer Daily => SectionTabs.GetNode<DailyExpTabContainer>("Daily");
    private StarMarks StarMarks => SectionTabs.GetNode<StarMarks>("Star Marks");
    private MyrimonDataSelection Myrimon => SectionTabs.GetNode<MyrimonDataSelection>("Myrimon");

    private ProfileSwapper Profiles => Contents.GetNode<ProfileSwapper>("Profile Swapper");

    private Button SettingsButton => GetNode<Button>("Settings Button");


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
        ConnectSignals();
    }

    private void Update()
    {
        GD.Print("MainPanel Update() called");

        Paths.Data = Data;
        Daily.Data = Data;
        StarMarks.Data = Data.StarMarks;
        Myrimon.Profile = Data;

        DailyExp.Data = Data;
        TargetRealms.Data = Data;
        MiscValues.Data = Data;
    }

    private void ConnectSignals()
    {
        _data.Changed += Update;

    }
}

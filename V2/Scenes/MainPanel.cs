using Godot;
using OvermortalTools.Scenes.Planner;
using OvermortalTools.V2.Resources;
using OvermortalTools.V2.Scenes;
using OvermortalTools.V2.Scripts;
using System;

public partial class MainPanel : PanelContainer
{
    #region Nodes
    private VBoxContainer Cultivation => GetNode<VBoxContainer>("Sections/Cultivation");

    #region Cultivation Nodes
    private TabContainer InfoTabs => Cultivation.GetNode<TabContainer>("Info Tabs");
    private DailyExp DailyExp => InfoTabs.GetNode<DailyExp>("Daily");
    private TargetsTabContainer TargetRealms => InfoTabs.GetNode<TargetsTabContainer>("Targets");
    private CalculatedValues MiscValues => InfoTabs.GetNode<CalculatedValues>("Misc");
    

    private TabContainer SectionTabs => Cultivation.GetNode<TabContainer>("Section Tabs");
    private PathTabContainer Paths => SectionTabs.GetNode<PathTabContainer>("Paths");
    private DailyExpTabContainer Daily => SectionTabs.GetNode<DailyExpTabContainer>("Daily");
    private StarMarks StarMarks => SectionTabs.GetNode<StarMarks>("Star Marks");
    private MyrimonDataSelection Myrimon => SectionTabs.GetNode<MyrimonDataSelection>("Myrimon");

    #endregion

    private LawsCalculator Laws => GetNode<LawsCalculator>("Sections/Laws");


    private TabBar FeaturesTabs => GetNode<TabBar>("Sections/Bottom Bar/Features");

    private ProfileSwapper Profiles => GetNode<ProfileSwapper>("Sections/Profile Swapper");

    private Button SettingsButton => GetNode<Button>("Sections/Bottom Bar/Settings Button");


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

    private int ActiveProfile { get; set; } = 0;

    public override void _EnterTree()
    {
        ProfileManagement.FileAlterationInProgress = true;
    }


    public override void _Ready()
    {
        Update();
        ConnectSignals();
        var profile = ProfileManagement.LoadProfile(0);
        if (profile != null) Data = profile;
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

        Laws.Data = Data;

        Profiles.ProfileName.Text = Data.ProfileName;

        ProfileManagement.SaveProfile(Data, ActiveProfile);
    }

    private void ConnectSignals()
    {
        _data.Changed += Update;

        FeaturesTabs.TabChanged += tab =>
        {
            switch (tab)
            {
                case 0:
                    Cultivation.Visible = true;
                    Laws.Visible = false;
                    break;
                case 1:
                    Cultivation.Visible = false;
                    Laws.Visible = true;
                    break;
                default:
                    break;
            }
        };

        Profiles.SwapButtonPressed += profile =>
        {
            var loaded = ProfileManagement.LoadProfile(profile);
            if (loaded == null) return;
            Data = loaded;
            ActiveProfile = profile;
            Profiles.ProfileName.Text = Data.ProfileName;
            GD.Print($"Profile {ActiveProfile + 1} loaded");
        };

        Profiles.ProfileNameChanged += name =>
        {
            GD.Print($"Profile {ActiveProfile + 1} name changed to {name}");
            Data.ProfileName = name;
            ProfileManagement.SaveProfile(Data, ActiveProfile);
        };

    }
}

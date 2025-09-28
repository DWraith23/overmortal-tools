using Godot;
using OvermortalTools.Scripts;
using OvermortalTools.V2.Resources;

namespace OvermortalTools.V2.Scenes;

public partial class LawCalculatorOutput : VBoxContainer
{
    private VBoxContainer Laws => GetNode<VBoxContainer>("Laws");
    private LawOutputNode Metal => Laws.GetNode<LawOutputNode>("Metal");
    private LawOutputNode Wood => Laws.GetNode<LawOutputNode>("Wood");
    private LawOutputNode Water => Laws.GetNode<LawOutputNode>("Water");
    private LawOutputNode Fire => Laws.GetNode<LawOutputNode>("Fire");
    private LawOutputNode Earth => Laws.GetNode<LawOutputNode>("Earth");

    private VBoxContainer GeneralInfo => GetNode<VBoxContainer>("General Info");
    private LineEdit TotalLawLevel => GeneralInfo.GetNode<LineEdit>("Total Level/LineEdit");
    private LineEdit TotalHourly => GeneralInfo.GetNode<LineEdit>("Total Hourly/LineEdit");

    private VBoxContainer MajorThresholds => GetNode<VBoxContainer>("Major Thresholds");
    private LineEdit Threshold2000 => MajorThresholds.GetNode<LineEdit>("2000/LineEdit");
    private LineEdit Threshold4000 => MajorThresholds.GetNode<LineEdit>("4000/LineEdit");
    private LineEdit Threshold6000 => MajorThresholds.GetNode<LineEdit>("6000/LineEdit");
    private LineEdit Threshold8000 => MajorThresholds.GetNode<LineEdit>("8000/LineEdit");
    private LineEdit Threshold10000 => MajorThresholds.GetNode<LineEdit>("10000/LineEdit");



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
                _profile.Laws.Changed += Update;
            }
        }
    }

    private Laws Data => Profile.Laws;

    private void Update()
    {
        if (Data == null) return;

        GD.Print("LawCalculatorOutput Update() called");

        Metal.Laws = Data;
        Wood.Laws = Data;
        Water.Laws = Data;
        Fire.Laws = Data;
        Earth.Laws = Data;

        Metal.Data = Data.Metal;
        Wood.Data = Data.Wood;
        Water.Data = Data.Water;
        Fire.Data = Data.Fire;
        Earth.Data = Data.Earth;

        TotalLawLevel.Text = Data.TotalLawLevel.ToString("N0");
        TotalHourly.Text = Data.TotalXpPerHour.FormatLargeNumber();

        Threshold2000.Text = Scripts.LawSimulation.SimulateDaysToMajorThreshold(Data, 2000).ToString("N0");
        Threshold4000.Text = Scripts.LawSimulation.SimulateDaysToMajorThreshold(Data, 4000).ToString("N0");
        Threshold6000.Text = Scripts.LawSimulation.SimulateDaysToMajorThreshold(Data, 6000).ToString("N0");
        Threshold8000.Text = Scripts.LawSimulation.SimulateDaysToMajorThreshold(Data, 8000).ToString("N0");
        Threshold10000.Text = Scripts.LawSimulation.SimulateDaysToMajorThreshold(Data, 10000).ToString("N0");

    }
}

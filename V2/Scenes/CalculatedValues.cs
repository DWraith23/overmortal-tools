using Godot;
using OvermortalTools.V2.Resources;
using System;

public partial class CalculatedValues : VBoxContainer
{

    private LineEdit CosmoapsisOutput => GetNode<LineEdit>("Cosmoapsis/LineEdit");
    private LineEdit AvgMyrmValueOutput => GetNode<LineEdit>("Average Myrimon/LineEdit");
    private LineEdit AvgMyrmTechOutput => GetNode<LineEdit>("Myrimon Tech/LineEdit");

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

    private void Update()
    {
        GD.Print("CalculatedValues Update() called");

        var realm = Data.HighestRealm.Item1;
        var minor = Data.HighestRealm.Item2;

        CosmoapsisOutput.Text = Data.PassiveCultivation.GetCosmoapsisPerCycle((realm, minor)).ToString("N2");
        AvgMyrmValueOutput.Text = Data.MyrmimonData.GetAverageValue(realm).ToString("N0");
        AvgMyrmTechOutput.Text = Data.MyrmimonData.GetAverageTechPts().ToString("N0");
    }
}

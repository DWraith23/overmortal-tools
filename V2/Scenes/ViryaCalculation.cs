using Godot;
using OvermortalTools.V2.Resources;
using OvermortalTools.V2.Scripts;
using System;
using System.Linq;

namespace OvermortalTools.V2.Scenes;

public partial class ViryaCalculation : VBoxContainer
{

    private OptionButton ViryaSelect => GetNode<OptionButton>("Virya");
    private LineEdit MainPathOutput => GetNode<LineEdit>("Output/Main Path/LineEdit");
    private LineEdit AuxPathOutput => GetNode<LineEdit>("Output/Completion/LineEdit");
    private LineEdit MyrimonFactorOutput => GetNode<LineEdit>("Output/Myrimon/LineEdit");

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
        PopulateOptionButton();
        ConnectSignals();
    }

    private void Update()
    {
        if (Data == null) return;

        ViryaSelect.Select((int)Data.TargetVirya + 1);

        var mainPath = Data.GetPathsInOrder().First();

        var mainDays = CultivationTimeSimulation.CountDaysToTargetRealm(
            mainPath,
            Data.TotalDailyExp,
            mainPath.CurrentRealm,
            PathData.MinorRealm.Late
        );

        var completionDays = CultivationTimeSimulation.CalculateDaysToVirya(Data, Data.TargetVirya);

        MainPathOutput.Text = mainDays.ToString();
        AuxPathOutput.Text = completionDays == -1
            ? "N/A"
            : completionDays == -2
                ? "0"
                : completionDays.ToString();

        var myrmDays = CultivationTimeSimulation.CalculateDaysToVirya(Data, Data.TargetVirya, true);
        MyrimonFactorOutput.Text = myrmDays == -1
            ? "N/A"
            : myrmDays == -2
                ? "0"
                : myrmDays.ToString();
    }

    private void ConnectSignals()
    {
        _data.Changed += Update;

        ViryaSelect.ItemSelected += item =>
        {
            if (Data == null) return;
            Data.TargetVirya = (PathData.Virya)ViryaSelect.Selected - 1;

            Update();
        };
    }

    private void PopulateOptionButton()
    {
        foreach (var realm in PathData.ViryaNames)
        {
            ViryaSelect.AddItem(realm.Value, (int)realm.Key);
        }
    }
}

using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class LawCalculatorInput : VBoxContainer
{

    private SpinBox AverageBlitzHoursSpinBox => GetNode<SpinBox>("Blitz/Average Blitz Hours");
    private OptionButton FruitQualitySelect => GetNode<OptionButton>("Blitz/Fruit Quality");

    private CheckBox ShearsCheckBox => GetNode<CheckBox>("Blitz/Shears/CheckBox");
    private HBoxContainer ShearsStarsContainer => GetNode<HBoxContainer>("Blitz/Stars");
    private SpinBox ShearsStarsSpinBox => GetNode<SpinBox>("Blitz/Stars/SpinBox");

    private LawNode MetalNode => GetNode<LawNode>("Laws/Metal");
    private LawNode WoodNode => GetNode<LawNode>("Laws/Wood");
    private LawNode WaterNode => GetNode<LawNode>("Laws/Water");
    private LawNode FireNode => GetNode<LawNode>("Laws/Fire");
    private LawNode EarthNode => GetNode<LawNode>("Laws/Earth");

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

    private Laws Data => Profile.Laws;

    public override void _Ready()
    {
        PopulateOptionButton();
        ConnectSignals();
    }

    private void Update()
    {
        if (Data == null) return;

        GD.Print("LawCalculatorInput Update() called");

        AverageBlitzHoursSpinBox.SetValueNoSignal(Data.AverageBlitzHours);
        FruitQualitySelect.Select((int)Data.FruitQuality);

        ShearsCheckBox.ButtonPressed = Data.HasShears;
        ShearsStarsSpinBox.SetValueNoSignal(Data.ShearsStars);

        ShearsStarsContainer.Modulate = Data.HasShears ? Colors.White : Colors.Transparent;
        ShearsStarsSpinBox.Editable = Data.HasShears;

        MetalNode.Data = Data.Metal;
        WoodNode.Data = Data.Wood;
        WaterNode.Data = Data.Water;
        FireNode.Data = Data.Fire;
        EarthNode.Data = Data.Earth;
    }

    private void PopulateOptionButton()
    {
        foreach (var name in Enum.GetNames<Quality>())
        {
            if (name == "Common" || name == "Mythic") continue;
            FruitQualitySelect.AddItem(name);
        }
    }

    private void ConnectSignals()
    {
        _profile.Changed += Update;

        AverageBlitzHoursSpinBox.ValueChanged += value =>
        {
            Data.AverageBlitzHours = (int)value;
        };

        FruitQualitySelect.ItemSelected += idx =>
        {
            if (idx == 0) return;
            Data.FruitQuality = (Quality)idx;
        };

        ShearsCheckBox.Toggled += toggled =>
        {
            Data.HasShears = toggled;
        };

        ShearsStarsSpinBox.ValueChanged += value =>
        {
            Data.ShearsStars = (int)value;
        };
        
    }

    
}

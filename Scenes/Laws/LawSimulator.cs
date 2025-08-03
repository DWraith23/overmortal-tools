using Godot;
using OvermortalTools.Resources.Laws;
using OvermortalTools.Scripts;
using System.Collections.Generic;
using System.Linq;

namespace OvermortalTools.Scenes.Laws;

public partial class LawSimulator : Control
{
	[Signal] public delegate void RequestSaveEventHandler();

	private LawsData _data;
	public LawsData Data
	{
		get => _data;
		set
		{
			if (_data == value) return;
			_data = value;
			if (value != null) _data.Changed += Update;
			SetNodes();
			Update();
		}
	}

	private VBoxContainer Laws => GetNode<VBoxContainer>("Laws");
	private Law MetalLaw => Laws.GetNode<Law>("Metal");
	private LineEdit MetalHours => MetalLaw.GetNode<LineEdit>("Hours");
	private Law WoodLaw => Laws.GetNode<Law>("Wood");
	private LineEdit WoodHours => WoodLaw.GetNode<LineEdit>("Hours");
	private Law WaterLaw => Laws.GetNode<Law>("Water");
	private LineEdit WaterHours => WaterLaw.GetNode<LineEdit>("Hours");
	private Law FireLaw => Laws.GetNode<Law>("Fire");
	private LineEdit FireHours => FireLaw.GetNode<LineEdit>("Hours");
	private Law EarthLaw => Laws.GetNode<Law>("Earth");
	private LineEdit EarthHours => EarthLaw.GetNode<LineEdit>("Hours");

	private HBoxContainer Totals => Laws.GetNode<HBoxContainer>("Totals");
	private LineEdit TotalLevelDisplay => Totals.GetNode<LineEdit>("Total Level");
	private LineEdit TotalPointsPerHourDisplay => Totals.GetNode<LineEdit>("Points per Hour");

	private VBoxContainer SimulationContainer => GetNode<VBoxContainer>("Simulation");
	private LineEdit Level2000Display => SimulationContainer.GetNode<LineEdit>("Level 2000/Days");
	private LineEdit Level4000Display => SimulationContainer.GetNode<LineEdit>("Level 4000/Days");
	private LineEdit Level6000Display => SimulationContainer.GetNode<LineEdit>("Level 6000/Days");
	private LineEdit Level8000Display => SimulationContainer.GetNode<LineEdit>("Level 8000/Days");
	private LineEdit Level10000Display => SimulationContainer.GetNode<LineEdit>("Level 10000/Days");
	private SpinBox AverageBlitzBox => SimulationContainer.GetNode<SpinBox>("Blitz/Average Blitz Hours");
	private OptionButton FruitQualitySelect => SimulationContainer.GetNode<OptionButton>("Blitz/Fruit Quality");
	private HBoxContainer ShearsContainer => SimulationContainer.GetNode<HBoxContainer>("Blitz/Shears");
	private CheckBox ShearsCheck => ShearsContainer.GetNode<CheckBox>("CheckBox");
	private HBoxContainer StarsContainer => ShearsContainer.GetNode<HBoxContainer>("Stars");
	private SpinBox StarsBox => StarsContainer.GetNode<SpinBox>("SpinBox");

	private List<ElementalLaw> AllLaws =>
	[
		Data.Metal,
		Data.Wood,
		Data.Water,
		Data.Fire,
		Data.Earth
	];

	private int TotalLevel => AllLaws.Sum(law => law.Level);
	private long TotalPointsPerHour => AllLaws.Sum(law => law.PointsPerHour);

	public override void _Ready()
	{
		Data ??= new();
		ConnectSignals();
		Update();
	}

	private void ConnectSignals()
	{
		AverageBlitzBox.ValueChanged += OnAverageBlitzBoxChanged;
		FruitQualitySelect.ItemSelected += OnFruitQualitySelected;
		ShearsCheck.Toggled += OnShearsCheckToggled;
		StarsBox.ValueChanged += OnShearsStarsValueChanged;
	}

	private void SetNodes()
	{
		if (Data == null) return;

		MetalLaw.LawData = Data.Metal;
		WoodLaw.LawData = Data.Wood;
		WaterLaw.LawData = Data.Water;
		FireLaw.LawData = Data.Fire;
		EarthLaw.LawData = Data.Earth;

		AverageBlitzBox.Value = Data.AverageBlitzHours;
		FruitQualitySelect.Selected = (int)Data.FruitQuality;
		ShearsCheck.ButtonPressed = Data.HasShears;
		StarsBox.Value = Data.ShearsStars;
	}

	private void Update()
	{
		if (Data == null) return;

		TotalLevelDisplay.Text = TotalLevel.ToString("N0");
		TotalPointsPerHourDisplay.Text = TotalPointsPerHour.ToString("N0");

		MetalHours.Text = DaysToNextThreshold(Data.Metal).ToString("N0");
		WoodHours.Text = DaysToNextThreshold(Data.Wood).ToString("N0");
		WaterHours.Text = DaysToNextThreshold(Data.Water).ToString("N0");
		FireHours.Text = DaysToNextThreshold(Data.Fire).ToString("N0");
		EarthHours.Text = DaysToNextThreshold(Data.Earth).ToString("N0");

		Simulate();
		Tools.EmitLoggedSignal(this, SignalName.RequestSave);
	}

	private void OnAverageBlitzBoxChanged(double value)
	{
		if (Data == null) return;
		Data.AverageBlitzHours = (int)value;
	}

	private void OnFruitQualitySelected(long index)
	{
		if (Data == null) return;
		Data.FruitQuality = (LawsData.LawFruitQuality)index;
	}

	private void OnShearsCheckToggled(bool value)
	{
		if (Data == null) return;
		Data.HasShears = value;
		StarsContainer.Visible = value;

		if (!value)
		{
			StarsBox.Value = 0;
		}
	}
	private void OnShearsStarsValueChanged(double value)
	{
		if (Data == null) return;
		Data.ShearsStars = (int)value;
	}

	private int DaysToNextThreshold(ElementalLaw law)
	{
		if (law.Level < 1) return 1;
		var simulator = new LawSimulation(Data);

		return simulator.SimulateLawToNextThreshold(law);
	}

	private void Simulate()
	{
		var l2000 = GetDaysToLevel(2000);
		var l4000 = GetDaysToLevel(4000);
		var l6000 = GetDaysToLevel(6000);
		var l8000 = GetDaysToLevel(8000);
		var l10000 = GetDaysToLevel(10000);

		Level2000Display.Text = l2000 == -1 ? ">1 Year" : l2000.ToString("N0");
		Level4000Display.Text = l4000 == -1 ? ">1 Year" : l4000.ToString("N0");
		Level6000Display.Text = l6000 == -1 ? ">1 Year" : l6000.ToString("N0");
		Level8000Display.Text = l8000 == -1 ? ">1 Year" : l8000.ToString("N0");
		Level10000Display.Text = l10000 == -1 ? ">1 Year" : l10000.ToString("N0");

	}

	private int GetDaysToLevel(int level)
	{
		var simulator = new LawSimulation(Data);

		return simulator.SimulateToLevel(level);
	}

}

using Godot;
using OvermortalTools.Resources.Laws;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LawTesting : Control
{
	[Export] public ElementalLaw Metal { get; set; }
	[Export] public ElementalLaw Earth { get; set; }
	[Export] public ElementalLaw Water { get; set; }
	[Export] public ElementalLaw Fire { get; set; }
	[Export] public ElementalLaw Wood { get; set; }

	private VBoxContainer Laws => GetNode<VBoxContainer>("Panel/Contents/Laws");
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

	private VBoxContainer SimulationContainer => GetNode<VBoxContainer>("Panel/Contents/Simulation");
	private LineEdit Level2000Display => SimulationContainer.GetNode<LineEdit>("Level 2000/Days");
	private LineEdit Level4000Display => SimulationContainer.GetNode<LineEdit>("Level 4000/Days");
	private LineEdit Level6000Display => SimulationContainer.GetNode<LineEdit>("Level 6000/Days");
	private LineEdit Level8000Display => SimulationContainer.GetNode<LineEdit>("Level 8000/Days");
	private LineEdit Level10000Display => SimulationContainer.GetNode<LineEdit>("Level 10000/Days");
	private SpinBox AverageBlitzBox => SimulationContainer.GetNode<SpinBox>("Blitz/Average Blitz Hours");
	private OptionButton FruitQualitySelect => SimulationContainer.GetNode<OptionButton>("Blitz/Fruit Quality");
	private Button SimulateButton => SimulationContainer.GetNode<Button>("Button");



	private List<ElementalLaw> AllLaws =>
	[
		Metal,
		Wood,
		Water,
		Fire,
		Earth
	];

	private int TotalLevel => AllLaws.Sum(law => law.Level);
	private long TotalPointsPerHour => AllLaws.Sum(law => law.PointsPerHour);

	public override void _Ready()
	{
		MetalLaw.LawData = Metal;
		WoodLaw.LawData = Wood;
		WaterLaw.LawData = Water;
		FireLaw.LawData = Fire;
		EarthLaw.LawData = Earth;

		ConnectSignals();
		Update();
	}

	private void ConnectSignals()
	{
		Metal.Changed += Update;
		Wood.Changed += Update;
		Water.Changed += Update;
		Fire.Changed += Update;
		Earth.Changed += Update;

		SimulateButton.Pressed += Simulate;
	}

	private void Update()
	{
		if (AllLaws.Any(law => law == null) || TotalLevel < 5) return;

		TotalLevelDisplay.Text = TotalLevel.ToString("N0");
		TotalPointsPerHourDisplay.Text = TotalPointsPerHour.ToString("N0");

		MetalHours.Text = (Metal.XpRemaining / TotalPointsPerHour).ToString("N0");
		WoodHours.Text = (Wood.XpRemaining / TotalPointsPerHour).ToString("N0");
		WaterHours.Text = (Water.XpRemaining / TotalPointsPerHour).ToString("N0");
		FireHours.Text = (Fire.XpRemaining / TotalPointsPerHour).ToString("N0");
		EarthHours.Text = (Earth.XpRemaining / TotalPointsPerHour).ToString("N0");
	}

	private void Simulate()
	{
		var quality = (LawSimulation.LawFruitQuality)FruitQualitySelect.Selected;
		var simulator = new LawSimulation()
		{
			AverageBlitzHours = (int)AverageBlitzBox.Value,
			FruitQuality = quality
		};

		var l2000 = simulator.SimulateToLevel(2000, AllLaws);
		var l4000 = simulator.SimulateToLevel(4000, AllLaws);
		var l6000 = simulator.SimulateToLevel(6000, AllLaws);
		var l8000 = simulator.SimulateToLevel(8000, AllLaws);
		var l10000 = simulator.SimulateToLevel(9750, AllLaws);

		Level2000Display.Text = l2000 == -1 ? ">1 Year" : l2000.ToString("N0");
		Level4000Display.Text = l4000 == -1 ? ">1 Year" : l4000.ToString("N0");
		Level6000Display.Text = l6000 == -1 ? ">1 Year" : l6000.ToString("N0");
		Level8000Display.Text = l8000 == -1 ? ">1 Year" : l8000.ToString("N0");
		Level10000Display.Text = l10000 == -1 ? ">1 Year" : l10000.ToString("N0");

	}

}

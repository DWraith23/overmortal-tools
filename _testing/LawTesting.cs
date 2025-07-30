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

	private VBoxContainer Laws => GetNode<VBoxContainer>("PanelContainer/Laws");
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
	private LineEdit TotalLevelDisplay => GetNode<LineEdit>("PanelContainer/Laws/Totals/Total Level");
	private LineEdit TotalPointsPerHourDisplay => GetNode<LineEdit>("PanelContainer/Laws/Totals/Points per Hour");

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
	}

	private void Update()
	{
		if (AllLaws.Any(law => law == null)) return;

		TotalLevelDisplay.Text = TotalLevel.ToString("N0");
		TotalPointsPerHourDisplay.Text = TotalPointsPerHour.ToString("N0");

		MetalHours.Text = (Metal.XpRemaining / TotalPointsPerHour).ToString("N0");
		WoodHours.Text = (Wood.XpRemaining / TotalPointsPerHour).ToString("N0");
		WaterHours.Text = (Water.XpRemaining / TotalPointsPerHour).ToString("N0");
		FireHours.Text = (Fire.XpRemaining / TotalPointsPerHour).ToString("N0");
		EarthHours.Text = (Earth.XpRemaining / TotalPointsPerHour).ToString("N0");
	}



}

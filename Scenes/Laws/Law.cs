using Godot;
using OvermortalTools.Resources.Laws;
using System;

namespace OvermortalTools.Scenes.Laws;

public partial class Law : HBoxContainer
{
	private Label NameLabel => GetNode<Label>("Name");
	private SpinBox LevelBox => GetNode<SpinBox>("Level");
	private SpinBox BonusBox => GetNode<SpinBox>("Bonus");

	private LineEdit MultiplierDisplay => GetNode<LineEdit>("Multiplier");
	private LineEdit PointsPerHourDisplay => GetNode<LineEdit>("Points per Hour");
	private LineEdit PointsTowardDisplay => GetNode<LineEdit>("Points Toward");
	private LineEdit PointsNeededDisplay => GetNode<LineEdit>("Points Needed");

	private ElementalLaw _lawData;
	public ElementalLaw LawData
	{
		get => _lawData;
		set
		{
			if (_lawData == value) return;
			_lawData = value;
			Update();
			_lawData.Changed += Update;
		}
	}

	private void Update()
	{
		if (LawData == null) return;
		NameLabel.Text = LawData.Name;
		LevelBox.Value = LawData.Level;
		BonusBox.Value = LawData.Bonus * 100f;
		MultiplierDisplay.Text = LawData.Multiplier.ToString("N0");
		PointsPerHourDisplay.Text = LawData.PointsPerHour.ToString("N0");
		PointsTowardDisplay.Text = LawData.XpTowards.ToString("N0");
		PointsNeededDisplay.Text = LawData.XpRemaining.ToString("N0");
	}

	public override void _Ready()
	{
		LevelBox.ValueChanged += OnLevelSpinboxChanged;
		BonusBox.ValueChanged += OnBonusSpinboxChanged;
		Update();
	}

	private void OnLevelSpinboxChanged(double value) =>
		LawData.Level = (int)value;

	private void OnBonusSpinboxChanged(double value) =>
		LawData.Bonus = (float)value / 100f;


}

using Godot;
using OvermortalTools.Resources.Laws;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes.Laws;

public partial class Law : HBoxContainer
{
	[Signal] public delegate void ChangedEventHandler();

	private Label NameLabel => GetNode<Label>("Name");
	private SpinBox LevelBox => GetNode<SpinBox>("Level");
	private SpinBox BonusBox => GetNode<SpinBox>("Bonus");

	private LineEdit PointsPerHourDisplay => GetNode<LineEdit>("Points per Hour");

	private ElementalLaw _lawData;
	public ElementalLaw LawData
	{
		get => _lawData;
		set
		{
			if (_lawData == value) return;
			_lawData = value;
			Update();
			if (_lawData == null) return;
			_lawData.Changed += Update;
		}
	}

	private void Update()
	{
		if (LawData == null) return;
		NameLabel.Text = LawData.Name;
		LevelBox.Value = LawData.Level;
		BonusBox.Value = LawData.Bonus * 100f;
		PointsPerHourDisplay.Text = FormatLargeNumber(LawData.PointsPerHour);
		Tools.EmitLoggedSignal(this, SignalName.Changed);
	}

	private static string FormatLargeNumber(long number)
	{
		if (number < 1000) return number.ToString("N2");
		if (number < 10000) return number.ToString("N0");
		if (number < 1000000) return $"{number / 1000f:N2}K";
		if (number < 1_000_000_000) return $"{number / 1_000_000f:N2}M";
		if (number < 1_000_000_000_000L) return $"{number / 1_000_000_000d:N2}B";
		if (number < 1_000_000_000_000_000L) return $"{number / 1_000_000_000_000d:N2}T";
		return $"{number / 1_000_000_000_000_000d:N2}Q";
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

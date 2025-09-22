using Godot;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scenes.Components;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes.Planner;

[GlobalClass, Tool]
public partial class StarMarks : VBoxContainer
{
	[Signal] public delegate void ValuesChangedEventHandler();

	private StarMarksData _data = new();
	public StarMarksData Data
	{
		get => _data;
		set
		{
			if (_data == value) return;
			_data = value;
			Update();
			UpdateSpinboxes();
			if (value != null)
			{
				_data.Changed += Update;
			}
		}
	}

	private void UpdateSpinboxes()
	{
		if (Data == null) return;
		RarePills.SpinBox.Value = Data.RarePills;
		EpicPills.SpinBox.Value = Data.EpicPills;
		LegendaryPills.SpinBox.Value = Data.LegendaryPills;
		RespiraExp.SpinBox.Value = Data.RespiraExp;
	}

	private LabeledSpinbox RarePills => GetNode<LabeledSpinbox>("Rare Pills");
	private LabeledSpinbox EpicPills => GetNode<LabeledSpinbox>("Epic Pills");
	private LabeledSpinbox LegendaryPills => GetNode<LabeledSpinbox>("Legendary Pills");
	private LabeledSpinbox RespiraExp => GetNode<LabeledSpinbox>("Respira");

	private void Update()
	{

		Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);  
	}

	public override void _Ready()
	{
		_data.Changed += () => Tools.EmitLoggedSignal(this, SignalName.ValuesChanged);

		SetSignals();
	}

	private void SetSignals()
	{
		RarePills.SpinBox.ValueChanged += OnRarePillsSpinBoxChanged;
		EpicPills.SpinBox.ValueChanged += OnEpicPillsSpinBoxChanged;
		LegendaryPills.SpinBox.ValueChanged += OnLegendaryPillsSpinBoxChanged;
		RespiraExp.SpinBox.ValueChanged += OnRespiraExpSpinBoxChanged;
	}


	private void OnRarePillsSpinBoxChanged(double value)
	{
		if (Data == null) return;
		Data.RarePills = (float)value;
	}

	private void OnEpicPillsSpinBoxChanged(double value)
	{
		if (Data == null) return;
		Data.EpicPills = (float)value;
	}

	private void OnLegendaryPillsSpinBoxChanged(double value)
	{
		if (Data == null) return;
		Data.LegendaryPills = (float)value;
	}
	
	private void OnRespiraExpSpinBoxChanged(double value)
	{
		if (Data == null) return;
		Data.RespiraExp = (float)value;
	}
}

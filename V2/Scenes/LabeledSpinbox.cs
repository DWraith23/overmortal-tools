using Godot;
using System;

namespace OvermortalTools.V2.Scenes;

[GlobalClass, Tool]
public partial class LabeledSpinbox : HBoxContainer
{
	[Signal] public delegate void ValueChangedEventHandler(double value);

	public Label Label => GetNode<Label>("Label");
	public SpinBox SpinBox => GetNode<SpinBox>("SpinBox");
	public QuestionButton Tooltip => GetNode<QuestionButton>("Question Button");

	// Label properties
	private float _labelWidth = 0f;
	private string _labelText = "[LABEL]";
	// SpinBox properties
	private float _value = 0f;
	private float _minValue = 0f;
	private float _maxValue = 100f;
	private float _step = 1f;
	private string _prefix = "";
	private string _suffix = "";
	private bool _rounded = false;
	// Tooltip properties
	private string _tooltipText = "";
	private bool _tooltipVisible = false;

	[ExportCategory("Label")]
	[Export]
	public float LabelWidth
	{
		get => _labelWidth;
		set
		{
			if (_labelWidth == value) return;
			_labelWidth = value;
			if (Label != null)
			{
				if (value <= 0)
					Label.CustomMinimumSize = new Vector2I(0, 0);
				else
					Label.CustomMinimumSize = new Vector2I((int)value, 0);
			}
		}
	}
	[Export]
	public string LabelText
	{
		get => _labelText;
		set
		{
			if (_labelText == value) return;
			_labelText = value;
			if (Label != null)
			{
				Label.Text = value;
			}
		}
	}

	[ExportCategory("SpinBox")]
	[Export]
	public float Value
	{
		get => _value;
		set
		{
			if (_value == value) return;
			_value = value;
			if (SpinBox != null)
			{
				SpinBox.Value = value;
			}
		}
	}

	[Export]
	public float MinValue
	{
		get => _minValue;
		set
		{
			if (_minValue == value) return;
			_minValue = value;
			if (SpinBox != null)
			{
				SpinBox.MinValue = value;
			}
		}
	}

	[Export]
	public float MaxValue
	{
		get => _maxValue;
		set
		{
			if (_maxValue == value) return;
			_maxValue = value;
			if (SpinBox != null)
			{
				SpinBox.MaxValue = value;
			}
		}
	}

	[Export]
	public float Step
	{
		get => _step;
		set
		{
			if (_step == value) return;
			_step = value;
			if (SpinBox != null)
			{
				SpinBox.Step = value;
			}
		}
	}

	[Export]
	public string Prefix
	{
		get => _prefix;
		set
		{
			if (_prefix == value) return;
			_prefix = value;
			if (SpinBox != null)
			{
				SpinBox.Prefix = value;
			}
		}
	}

	[Export]
	public string Suffix
	{
		get => _suffix;
		set
		{
			if (_suffix == value) return;
			_suffix = value;
			if (SpinBox != null)
			{
				SpinBox.Suffix = value;
			}
		}
	}

	[Export]
	public bool Rounded
	{
		get => _rounded;
		set
		{
			if (_rounded == value) return;
			_rounded = value;
			if (SpinBox != null)
			{
				SpinBox.Rounded = value;
			}
		}
	}

	[ExportCategory("Tooltip")]
	[Export]
	public new string TooltipText
	{
		get => _tooltipText;
		set
		{
			if (_tooltipText == value) return;
			_tooltipText = value;
			if (Tooltip != null)
			{
				Tooltip.PopupText = value;
			}
		}
	}

	[Export]
	public bool TooltipVisible
	{
		get => _tooltipVisible;
		set
		{
			if (_tooltipVisible == value) return;
			_tooltipVisible = value;
			if (Tooltip != null)
			{
				Tooltip.Visible = value;
			}
		}
	}


	public override void _Ready()
	{
		Label.Text = _labelText;
		SpinBox.Value = _value;
		SpinBox.MinValue = _minValue;
		SpinBox.MaxValue = _maxValue;
		SpinBox.Step = _step;
		SpinBox.Prefix = _prefix;
		SpinBox.Suffix = _suffix;
		SpinBox.Rounded = _rounded;
		Tooltip.PopupText = _tooltipText;
		Tooltip.Visible = _tooltipVisible;

		SpinBox.ValueChanged += (value) => EmitSignal(SignalName.ValueChanged, value);
	}

    public void SetValueNoSignal(double value) => SpinBox?.SetValueNoSignal(value);
}

using System.Collections.Generic;
using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class PassiveCultivationData : Resource
{
    /// <summary>
    /// Dictionary mapping the AuraGemIndex value to the mulitiplier for Cosmoapsis.
    /// </summary>
    private static Dictionary<int, float> AuraGemValues => new()
    {
        { -1, 0f },
        { 0, 0f },
        { 1, 0.1f },
        { 2, 0.13f },
        { 3, 0.16f },
        { 4, 0.2f },
        { 5, 0.24f },
        { 6, 0.28f },
    };

    private float _cosmoapsis = 1.0f;
    private int _auraGemIndex = 0;

    [Export]
    public float Cosmoapsis
    {
        get => _cosmoapsis;
        set
        {
            _cosmoapsis = value;
            EmitChanged();
        }
    }

    [Export]
    public int AuraGemIndex
    {
        get => _auraGemIndex;
        set
        {
            _auraGemIndex = value;
            EmitChanged();
        }
    }
    
    private float AuraGemMultiplier => AuraGemValues[AuraGemIndex];
    public float CosmoPerSecond => Cosmoapsis / 8f * (AuraGemMultiplier + 1f);
    public float CosmoPerMinute => CosmoPerSecond * 60f;
    public float CosmoPerHour => CosmoPerMinute * 60f;
    public float CosmoPerDay => CosmoPerHour * 24f;

}

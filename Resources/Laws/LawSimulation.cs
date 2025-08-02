using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace OvermortalTools.Resources.Laws;

public class LawSimulation
{
    private static Dictionary<LawsData.LawFruitQuality, int> FruitHours => new()
    {
        { LawsData.LawFruitQuality.Green, 1 },
        { LawsData.LawFruitQuality.Blue, 3 },
        { LawsData.LawFruitQuality.Purple, 6 },
        { LawsData.LawFruitQuality.Gold, 12 },
    };

    // Public set properties
    public LawsData Data { get; set; }

    private List<ElementalLaw> Laws =>
    [
        Data.Metal,
        Data.Wood,
        Data.Water,
        Data.Fire,
        Data.Earth
    ];

    private int AverageBlitzHours => Data.AverageBlitzHours;
    private LawsData.LawFruitQuality FruitQuality => Data.FruitQuality;

    private bool HasShears => Data.HasShears;
    private int ShearsStars => Data.ShearsStars;


    // Private fields
    private int TotalLevel => Laws.Sum(law => law.Level);
    private long PointsPerHour => Laws.Sum(law => law.PointsPerHour);

    public LawSimulation(LawsData data)
    {
        Data = data.Duplicate(true) as LawsData;
    }

    private float ShearsHours => (RechargeValues[ShearsStars] * 96f + 100) / 100f * 14;

    private static Dictionary<int, float> RechargeValues => new()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
        { 3, 2f },
        { 4, 2.4f },
        { 5, 3f }
    };

    private ElementalLaw GetBestLawToLevel()
    {
        var max = Laws.MaxBy(law => law.Level);
        var min = Laws.MinBy(law => law.Level);

        var validChoices = Laws.Where(law => law.Level < 2000);

        // Get all laws to 50 first
        if (Laws.Any(law => law.Level < 50))
        {
            return Laws.Where(law => law.Level < 50).MaxBy(law => law.Level);
        }


        var currentTarget = min.NextThreshold;
        var optimalChoices = validChoices.Where(law => law.Level < currentTarget);
        // GD.Print($"Current Target: {currentTarget} | Optimal: {string.Join(", ", optimalChoices.Select(law => law.Name))}");
        if (optimalChoices.Any())
        {
            return optimalChoices.MaxBy(law => law.Level);
        }

        return validChoices.MaxBy(law => law.Level);
    }


    // Simulation
    public int SimulateToLevel(int level)
    {
        if (TotalLevel >= 10000) return 0;
        int days = 0;
        while (TotalLevel < level)
        {
            // GD.Print($"--- NEW DAY: DAY {days} ---");
            SimulateDay();
            // GD.Print($"Law Levels: {TotalLevel} - {PointsPerHour} - {AverageBlitzHours}");
            // Laws.ForEach(law => GD.Print($"|     {law}"));
            days++;
            if (days > 364) return -1;
        }
        return days;
    }

    public int SimulateLawToNextThreshold(ElementalLaw law)
    {
        if (law.Level >= 2000) return 0;
        law = law.Duplicate(true) as ElementalLaw;
        if (law.Level == 1) return 1;
        int threshold = law.NextThreshold;
        int days = 0;
        int n = 0;
        while (law.Level < threshold)
        {
            if (law.Level >= 2000) return days;
            n++;
            days++;
            SimulateDay(law);
            if (days > 364) return -1;
            if (n > 364)
            {
                GD.PrintErr($"ERROR: Too many days. n={n}, days={days}");
                break;
            }
        }

        return days;
    }

    private void SimulateDay()
    {
        int hours = AverageBlitzHours;
        var dailyXp = PointsPerHour * 24;
        var fruitHours = FruitHours[FruitQuality];
        var fruit = (int)Math.Floor(hours / (float)fruitHours);
        var leftover = hours % fruitHours;

        if (Laws.Average(law => law.Level) != 1)
        {
            var dailyLaw = GetBestLawToLevel();
            dailyLaw?.AddXp(dailyXp);
        }

        if (Data.HasShears) GetBestLawToLevel()?.AddXp((long)Math.Floor(ShearsHours * PointsPerHour));

        while (fruit > 0)
        {
            var law = GetBestLawToLevel();
            if (law == null) break;
            var xp = (long)Math.Floor(fruitHours * PointsPerHour * 0.95f); // Reducing some due to waste.
            // GD.Print($"Adding {xp} ({fruitHours} * {PointsPerHour}) to {law.Name}. {FruitQuality} fruit remaining: {fruit - 1}");
            law.AddXp(xp);
            fruit--;
        }
        for (int i = 0; i < leftover; i++)
        {
            GetBestLawToLevel()?.AddXp(PointsPerHour);
        }
    }

    private void SimulateDay(ElementalLaw law)
    {
        var dailyXp = PointsPerHour * 24;
        var hours = AverageBlitzHours;
        var fruitHours = FruitHours[FruitQuality];
        var fruit = (int)Math.Floor(hours / (float)fruitHours);
        var leftover = hours % fruitHours;

        if (Laws.Average(law => law.Level) != 1) law.AddXp(dailyXp);

        if (Data.HasShears)
        {
            law?.AddXp((long)Math.Floor(ShearsHours * PointsPerHour));
            GD.Print($"ShearsHours: {ShearsHours} | ShearsStars: {ShearsStars} | Extra Points: {ShearsHours * PointsPerHour}");
        }

        int n = 0;
        while (fruit > 0)
        {
            n++;
            var xp = (long)Math.Floor(fruitHours * PointsPerHour * 0.95f); // Reducing some due to waste.
            law.AddXp(xp);
            fruit--;
            if (n > 150)
            {
                GD.PrintErr($"ERROR: Too many loops.  n={n}, fruit={fruit}");
                break;
            }
        }
        for (int i = 0; i < leftover; i++)
        {
            law.AddXp(PointsPerHour);
        }
    }
}

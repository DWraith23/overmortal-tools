using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace OvermortalTools.Resources.Laws;

public class LawSimulation
{
    public enum LawFruitQuality
    {
        Green,
        Blue,
        Purple,
        Gold
    }

    private static Dictionary<LawFruitQuality, int> FruitHours => new()
    {
        { LawFruitQuality.Green, 1 },
        { LawFruitQuality.Blue, 3 },
        { LawFruitQuality.Purple, 6 },
        { LawFruitQuality.Gold, 12 },
    };

    // Public set properties
    public List<ElementalLaw> Laws { get; set; } = [];
    public int AverageBlitzHours { get; set; } = 120;
    public LawFruitQuality FruitQuality { get; set; } = LawFruitQuality.Gold;


    // Private fields
    private int TotalLevel => Laws.Sum(law => law.Level);
    private long PointsPerHour => Laws.Sum(law => law.PointsPerHour);


    // Private methods
    private float GetHoursPerLevel(ElementalLaw law) => law.NextLevelXp / (float)PointsPerHour;

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

        // If all laws are equal, choose the one with the highest bonus
        // if (Laws.All(law => law.Level == Laws[0].Level))
        // {
        //     return Laws.MaxBy(law => law.Bonus);
        // }   // Else return highest valid choice
        // else
        // {
        //     return validChoices.MaxBy(law => law.Level);
        // }

    }


    // Simulation
    public int SimulateToLevel(int level)
    {
        int days = 0;
        while (TotalLevel < level)
        {
            GD.Print($"--- NEW DAY: DAY {days} ---");
            SimulateDay();
            GD.Print($"Law Levels: {TotalLevel} - {PointsPerHour} - {AverageBlitzHours}");
            Laws.ForEach(law => GD.Print($"|     {law}"));
            days++;
            if (days > 364) return -1;
        }
        return days;
    }

    public int SimulateLawToNextThreshold(ElementalLaw law)
    {
        law = law.Duplicate(true) as ElementalLaw;
        if (law.Level == 1) return 1;
        int threshold = law.NextThreshold;
        int days = 0;
        int n = 0;
        while (law.Level < threshold)
        {
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

    public void DuplicateLaws(List<ElementalLaw> laws)
    {
        var result = new List<ElementalLaw>();
        foreach (var law in laws)
        {
            result.Add(law.Duplicate(true) as ElementalLaw);
        }
        Laws = result;
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

        while (fruit > 0)
        {
            var law = GetBestLawToLevel();
            if (law == null) break;
            var xp = (long)Math.Floor(fruitHours * PointsPerHour * 0.95f); // Reducing some due to waste.
            GD.Print($"Adding {xp} ({fruitHours} * {PointsPerHour}) to {law.Name}. {FruitQuality} fruit remaining: {fruit - 1}");
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

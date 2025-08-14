using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

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
            SimulateDay();
            days++;
            if (days > 364)
            {
                GD.PrintErr($"ERROR [SimulateToLevel(int level)]: Too many days. days={days}");
                return -1;
            }
        }
        return days;
    }

    /// <summary>
    /// Simulates the days and hours needed for a law to reach its next threshold.
    /// </summary>
    /// <param name="law"></param>
    /// <returns>A tuple containing (days, hours)</returns>
    public (int, int) SimulateLawHoursNeeded(ElementalLaw law)
    {
        if (law.Level >= 2000) return (0, 0);
        law = law.Duplicate(true) as ElementalLaw;
        int threshold = law.NextThreshold;
        GD.Print($"Simulating {law.Name} to threshold {threshold} (Level {law.Level})");
        return SimulateHourlyGrowth(law, threshold);
    }

    private (int, int) SimulateHourlyGrowth(ElementalLaw law, int threshold)
    {
        var dailyXp = PointsPerHour * 24;
        var fruitHours = FruitHours[FruitQuality];
        var dailyHours = AverageBlitzHours;
        var fruit = (int)Math.Floor(dailyHours / (float)fruitHours);
        var leftover = dailyHours % fruitHours;

        int hours = 0;

        while (law.Level < threshold)
        {
            if (hours > 9999)
            {
                GD.PrintErr($"ERROR [SimulateHourlyGrowth(ElementalLaw law, int threshold)]: Too many hours. law={law.Name}, hours={hours}, threshold={threshold}");
                return (-1, -1);
            }
            if (law.Level >= 1950)
            {
                GD.Print($"{law.Name} is closing in on 2000, documenting simulation.");
                GD.Print($"|    Current Level: {law.Level}, Next Threshold: {threshold}");
                GD.Print($"|    Current Points Per Hour: {PointsPerHour:N0}, Hours Passed: {hours}");
                GD.Print($"|    Daily XP: {dailyXp:N0}, Fruit Hours: {fruitHours}, Fruit Count: {fruit}, Leftover Hours: {leftover}");
                GD.Print($"|    Shears Hours: {ShearsHours:N0}, Shears Stars: {ShearsStars}");
                GD.Print($"|    XP Remaining: {law.XpRemaining:N0}, Next Level XP: {law.NextLevelXp:N0}");
                GD.Print($"|    Hours Estimated: {law.XpRemaining / PointsPerHour:N0}");
            }

            if (hours % AverageBlitzHours == 0 || hours == 0)
            {
                law.AddXp(dailyXp);
                if (law.Level >= 1950) GD.Print($"| |   Adding {dailyXp:N0} XP from Daily Accumulation");
                if (Data.HasShears)
                {
                    law.AddXp((long)Math.Floor(ShearsHours * PointsPerHour));
                    if (law.Level >= 1950) GD.Print($"| |   Adding {ShearsHours * PointsPerHour:N0} XP from Shears");
                }
            }

            if (law.Level >= threshold || law.Level >= 2000) break;

            var xp = PointsPerHour * fruitHours;
            law.AddXp(xp);
            fruit--;
            hours += fruitHours;
            if (law.Level >= 1950) GD.Print($"| |   Adding {xp:N0} XP from Fruit");

            if (law.Level >= threshold || law.Level >= 2000) break;

            if (fruit == 0)
            {
                fruit = (int)Math.Floor(dailyHours / (float)fruitHours);
                hours += leftover;
                xp = leftover * PointsPerHour;
                law.AddXp(xp);
                if (law.Level >= 1950) GD.Print($"| |   Adding {xp:N0} XP from Leftover Hours");
            }
            if (law.Level >= 1950) GD.Print($"| |   Hours: {hours}, Fruit: {fruit}");
        }

        var days = (int)Math.Floor((double)hours / AverageBlitzHours).RoundDown();
        var hoursRem = hours % AverageBlitzHours;
        return (days, hoursRem);
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

        var n = 0;
        while (fruit > 0)
        {
            n++;
            if (n > 136)
            {
                GD.PrintErr($"ERROR [SimulateDay()]: Too many loops. n={n}, fruit={fruit}");
                break;
            }
            var law = GetBestLawToLevel();
            if (law == null) break;
            var xp = (long)Math.Floor(fruitHours * PointsPerHour * 0.95f); // Reducing some due to waste.
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

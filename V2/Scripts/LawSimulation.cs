using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using OvermortalTools.Scripts;
using OvermortalTools.V2.Resources;

namespace OvermortalTools.V2.Scripts;

public static class LawSimulation
{
    private static Dictionary<Quality, int> FruitHours => new()
    {
        { Quality.Uncommon, 1 },
        { Quality.Rare, 3 },
        { Quality.Epic, 6 },
        { Quality.Legendary, 12 },
    };

    public static int SimulateHoursToNextThreshold(Laws allLaws, LawData law)
    {
        if (law.Level >= 2000) return 0;

        var otherLawsXp = allLaws.TotalXpPerHour - law.PointsPerHour;
        var targetClone = law.Duplicate(true) as LawData;

        var target = targetClone.NextMultiplierThreshold;

        var hours = 0;

        Tools.LogSignals = false;
        while (targetClone.Level < target)
        {
            if (hours >= 9999) break;

            var xp = otherLawsXp + (targetClone.PointsPerHour * FruitHours[allLaws.FruitQuality]);
            targetClone.AddXp(xp);
            hours += FruitHours[allLaws.FruitQuality];
        }
        Tools.LogSignals = true;

        return hours;
    }

    public static int SimulateDaysToMajorThreshold(Laws allLaws, int threshold)
    {
        if (allLaws.TotalLawLevel >= 10000 || allLaws.TotalLawLevel >= threshold) return 0;

        var clone = allLaws.Duplicate(true) as Laws;
        var dailyHours = clone.AverageBlitzHours;
        var fruitHours = FruitHours[clone.FruitQuality];
        var leftover = dailyHours % fruitHours;

        var days = 0;


        Tools.LogSignals = false;
        while (clone.TotalLawLevel < threshold)
        {
            if (days > 364) break;
            var fruit = (int)Math.Floor(dailyHours / (float)fruitHours);
            LawData law;

            while (fruit > 0)
            {
                law = ChooseLaw(clone);
                var cLevel = law.Level;
                law.AddXp(fruitHours * clone.TotalXpPerHour);
                fruit--;
            }

            law = ChooseLaw(clone);
            law.AddXp(leftover * law.PointsPerHour);

            law = ChooseLaw(clone);
            law.AddXp((long)Math.Floor(clone.ShearsHours * law.PointsPerHour));

            days++;
        }
        Tools.LogSignals = true;

        return days;
    }

    private static LawData ChooseLaw(Laws laws)
    {
        var threshold = laws.AllLaws
            .MinBy(law => law.Level)
            .NextMultiplierThreshold;

        var chosen = laws.AllLaws
            .Where(law => law.Level < threshold)
            .OrderByDescending(law => law.Level)
            .ThenByDescending(law => law.Bonus)
            .FirstOrDefault();

        if (chosen == null) return laws.AllLaws.MaxBy(law => law.Bonus);

        return chosen;
    }
}

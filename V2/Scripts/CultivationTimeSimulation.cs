using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;
using OvermortalTools.V2.Resources;

namespace OvermortalTools.V2.Scripts;

public static class CultivationTimeSimulation
{

    public static int CountDaysToTargetRealm(PathData data, long dailyExp, PathData.Realm targetRealm, PathData.MinorRealm targetMinorRealm)
    {
        var clone = data.Duplicate(true) as PathData ?? throw new Exception("Failed to clone PathData.");

        if (data.CurrentRealm < PathData.Realm.Connection) return -1; // Simulation only works from Connection and above

        int days = 0;
        long currentXp = (long)Math.Floor(clone.CurrentRealmProgress * PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm));

        Tools.LogSignals = false;
        while (true)
        {
            if (clone.CurrentRealm >= targetRealm && clone.CurrentMinorRealm >= targetMinorRealm && clone.CurrentRealmProgress >= 1f)
                break;

            currentXp += dailyExp;
            clone.CurrentRealmProgress = currentXp / (float)PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm);
            if (clone.CurrentRealmProgress >= 1f)
            {
                if (clone.CurrentRealm == targetRealm && clone.CurrentMinorRealm == targetMinorRealm)
                {
                    days++;
                    break;
                }

                currentXp = (long)Math.Floor((clone.CurrentRealmProgress - 1f) * PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm));
                AdvanceMinorRealm(clone);

                clone.CurrentRealmProgress = currentXp / (float)PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm);
            }

            // if (days % 10 == 0)
            // GD.Print($"Day {days + 1}: {clone.CurrentRealm} {clone.CurrentMinorRealm} - {clone.CurrentRealmProgress:P2} ({currentXp:N0} XP)");

            days++;
            if (days > 999) break; // Prevent infinite loop
        }
        Tools.LogSignals = true;

        return days;
    }

    public static int CountDaysToTargetRealm(PathData data, long dailyExp, PathData.Realm targetRealm, PathData.MinorRealm targetMinorRealm, long myrimonXp)
    {
        var clone = data.Duplicate(true) as PathData ?? throw new Exception("Failed to clone PathData.");

        if (data.CurrentRealm < PathData.Realm.Connection) return -1; // Simulation only works from Connection and above

        int days = 0;
        long currentXp = (long)Math.Floor(clone.CurrentRealmProgress * PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm)) + myrimonXp;


        Tools.LogSignals = false;
        while (true)
        {
            if (clone.CurrentRealm >= targetRealm && clone.CurrentMinorRealm >= targetMinorRealm && clone.CurrentRealmProgress >= 1f)
                break;

            currentXp += dailyExp;
            clone.CurrentRealmProgress = currentXp / (float)PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm);
            if (clone.CurrentRealmProgress >= 1f)
            {
                if (clone.CurrentRealm == targetRealm && clone.CurrentMinorRealm == targetMinorRealm)
                {
                    days++;
                    break;
                }

                currentXp = (long)Math.Floor((clone.CurrentRealmProgress - 1f) * PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm));
                AdvanceMinorRealm(clone);

                clone.CurrentRealmProgress = currentXp / (float)PathData.GetRealmExpReq(clone.CurrentRealm, clone.CurrentMinorRealm);
            }

            // if (days % 10 == 0)
            // GD.Print($"Day {days + 1}: {clone.CurrentRealm} {clone.CurrentMinorRealm} - {clone.CurrentRealmProgress:P2} ({currentXp:N0} XP)");

            days++;
            if (days > 999) break; // Prevent infinite loop
        }
        Tools.LogSignals = true;

        return days;
    }

    private static void AdvanceMinorRealm(PathData data)
    {
        if (data.CurrentMinorRealm == PathData.MinorRealm.Late)
        {
            data.CurrentMinorRealm = PathData.MinorRealm.Early;
            data.CurrentRealm += 1;
        }
        else
        {
            data.CurrentMinorRealm += 1;
        }
    }

    public static int CalculateDaysToVirya(ProfileData data, PathData.Virya virya, bool usingMyrm = false)
    {
        var clone = data.Duplicate(true) as ProfileData;
        int days = 0;

        var paths = clone.GetPathsInOrder();
        if (!paths.Any()) return -1;    // No paths, no virya.
        var main = paths.First();
        if (main.CurrentRealm < PathData.Realm.Incarnation) return -1;  // Below Incarnation there is no virya
        if (paths.Count() < 2) return -1;   // Can't virya with only 1 path
        var aux = paths.ToList()[1];
        if (aux.CurrentRealm <= PathData.Realm.Novice) return -1; // Set path to Novice at least to Virya.

        if (CheckViryaStatus(clone, virya)) return -2;  // Already at that virya, you silly goose.

        Tools.LogSignals = false;
        while (days < 999)
        {
            var path = CheckIfPathAtCompletion(main) ? aux : main;
            if (usingMyrm)
            {
                if (days % 7 == 0 && days != 0)
                {
                    var fruitPacks = clone.MyrmimonData.BuysFruitPacks ? 6 : 0;
                    var tokenPacks = clone.MyrmimonData.BuysTokenPacks ? 6 : 0;
                    clone.MyrmimonData.CurrentFruitQuantity += 9 + fruitPacks + tokenPacks;
                    GD.Print($"|    Day {days}: Gained {9 + fruitPacks + tokenPacks} fruits, now have {clone.MyrmimonData.CurrentFruitQuantity} fruits.");
                }
                var remainder = path.GetXpToTargetRealm(main.CurrentRealm, PathData.MinorRealm.Late);
                var myrmValue = clone.MyrmimonData.GetAverageValue(main.CurrentRealm);
                if (myrmValue >= remainder)
                {
                    GD.Print($"|    Using {myrmValue} XP from Myrmimon to finish {path.SelectedPath} (remaining: {remainder}).");
                    while (clone.MyrmimonData.CurrentFruitQuantity > 0 && !CheckIfPathAtCompletion(path))
                    {
                        var singleFruit = clone.MyrmimonData.GetSingleFruitValue(main.CurrentRealm);
                        GenericAddXpToPath(path, singleFruit);
                        clone.MyrmimonData.CurrentFruitQuantity--;
                    }
                }
            }
            if (days % 10 == 0)
            {
                GD.Print($"Day {days}: {path.SelectedPath} {path.CurrentRealm} {path.CurrentMinorRealm} - {path.CurrentRealmProgress:P2}");
            }
            CultivatePath(clone, path);

            days++;

            if (main.CurrentMinorRealm >= PathData.MinorRealm.Late && main.CurrentRealmProgress < 1f)
                clone.PassiveCultivation.ViryaPercent = 0f;

            if (CheckViryaStatus(clone, PathData.Virya.Eminence) || CheckViryaStatus(clone, PathData.Virya.Perfection))
                clone.PassiveCultivation.ViryaPercent = 0.2f;

            if (CheckViryaStatus(clone, virya)) break;

            if (path.CurrentRealmProgress >= 1f && path != main)
            {
                AdvanceMinorRealm(path);
            }
        }
        Tools.LogSignals = true;

        return days;
    }

    private static bool CheckViryaStatus(ProfileData data, PathData.Virya virya)
    {
        if (virya == PathData.Virya.None) return true;

        var paths = data.GetPathsInOrder().ToList();
        var main = paths.ElementAt(0);
        var aux = paths.ElementAt(1);

        var mainIndex = (int)main.CurrentRealm;
        var auxIndex = (int)aux.CurrentRealm;

        if (main.CurrentMinorRealm < PathData.MinorRealm.Late || main.CurrentRealmProgress < 1f) return false;

        if (virya == PathData.Virya.Completion)
        {
            if (CheckIfPathAtCompletion(main)) return true;
            return false;
        }
        else if (virya == PathData.Virya.Eminence)
        {
            // GD.Print($"|    Eminence check: auxIndex: {auxIndex} mainIndex: {mainIndex} - {auxIndex >= mainIndex} | {aux.CurrentMinorRealm >= PathData.MinorRealm.Middle}");
            if (auxIndex >= mainIndex) return true;
            if (auxIndex == mainIndex - 1 && aux.CurrentMinorRealm >= PathData.MinorRealm.Middle) return true;
            return false;
        }
        else if (virya == PathData.Virya.Perfection)
        {
            // GD.Print($"|    Perfection check: auxIndex {auxIndex} mainIndex {mainIndex} - {auxIndex >= mainIndex}");
            if (auxIndex >= mainIndex) return true;
            return false;
        }
        else if (virya == PathData.Virya.HalfStep)
        {
            // GD.Print($"|    Half-Step check: auxIndex {auxIndex} mainIndex {mainIndex} - !{auxIndex < mainIndex} | {aux.CurrentRealm == main.CurrentRealm} {aux.CurrentMinorRealm == PathData.MinorRealm.Late} {aux.CurrentRealmProgress >= 1f}");
            if (auxIndex < mainIndex) return false;
            if (aux.CurrentRealm == main.CurrentRealm
                && aux.CurrentMinorRealm == PathData.MinorRealm.Late
                && aux.CurrentRealmProgress >= 1f) return true;
            return false;
        }
        return false;
    }

    private static bool CheckIfPathAtCompletion(PathData path) =>
        path.CurrentMinorRealm == PathData.MinorRealm.Late && path.CurrentRealmProgress >= 1f;

    private static void CultivatePath(ProfileData data, PathData path)
    {
        var dailyExp = data.TotalDailyExp;
        var c = path.CurrentRealmProgress;
        var newProgress = path.AddExp(dailyExp);
        path.CurrentRealmProgress = newProgress;
        if (path.CurrentRealmProgress >= 1f && path.CurrentMinorRealm < PathData.MinorRealm.Late)
        {
            var req = PathData.ExperienceReqs[(path.CurrentRealm, path.CurrentMinorRealm)];
            var exp = (long)(path.CurrentRealmProgress * req);
            exp -= req;
            AdvanceMinorRealm(path);
            req = PathData.ExperienceReqs[(path.CurrentRealm, path.CurrentMinorRealm)];
            path.CurrentRealmProgress = (float)((float)exp / req);
        }
        // GD.Print($"|    Path progress: {c} -> {path.CurrentRealmProgress:P2} | Remaining XP: {path.GetXpToTargetRealm(path.CurrentRealm, PathData.MinorRealm.Late):N0} XP");
    }

    private static void GenericAddXpToPath(PathData path, long xp)
    {
        var c = path.CurrentRealmProgress;
        var newProgress = path.AddExp(xp);
        path.CurrentRealmProgress = newProgress;
        if (path.CurrentRealmProgress >= 1f && path.CurrentMinorRealm < PathData.MinorRealm.Late)
        {
            var req = PathData.ExperienceReqs[(path.CurrentRealm, path.CurrentMinorRealm)];
            var exp = (long)(path.CurrentRealmProgress * req);
            exp -= req;
            AdvanceMinorRealm(path);
            req = PathData.ExperienceReqs[(path.CurrentRealm, path.CurrentMinorRealm)];
            path.CurrentRealmProgress = (float)((float)exp / req);
        }
    }

}

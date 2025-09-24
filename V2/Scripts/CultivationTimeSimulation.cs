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

    public static int CalculateDaysToVirya(ProfileData data, PathData.Virya virya)
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
            CultivatePath(clone, path);
            days++;

            if (days % 10 == 0)
            {
                GD.Print($"Day {days}: {path.SelectedPath} {path.CurrentRealm} {path.CurrentMinorRealm} - {path.CurrentRealmProgress:P2}");
            }

            if (main.CurrentMinorRealm >= PathData.MinorRealm.Late && main.CurrentRealmProgress < 1f)
                clone.PassiveCultivation.ViryaPercent = 0f;

            if (CheckViryaStatus(clone, PathData.Virya.Eminence) || CheckViryaStatus(clone, PathData.Virya.Perfection))
                clone.PassiveCultivation.ViryaPercent = 0.2f;

            if (CheckViryaStatus(clone, virya)) break;

            if (path.CurrentRealmProgress >= 1f)
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

        if (virya == PathData.Virya.Eminence)
        {
            if (auxIndex >= mainIndex) return true;
            if (auxIndex == mainIndex - 1 && aux.CurrentMinorRealm >= PathData.MinorRealm.Middle) return true;
            return false;
        }
        else if (virya == PathData.Virya.Perfection)
        {
            if (auxIndex >= mainIndex) return true;
            return false;
        }
        else if (virya == PathData.Virya.HalfStep)
        {
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
        var dailyExp = data.DailyPassiveExp;
        var c = path.CurrentRealmProgress;
        var newProgress = path.AddExp(dailyExp);
        path.CurrentRealmProgress = newProgress;
        GD.Print($"|    Path progress: {c} -> {path.CurrentRealmProgress}");
    }

}

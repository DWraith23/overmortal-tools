using System;
using System.Collections.Generic;
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
}

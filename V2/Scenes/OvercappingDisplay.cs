using Godot;
using OvermortalTools.V2.Resources;
using OvermortalTools.V2.Scripts;
using System;
using System.Linq;

namespace OvermortalTools.V2.Scenes;

public partial class OvercappingDisplay : VBoxContainer
{
    private LabeledSpinbox DaysToTimegateSpinBox => GetNode<LabeledSpinbox>("Timegate");

    private LineEdit CompletionDaysToNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Completion/Days To");
    private LineEdit CompletionOvercapNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Completion/Overcap");
    private LineEdit EminenceDaysToNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Eminence/Days To");
    private LineEdit EminenceOvercapNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Eminence/Overcap");
    private LineEdit PerfectionDaysToNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Perfection/Days To");
    private LineEdit PerfectionOvercapNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Perfection/Overcap");
    private LineEdit HalfStepDaysToNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Half Step/Days To");
    private LineEdit HalfStepOvercapNoMyrm => GetNode<LineEdit>("Scroller/Container/No Myrimon/Half Step/Overcap");

    private LineEdit CompletionDaysToWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Completion/Days To");
    private LineEdit CompletionOvercapWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Completion/Overcap");
    private LineEdit EminenceDaysToWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Eminence/Days To");
    private LineEdit EminenceOvercapWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Eminence/Overcap");
    private LineEdit PerfectionDaysToWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Perfection/Days To");
    private LineEdit PerfectionOvercapWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Perfection/Overcap");
    private LineEdit HalfStepDaysToWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Half Step/Days To");
    private LineEdit HalfStepOvercapWithMyrm => GetNode<LineEdit>("Scroller/Container/With Myrimon/Half Step/Overcap");

    private ProfileData _data = new();
    public ProfileData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            Update();
            if (value != null)
            {
                _data.Changed += Update;
            }
        }
    }

    public override void _Ready()
    {
        DaysToTimegateSpinBox.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.DaysToTimegate = (int)value;
        };
    }

    private void Update()
    {
        if (Data == null) return;

        DaysToTimegateSpinBox.SetValueNoSignal(Data.DaysToTimegate);

        CompletionDaysToNoMyrm.Text = GetDaysToVirya(PathData.Virya.Completion, false);
        CompletionOvercapNoMyrm.Text = GetOvercap(PathData.Virya.Completion, false);
        EminenceDaysToNoMyrm.Text = GetDaysToVirya(PathData.Virya.Eminence, false);
        EminenceOvercapNoMyrm.Text = GetOvercap(PathData.Virya.Eminence, false);
        PerfectionDaysToNoMyrm.Text = GetDaysToVirya(PathData.Virya.Perfection, false);
        PerfectionOvercapNoMyrm.Text = GetOvercap(PathData.Virya.Perfection, false);
        HalfStepDaysToNoMyrm.Text = GetDaysToVirya(PathData.Virya.HalfStep, false);
        HalfStepOvercapNoMyrm.Text = GetOvercap(PathData.Virya.HalfStep, false);

        CompletionDaysToWithMyrm.Text = GetDaysToVirya(PathData.Virya.Completion, true);
        CompletionOvercapWithMyrm.Text = GetOvercap(PathData.Virya.Completion, true);
        EminenceDaysToWithMyrm.Text = GetDaysToVirya(PathData.Virya.Eminence, true);
        EminenceOvercapWithMyrm.Text = GetOvercap(PathData.Virya.Eminence, true);
        PerfectionDaysToWithMyrm.Text = GetDaysToVirya(PathData.Virya.Perfection, true);
        PerfectionOvercapWithMyrm.Text = GetOvercap(PathData.Virya.Perfection, true);
        HalfStepDaysToWithMyrm.Text = GetDaysToVirya(PathData.Virya.HalfStep, true);
        HalfStepOvercapWithMyrm.Text = GetOvercap(PathData.Virya.HalfStep, true);
    }

    private string GetOvercap(PathData.Virya virya, bool myrm)
    {
        if (Data == null || Data.DaysToTimegate == 0) return "N/A";

        var mainPath = Data.GetPathsInOrder().First();
        if (mainPath.CurrentRealm < PathData.Realm.Incarnation) return "Realm too low";
        if (mainPath.CurrentRealm == PathData.Realm.Eternal) return "No data.";

        var daysTo = CultivationTimeSimulation.CalculateDaysToVirya(Data, virya, myrm);
        if (daysTo == -1) return "N/A";

        var days = Data.DaysToTimegate - daysTo;
        if (days <= 0) return $"Late By {-days} Days";

        var totalExp = Data.TotalDailyExp * days;

        var nextRealm = mainPath.CurrentRealm + 1;

        var early = PathData.GetRealmExpReq(nextRealm, PathData.MinorRealm.Early);
        var mid = PathData.GetRealmExpReq(nextRealm, PathData.MinorRealm.Middle);
        var late = PathData.GetRealmExpReq(nextRealm, PathData.MinorRealm.Late);

        if (totalExp < early) return $"{totalExp / (float)early * 100f:N1}% of Early";
        if (totalExp - early < mid) return $"{(totalExp - early) / (float)mid * 100f:N1}% of Middle";
        if (totalExp - early - mid < late) return $"{(totalExp - early - mid) / (float)late * 100f:N1}% of Late";

        return "N/A";
    }

    private string GetDaysToVirya(PathData.Virya virya, bool myrm)
    {
        if (Data == null || Data.DaysToTimegate == 0) return "N/A";
        var days = CultivationTimeSimulation.CalculateDaysToVirya(Data, virya, myrm);
        return days == -1 ? "N/A" : days == -2 ? "0" : days.ToString("N0");
    }
}

using Godot;
using OvermortalTools.Scripts;
using System;
using System.Linq;

namespace OvermortalTools.Scenes;

public partial class CosmoapsisCalculator : PanelContainer
{
    [Signal] public delegate void ChangedEventHandler();

    /// <summary>
    /// Contains a key (realm name) and a tuple with the amount required as Item1 and the cumulative xp requirement as Item2
    /// </summary>
    public readonly static System.Collections.Generic.Dictionary<string, (int, int)> RealmXpRequirements = new()
    {
        { "Novice", (2, 0) },
        { "Connection F1", (13, 2) },
        { "Connection F2", (73, 15) },
        { "Connection F3", (176, 88) },
        { "Connection F4", (500, 264) },
        { "Connection F5", (1000, 764) },
        { "Connection F6", (1600, 1764) },
        { "Connection F7", (2500, 3364) },
        { "Connection F8", (3000, 5864) },
        { "Connection F9", (3500, 8864) },
        { "Connection F10", (4500, 12364) },
        { "Foundation Early G1", (11510, 16864) },
        { "Foundation Early G2", (14530, 28374) },
        { "Foundation Middle G1", (14595, 42904) },
        { "Foundation Middle G2", (18464, 57499) },
        { "Foundation Middle G3", (22223, 75963) },
        { "Foundation Late G1", (33585, 98186) },
        { "Foundation Late G2", (42169, 131771) },
        { "Foundation Late G3", (51125, 173940) },
        { "Foundation Late G4", (59706, 225065) },
        { "Virtuoso Early G1", (39411, 284771) },
        { "Virtuoso Early G2", (49482, 324182) },
        { "Virtuoso Early G3", (59991, 373664) },
        { "Virtuoso Early G4", (70062, 433655) },
        { "Virtuoso Middle G1", (60862, 503717) },
        { "Virtuoso Middle G2", (77123, 564579) },
        { "Virtuoso Middle G3", (92919, 641702) },
        { "Virtuoso Middle G4", (108715, 734621) },
        { "Virtuoso Middle G5", (124975, 843336) },
        { "Virtuoso Late G1", (158370, 968311) },
        { "Virtuoso Late G2", (199138, 1126681) },
        { "Virtuoso Late G3", (241475, 1325819) },
        { "Virtuoso Late G4", (280676, 1567294) },
        { "Virtuoso Late G5", (324579, 1847970) },
        { "Virtuoso Late G6", (364077, 2172549) },
        { "Nascent Soul Early G1", (183679, 2536626) },
        { "Nascent Soul Early G2", (230962, 2720305) },
        { "Nascent Soul Early G3", (280065, 2951267) },
        { "Nascent Soul Early G4", (325529, 3231332) },
        { "Nascent Soul Early G5", (376450, 3556861) },
        { "Nascent Soul Early G6", (421915, 3933311) },
        { "Nascent Soul Middle G1", (308712, 4355226) },
        { "Nascent Soul Middle G2", (389749, 4663938) },
        { "Nascent Soul Middle G3", (470786, 5053687) },
        { "Nascent Soul Middle G4", (551823, 5524473) },
        { "Nascent Soul Middle G5", (632859, 6076296) },
        { "Nascent Soul Middle G6", (710038, 6709155) },
        { "Nascent Soul Middle G7", (801067, 7419193) },
        { "Nascent Soul Late G1", (719563, 8220260) },
        { "Nascent Soul Late G2", (918827, 8939823) },
        { "Nascent Soul Late G3", (1095950, 9858650) },
        { "Nascent Soul Late G4", (1305208, 10954600) },
        { "Nascent Soul Late G5", (1483697, 12259808) },
        { "Nascent Soul Late G6", (1684499, 13743505) },
        { "Nascent Soul Late G7", (1862988, 15428004) },
        { "Nascent Soul Late G8", (2063790, 17290992) },
        { "Incarnation Early G1", (789653, 19354782) },
        { "Incarnation Early G2", (1008326, 20144435) },
        { "Incarnation Early G3", (1202702, 21152761) },
        { "Incarnation Early G4", (1421375, 22355463) },
        { "Incarnation Early G5", (1615751, 23776838) },
        { "Incarnation Early G6", (1834424, 25392589) },
        { "Incarnation Early G7", (2039156, 27227013) },
        { "Incarnation Early G8", (2247473, 29266169) },
        { "Incarnation Middle G1", (1392023, 31513642) },
        { "Incarnation Middle G2", (1778696, 32905665) },
        { "Incarnation Middle G3", (2139589, 34684361) },
        { "Incarnation Middle G4", (2500485, 36823950) },
        { "Incarnation Middle G5", (2861380, 39324435) },
        { "Incarnation Middle G6", (3222274, 42185815) },
        { "Incarnation Middle G7", (3583169, 45408089) },
        { "Incarnation Middle G8", (3969842, 48991258) },
        { "Incarnation Middle G9", (4394107, 52961100) },
        { "Incarnation Late G1", (1483013, 57355207) },
        { "Incarnation Late G2", (1853766, 58838220) },
        { "Incarnation Late G3", (2286312, 60691986) },
        { "Incarnation Late G4", (2595272, 62978298) },
        { "Incarnation Late G5", (3027818, 65573570) },
        { "Incarnation Late G6", (3460363, 68601388) },
        { "Incarnation Late G7", (3769324, 72061751) },
        { "Incarnation Late G8", (4201870, 75831075) },
        { "Incarnation Late G9", (4572623, 80032945) },
        { "Incarnation Late G10", (4943376, 84605568) },
        { "Incarnation Late G11", (5252337, 89548944) },
        { "Incarnation Late G12", (5623090, 94801281) },
        { "Incarnation Late G13", (5932051, 100424371) },
        { "Incarnation Late G14", (6241012, 106356422) },
        { "Incarnation Late G15", (6549973, 112597434) },
        { "Voidbreak Early G1", (1020222, 113617656) },
        { "Voidbreak Early G2", (1292280, 114909936) },
        { "Voidbreak Early G3", (1496324, 116406260) },
        { "Voidbreak Early G4", (1836398, 118242658) },
        { "Voidbreak Early G5", (2040442, 120283100) },
        { "Voidbreak Early G6", (2380515, 122663615) },
        { "Voidbreak Early G7", (2584560, 125248175) },
        { "Voidbreak Early G8", (2856619, 128104794) },
        { "Voidbreak Early G9", (3110128, 131214922) },
        { "Voidbreak Early G10", (3363637, 134578559) },
        { "Voidbreak Early G11", (3617147, 138195706) },
        { "Voidbreak Early G12", (3870656, 142066362) },
        { "Voidbreak Early G13", (4080884, 146147246) },
        { "Voidbreak Early G14", (4216914, 150364160) },
        { "Voidbreak Early G15", (4488973, 154853133) },
        { "Voidbreak Early G16", (4761031, 159614164) },
        { "Voidbreak Early G17", (4897061, 164511225) },
        { "Voidbreak Early G18", (5169120, 169680345) },
        { "Voidbreak Early G19", (5373164, 175053509) },
        { "Voidbreak Early G20", (5577208, 180630717) },
        { "Voidbreak Middle G1", (2131409, 182762126) },
        { "Voidbreak Middle G2", (2699785, 185461911) },
        { "Voidbreak Middle G3", (3126066, 188587977) },
        { "Voidbreak Middle G4", (3839536, 192427513) },
        { "Voidbreak Middle G5", (4262817, 196690330) },
        { "Voidbreak Middle G6", (4973288, 201663618) },
        { "Voidbreak Middle G7", (5399569, 207063187) },
        { "Voidbreak Middle G8", (5967944, 213031131) },
        { "Voidbreak Middle G9", (6445896, 219477027) },
        { "Voidbreak Middle G10", (6923848, 226400875) },
        { "Voidbreak Middle G11", (7401801, 233802676) },
        { "Voidbreak Middle G12", (7879753, 241682429) },
        { "Voidbreak Middle G13", (8357705, 250040134) },
        { "Voidbreak Middle G14", (8835658, 258875792) },
        { "Voidbreak Middle G15", (9313610, 268189402) },
        { "Voidbreak Middle G16", (9791562, 277980964) },
        { "Voidbreak Middle G17", (10269515, 288250479) },
        { "Voidbreak Middle G18", (10747467, 298997946) },
        { "Voidbreak Middle G19", (11225420, 310223366) },
        { "Voidbreak Middle G20", (11651701, 321875067) },
        { "Voidbreak Late G1", (4615724, 326490791) },
        { "Voidbreak Late G2", (5846585, 332337376) },
        { "Voidbreak Late G3", (7077445, 339414821) },
        { "Voidbreak Late G4", (8308306, 347723127) },
        { "Voidbreak Late G5", (9539165, 357262292) },
        { "Voidbreak Late G6", (10770025, 368032317) },
        { "Voidbreak Late G7", (11898313, 379930630) },
        { "Voidbreak Late G8", (12924031, 392854661) },
        { "Voidbreak Late G9", (14154891, 407009552) },
        { "Voidbreak Late G10", (15385751, 422395303) },
        { "Voidbreak Late G11", (16308897, 438704200) },
        { "Voidbreak Late G12", (17232041, 455936241) },
        { "Voidbreak Late G13", (18462902, 474399143) },
        { "Voidbreak Late G14", (19078331, 493477474) },
        { "Voidbreak Late G15", (20309192, 513786666) },
        { "Voidbreak Late G16", (21540052, 535326718) },
        { "Voidbreak Late G17", (22155482, 557482200) },
        { "Voidbreak Late G18", (23386342, 580868542) },
        { "Voidbreak Late G19", (24924917, 605793459) },
        { "Voidbreak Late G20", (25232632, 631026091) },
    };

    private float _currentCosmoapsisValue = 0;
    [Export]
    public float CurrentCosmoapsisValue
    {
        get => _currentCosmoapsisValue;
        set
        {
            _currentCosmoapsisValue = value;
            if (!LineEditsValid) return;
            PerSecondField.Text = PerSecond.ToString("N1");
            PerMinuteField.Text = PerMinute.ToString("N0");
            PerHourField.Text = PerHour.ToString("N0");
            PerDayField.Text = PerDay.ToString("N0");
            PerWeekField.Text = PerWeek.ToString("N0");
            
            if (CurrentCosmoapsis.Text != CurrentCosmoapsisValue.ToString())
                CurrentCosmoapsis.Text = CurrentCosmoapsisValue.ToString();

            SetTimeRemainingText();
        }
    }
    private float _currentXpTowardsValue = 0;
    [Export]
    public float CurrentXpTowardsValue
    {
        get => _currentXpTowardsValue;
        set
        {
            _currentXpTowardsValue = value;
            if (!LineEditsValid) return;
            SetTimeRemainingText();
        }
    }


    [ExportGroup("Nodes")]
    [Export] public AuraGemWidget AuraGem { get; set; }
    [Export] private LineEdit CurrentCosmoapsis { get; set; }
    [Export] public OptionButton CurrentRealmSelection  { get; set; }
    [Export] public OptionButton TargetRealmSelection { get; set; }
    [Export] public LineEdit CurrentXPTowards { get; set; }
    [ExportSubgroup("Calculated Values")]
    [Export] private LineEdit PerSecondField { get; set; }
    [Export] private LineEdit PerMinuteField { get; set; }
    [Export] private LineEdit PerHourField { get; set; }
    [Export] private LineEdit PerDayField { get; set; }
    [Export] private LineEdit PerWeekField { get; set; }
    [Export] private LineEdit XPRemainingField { get; set; }
    [Export] private LineEdit TimeRemainingField { get; set; }
    [Export] private LineEdit TaoistYearsField { get; set; }


    private bool LineEditsValid =>
        CurrentCosmoapsis != null &&
        PerSecondField != null &&
        PerMinuteField != null &&
        PerHourField != null &&
        PerDayField != null &&
        PerWeekField != null;


    private string CurrentCosmoapsisText { get; set; } = "";
    private string CurrentXpTowardsText { get; set; } = "";

    private float PerSecond => CurrentCosmoapsisValue / 8;
    private float PerMinute => (float)Math.Round(PerSecond * 60, 1);
    private float PerHour => (float)Math.Round(PerMinute * 60, 0);
    public float PerDay => (float)Math.Round(PerHour * 24, 0);
    private float PerWeek => (float)Math.Round(PerDay * 7, 0);


    public float CurrentXpTowards => CurrentXpTowardsValue;
    public float XPRemaining => GetXpRemaining();
    private float GetXpRemaining()
    {
        if (CurrentRealmSelection.GetSelectedId() == -1) return 0;
        if (TargetRealmSelection.GetSelectedId() == -1) return 0;

        var current = CurrentRealmSelection.GetItemText(CurrentRealmSelection.GetSelectedId());
        var target = TargetRealmSelection.GetItemText(TargetRealmSelection.GetSelectedId());

        return RealmXpRequirements[target].Item2 - RealmXpRequirements[current].Item2 - CurrentXpTowardsValue;
    }

    private TimeSpan TimeRemaining => TimeSpan.FromSeconds((double)XPRemaining / PerSecond);
    private string TimeRemainingString =>
        $"{TimeRemaining.Days} Days, {TimeRemaining.Hours} Hours, {TimeRemaining.Minutes} Minutes ({TimeRemaining.TotalHours.ToString("N1")} Hours)";
    
    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        if (CurrentRealmSelection != null)
        {
            foreach (var realm in RealmXpRequirements.Keys)
            {
                CurrentRealmSelection.AddItem(realm);
            }
        }
        if (TargetRealmSelection != null)
        {
            foreach (var realm in RealmXpRequirements.Keys)
            {
                if (realm == "Novice") continue;
                TargetRealmSelection.AddItem(realm);
            }
        }
        AuraGem.Changed += EmitChanged;
    }

    public void Update() => EmitChanged();

    private void OnCurrentCosmoapsisChanged(string text)
    {
        if (text == string.Empty) return;
        if (text.Last() == '.') return;
        if (!text.IsValidFloat())
        {
            CurrentCosmoapsis.DeleteLastCharacter();
            return;
        }
        CurrentCosmoapsisText = text;
        CurrentCosmoapsisValue = float.Parse(text);
        AuraGem.Update(float.Parse(text));
        EmitChanged();
    }

    public void SetCurrentCosmoapsis(float value) => OnCurrentCosmoapsisChanged(value.ToString());

    private void OnCurrentXpTowardsChanged(string text)
    {
        if (text == "")
        {
            OnCurrentXpTowardsChanged("0");
            CurrentXPTowards.Text = CurrentXpTowardsValue.ToString("N0");
            return;
        }
        if (!text.IsValidFloat())
        {
            CurrentXPTowards.DeleteLastCharacter();
            return;
        }
        CurrentXpTowardsText = text;
        CurrentXpTowardsValue = float.Parse(text);
        SetRemainingXPText();
        SetTimeRemainingText();
        EmitChanged();
    }

    public void OnCurrentRealmSelect(long index)
    {
        TargetRealmSelection.Clear();
        var currentRealm = RealmXpRequirements.Keys.ToList()
            .First(key => key == CurrentRealmSelection.GetItemText((int)index));
        var currentCumulative = RealmXpRequirements[currentRealm].Item2;
        var higherRealms = RealmXpRequirements.Where(realm => realm.Value.Item2 > currentCumulative);
        foreach (var realm in higherRealms)
        {
            TargetRealmSelection.AddItem(realm.Key);
        }
        OnCurrentXpTowardsChanged(CurrentXpTowardsText);
        CurrentXPTowards.Text = CurrentXpTowardsValue.ToString("N0");
        EmitChanged();
    }

    private void OnTargetRealmSelect(long index)
    {
        _ = index;
        OnCurrentXpTowardsChanged(CurrentXpTowardsText);
        CurrentXPTowards.Text = CurrentXpTowardsValue.ToString("N0");
        EmitChanged();
    }

    private void SetRemainingXPText() => XPRemainingField.Text = XPRemaining.ToString("N0");
    private void SetTimeRemainingText()
    {
        if (CurrentRealmSelection.GetSelectedId() == -1 || TargetRealmSelection.GetSelectedId() == -1)
        {
            TimeRemainingField.Text = "N/A";
            TaoistYearsField.Text = "N/A";
            return;
        }
        if (CurrentCosmoapsisValue == 0)
        {
            TimeRemainingField.Text = "infinity";
            TaoistYearsField.Text = "infinity";
        }
        else
        {
            TimeRemainingField.Text = TimeRemainingString;
            var minutes = TimeRemaining.TotalMinutes;
            var taoist = (float)Math.Round(minutes / 15, 0);
            TaoistYearsField.Text = taoist.ToString("N0");
        }
    }

    private void EmitChanged() => EmitSignal(SignalName.Changed);

}

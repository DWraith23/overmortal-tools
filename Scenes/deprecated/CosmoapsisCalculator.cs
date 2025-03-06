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
    public readonly static System.Collections.Generic.Dictionary<string, (long, long)> RealmXpRequirements = new()
    {
        { "Novice", (2, 2) },
        { "Connection F1", (13, 15) },
        { "Connection F2", (73, 88) },
        { "Connection F3", (176, 264) },
        { "Connection F4", (500, 764) },
        { "Connection F5", (1000, 1764) },
        { "Connection F6", (1600, 3364) },
        { "Connection F7", (2500, 5864) },
        { "Connection F8", (3000, 8864) },
        { "Connection F9", (3500, 12364) },
        { "Connection F10", (4500, 16864) },
        { "Foundation Early 1G", (12376, 29240) },
        { "Foundation Early 2G", (15624, 44864) },
        { "Foundation Middle 1G", (15708, 60572) },
        { "Foundation Middle 2G", (19873, 80445) },
        { "Foundation Middle 3G", (22223, 102668) },
        { "Foundation Late 1G", (36180, 138848) },
        { "Foundation Late 2G", (45426, 184274) },
        { "Foundation Late 3G", (55074, 239348) },
        { "Foundation Late 4G", (64320, 303668) },
        { "Virtuoso Early 1G", (39438, 343106) },
        { "Virtuoso Early 2G", (49517, 392623) },
        { "Virtuoso Early 3G", (60033, 452656) },
        { "Virtuoso Early 4G", (70112, 522768) },
        { "Virtuoso Middle 1G", (60915, 583683) },
        { "Virtuoso Middle 2G", (77190, 660873) },
        { "Virtuoso Middle 3G", (93000, 753873) },
        { "Virtuoso Middle 4G", (108810, 862683) },
        { "Virtuoso Middle 5G", (125085, 987768) },
        { "Virtuoso Late 1G", (158500, 1146268) },
        { "Virtuoso Late 2G", (199301, 1345569) },
        { "Virtuoso Late 3G", (241672, 1587241) },
        { "Virtuoso Late 4G", (280905, 1868146) },
        { "Virtuoso Late 5G", (324845, 2192991) },
        { "Virtuoso Late 6G", (364077, 2557068) },
        { "Nascent Soul Early 1G", (183679, 2740747) },
        { "Nascent Soul Early 2G", (230962, 2971709) },
        { "Nascent Soul Early 3G", (280065, 3251774) },
        { "Nascent Soul Early 4G", (325529, 3577303) },
        { "Nascent Soul Early 5G", (376450, 3953753) },
        { "Nascent Soul Early 6G", (421915, 4375668) },
        { "Nascent Soul Middle 1G", (308712, 4684380) },
        { "Nascent Soul Middle 2G", (389749, 5074129) },
        { "Nascent Soul Middle 3G", (470786, 5544915) },
        { "Nascent Soul Middle 4G", (551823, 6096738) },
        { "Nascent Soul Middle 5G", (632859, 6729597) },
        { "Nascent Soul Middle 6G", (710038, 7439635) },
        { "Nascent Soul Middle 7G", (801067, 8240702) },
        { "Nascent Soul Late 1G", (719563, 8960265) },
        { "Nascent Soul Late 2G", (918827, 9879092) },
        { "Nascent Soul Late 3G", (1095950, 10975042) },
        { "Nascent Soul Late 4G", (1305208, 12280250) },
        { "Nascent Soul Late 5G", (1483697, 13763947) },
        { "Nascent Soul Late 6G", (1684499, 15448446) },
        { "Nascent Soul Late 7G", (1862988, 17311434) },
        { "Nascent Soul Late 8G", (2063790, 19375224) },
        { "Incarnation Early 1G", (789653, 20164877) },
        { "Incarnation Early 2G", (1008326, 21173203) },
        { "Incarnation Early 3G", (1202702, 22375905) },
        { "Incarnation Early 4G", (1421375, 23797280) },
        { "Incarnation Early 5G", (1615751, 25413031) },
        { "Incarnation Early 6G", (1834424, 27247455) },
        { "Incarnation Early 7G", (2039156, 29286611) },
        { "Incarnation Early 8G", (2247473, 31534084) },
        { "Incarnation Middle 1G", (1392023, 32926107) },
        { "Incarnation Middle 2G", (1778696, 34704803) },
        { "Incarnation Middle 3G", (2139589, 36844392) },
        { "Incarnation Middle 4G", (2500485, 39344877) },
        { "Incarnation Middle 5G", (2861380, 42206257) },
        { "Incarnation Middle 6G", (3222274, 45428531) },
        { "Incarnation Middle 7G", (3595169, 49023700) },
        { "Incarnation Middle 8G", (3969842, 52993542) },
        { "Incarnation Middle 9G", (4394107, 57387649) },
        { "Incarnation Late 1G", (1483013, 58870662) },
        { "Incarnation Late 2G", (1853766, 60724428) },
        { "Incarnation Late 3G", (2286312, 63010740) },
        { "Incarnation Late 4G", (2595272, 65606012) },
        { "Incarnation Late 5G", (3027818, 68633830) },
        { "Incarnation Late 6G", (3460363, 72094193) },
        { "Incarnation Late 7G", (3769324, 75863517) },
        { "Incarnation Late 8G", (4201870, 80065387) },
        { "Incarnation Late 9G", (4572623, 84638010) },
        { "Incarnation Late 10G", (4943376, 89581386) },
        { "Incarnation Late 11G", (5252337, 94833723) },
        { "Incarnation Late 12G", (5623090, 100456813) },
        { "Incarnation Late 13G", (5932051, 106388864) },
        { "Incarnation Late 14G", (6241012, 112629876) },
        { "Incarnation Late 15G", (6549973, 119179849) },
        { "Voidbreak Early 1G", (1020222, 120200071) },
        { "Voidbreak Early 2G", (1292280, 121492351) },
        { "Voidbreak Early 3G", (1496324, 122988675) },
        { "Voidbreak Early 4G", (1836398, 124825073) },
        { "Voidbreak Early 5G", (2040442, 126865515) },
        { "Voidbreak Early 6G", (2380515, 129246030) },
        { "Voidbreak Early 7G", (2584560, 131830590) },
        { "Voidbreak Early 8G", (2856619, 134687209) },
        { "Voidbreak Early 9G", (3128678, 137815887) },
        { "Voidbreak Early 10G", (3400737, 141216624) },
        { "Voidbreak Early 11G", (3604781, 144821405) },
        { "Voidbreak Early 12G", (3808825, 148630230) },
        { "Voidbreak Early 13G", (4080884, 152711114) },
        { "Voidbreak Early 14G", (4216914, 156928028) },
        { "Voidbreak Early 15G", (4488973, 161417001) },
        { "Voidbreak Early 16G", (4761031, 166178032) },
        { "Voidbreak Early 17G", (4897061, 171075093) },
        { "Voidbreak Early 18G", (5169120, 176244213) },
        { "Voidbreak Early 19G", (5373164, 181617377) },
        { "Voidbreak Early 20G", (5577208, 187194585) },
        { "Voidbreak Middle 1G", (2131409, 189325994) },
        { "Voidbreak Middle 2G", (2699785, 192025779) },
        { "Voidbreak Middle 3G", (3126066, 195151845) },
        { "Voidbreak Middle 4G", (3836536, 198988381) },
        { "Voidbreak Middle 5G", (4262817, 203251198) },
        { "Voidbreak Middle 6G", (4973288, 208224486) },
        { "Voidbreak Middle 7G", (5399569, 213624055) },
        { "Voidbreak Middle 8G", (5967944, 219591999) },
        { "Voidbreak Middle 9G", (6536321, 226128320) },
        { "Voidbreak Middle 10G", (7104696, 233233016) },
        { "Voidbreak Middle 11G", (7530977, 240763993) },
        { "Voidbreak Middle 12G", (7957260, 248721253) },
        { "Voidbreak Middle 13G", (8525635, 257246888) },
        { "Voidbreak Middle 14G", (8809823, 266056711) },
        { "Voidbreak Middle 15G", (9378199, 275434910) },
        { "Voidbreak Middle 16G", (9946574, 285381484) },
        { "Voidbreak Middle 17G", (10230762, 295612246) },
        { "Voidbreak Middle 18G", (10799138, 306411384) },
        { "Voidbreak Middle 19G", (11225420, 317636804) },
        { "Voidbreak Middle 20G", (11651701, 329288505) },
        { "Voidbreak Late 1G", (4615726, 333904231) },
        { "Voidbreak Late 2G", (5846585, 339750816) },
        { "Voidbreak Late 3G", (6769731, 346520547) },
        { "Voidbreak Late 4G", (8308306, 354828853) },
        { "Voidbreak Late 5G", (9231451, 364060304) },
        { "Voidbreak Late 6G", (10770250, 374830554) },
        { "Voidbreak Late 7G", (11693172, 386523726) },
        { "Voidbreak Late 8G", (12924031, 399447757) },
        { "Voidbreak Late 9G", (14154891, 413602648) },
        { "Voidbreak Late 10G", (15385751, 428988399) },
        { "Voidbreak Late 11G", (16308897, 445297296) },
        { "Voidbreak Late 12G", (17232041, 462529337) },
        { "Voidbreak Late 13G", (18462902, 480992239) },
        { "Voidbreak Late 14G", (19078331, 500070570) },
        { "Voidbreak Late 15G", (20309192, 520379762) },
        { "Voidbreak Late 16G", (21540052, 541919814) },
        { "Voidbreak Late 17G", (22155482, 564075296) },
        { "Voidbreak Late 18G", (23386342, 587461638) },
        { "Voidbreak Late 19G", (24309487, 611771125) },
        { "Voidbreak Late 20G", (25232632, 637003757) },
        { "Wholeness Early 1G", (2802204, 639805961) },
        { "Wholeness Early 2G", (3549458, 643355419) },
        { "Wholeness Early 3G", (4109898, 647465317) },
        { "Wholeness Early 4G", (5043967, 652509284) },
        { "Wholeness Early 5G", (5604407, 658113691) },
        { "Wholeness Early 6G", (6538475, 664652166) },
        { "Wholeness Early 7G", (7098915, 671751081) },
        { "Wholeness Early 8G", (7846170, 679597251) },
        { "Wholeness Early 9G", (8593424, 688190675) },
        { "Wholeness Early 10G", (9340679, 697531354) },
        { "Wholeness Early 11G", (9901119, 707432473) },
        { "Wholeness Early 12G", (10461560, 717894033) },
        { "Wholeness Early 13G", (11208814, 729102847) },
        { "Wholeness Early 14G", (11582441, 740685288) },
        { "Wholeness Early 15G", (12329696, 753014984) },
        { "Wholeness Early 16G", (13076950, 766091934) },
        { "Wholeness Early 17G", (13450577, 779542511) },
        { "Wholeness Early 18G", (14197831, 793740342) },
        { "Wholeness Early 19G", (14758272, 808498614) },
        { "Wholeness Early 20G", (15318712, 823817326) },
        { "Wholeness Middle 1G", (3959269, 827776595) },
        { "Wholeness Middle 2G", (5015074, 832791669) },
        { "Wholeness Middle 3G", (5806927, 838598596) },
        { "Wholeness Middle 4G", (7126683, 845725279) },
        { "Wholeness Middle 5G", (7918537, 853643816) },
        { "Wholeness Middle 6G", (9238293, 862882109) },
        { "Wholeness Middle 7G", (10030147, 872912256) },
        { "Wholeness Middle 8G", (11085952, 883998208) },
        { "Wholeness Middle 9G", (12141757, 896139965) },
        { "Wholeness Middle 10G", (13197562, 909337527) },
        { "Wholeness Middle 11G", (13989415, 923326942) },
        { "Wholeness Middle 12G", (14781270, 938108212) },
        { "Wholeness Middle 13G", (15837074, 953945286) },
        { "Wholeness Middle 14G", (16364976, 970310262) },
        { "Wholeness Middle 15G", (17420782, 987731044) },
        { "Wholeness Middle 16G", (18476587, 1006207631) },
        { "Wholeness Middle 17G", (19004489, 1025212120) },
        { "Wholeness Middle 18G", (20060294, 1045272414) },
        { "Wholeness Middle 19G", (20852147, 1066124561) },
        { "Wholeness Middle 20G", (21644001, 1087768562) },
        { "Wholeness Late 1G", (4566759, 1092335321) },
        { "Wholeness Late 2G", (5784560, 1098119881) },
        { "Wholeness Late 3G", (6697912, 1104817793) },
        { "Wholeness Late 4G", (8220165, 1113037958) },
        { "Wholeness Late 5G", (9133517, 1122171475) },
        { "Wholeness Late 6G", (10655769, 1132827244) },
        { "Wholeness Late 7G", (11569121, 1144396365) },
        { "Wholeness Late 8G", (12786923, 1157183288) },
        { "Wholeness Late 9G", (14004725, 1171188013) },
        { "Wholeness Late 10G", (15222528, 1186410541) },
        { "Wholeness Late 11G", (16135879, 1202546420) },
        { "Wholeness Late 12G", (17049231, 1219595651) },
        { "Wholeness Late 13G", (18267033, 1237862684) },
        { "Wholeness Late 14G", (18875934, 1256738618) },
        { "Wholeness Late 15G", (20093737, 1276832355) },
        { "Wholeness Late 16G", (21311538, 1298143893) },
        { "Wholeness Late 17G", (21920440, 1320064333) },
        { "Wholeness Late 18G", (23138242, 1343202575) },
        { "Wholeness Late 19G", (24051593, 1367254168) },
        { "Wholeness Late 20G", (24964954, 1392219122) },
        { "Perfection Early 1G", (7047835, 1399266957) },
        { "Perfection Early 2G", (8927256, 1408194213) },
        { "Perfection Early 3G", (10336824, 1418531037) },
        { "Perfection Early 4G", (12686101, 1431217138) },
        { "Perfection Early 5G", (14095669, 1445312807) },
        { "Perfection Early 6G", (16444946, 1461757753) },
        { "Perfection Early 7G", (17854513, 1479612266) },
        { "Perfection Early 8G", (19733936, 1499346202) },
        { "Perfection Early 9G", (21613358, 1520959560) },
        { "Perfection Early 10G", (23492780, 1544452340) },
        { "Perfection Early 11G", (24902348, 1569354688) },
        { "Perfection Early 12G", (26311914, 1595666602) },
        { "Perfection Early 13G", (28191337, 1623857939) },
        { "Perfection Early 14G", (29131048, 1652988987) },
        { "Perfection Early 15G", (31010470, 1683999457) },
        { "Perfection Early 16G", (32889893, 1716889350) },
        { "Perfection Early 17G", (33829604, 1750718954) },
        { "Perfection Early 18G", (35709026, 1786427980) },
        { "Perfection Early 19G", (37118593, 1823546573) },
        { "Perfection Early 20G", (38528160, 1862074733) },
        { "Perfection Middle 1G", (10361621, 1872436354) },
        { "Perfection Middle 2G", (13124720, 1885561074) },
        { "Perfection Middle 3G", (15197044, 1900758118) },
        { "Perfection Middle 4G", (18650918, 1919409036) },
        { "Perfection Middle 5G", (20723242, 1940132278) },
        { "Perfection Middle 6G", (24177115, 1964309393) },
        { "Perfection Middle 7G", (26249440, 1990558833) },
        { "Perfection Middle 8G", (29012538, 2019571371) },
        { "Perfection Middle 9G", (31775638, 2051347009) },
        { "Perfection Middle 10G", (34538736, 2085885745) },
        { "Perfection Middle 11G", (36611061, 2122496806) },
        { "Perfection Middle 12G", (38683384, 2161180190) },
        { "Perfection Middle 13G", (41446484, 2202626674) },
        { "Perfection Middle 14G", (42828033, 2245454707) },
        { "Perfection Middle 15G", (45591132, 2291045839) },
        { "Perfection Middle 16G", (48354231, 2339400070) },
        { "Perfection Middle 17G", (49735780, 2389135850) },
        { "Perfection Middle 18G", (52498879, 2441634729) },
        { "Perfection Middle 19G", (54571204, 2496205933) },
        { "Perfection Middle 20G", (56643527, 2552849460) },
        { "Perfection Late 1G", (13731909, 2566581369) },
        { "Perfection Late 2G", (17393750, 2583975119) },
        { "Perfection Late 3G", (20140132, 2604115251) },
        { "Perfection Late 4G", (24717435, 2628832686) },
        { "Perfection Late 5G", (27463816, 2656296502) },
        { "Perfection Late 6G", (32041119, 2688337621) },
        { "Perfection Late 7G", (34787501, 2723125122) },
        { "Perfection Late 8G", (38449343, 2761574465) },
        { "Perfection Late 9G", (42111185, 2803685650) },
        { "Perfection Late 10G", (45773027, 2849458677) },
        { "Perfection Late 11G", (48519409, 2897978086) },
        { "Perfection Late 12G", (51265790, 2949243876) },
        { "Perfection Late 13G", (54927633, 3004171509) },
        { "Perfection Late 14G", (56758553, 3060930062) },
        { "Perfection Late 15G", (60420396, 3121350458) },
        { "Perfection Late 16G", (64082238, 3185432696) },
        { "Perfection Late 17G", (65913160, 3251345856) },
        { "Perfection Late 18G", (69575001, 3320920857) },
        { "Perfection Late 19G", (72321383, 3393242240) },
        { "Perfection Late 20G", (75067764, 3468310004) },
        { "Nirvana Early 1G", (17387879, 3485697883) },
        { "Nirvana Early 2G", (22024647, 3507722530) },
        { "Nirvana Early 3G", (25502223, 3533224753) },
        { "Nirvana Early 4G", (31298183, 3564522936) },
        { "Nirvana Early 5G", (34775758, 3599298694) },
        { "Nirvana Early 6G", (40571718, 3639870412) },
        { "Nirvana Early 7G", (44049294, 3683919706) },
        { "Nirvana Early 8G", (48686062, 3732605768) },
        { "Nirvana Early 9G", (53322830, 3785928598) },
        { "Nirvana Early 10G", (57959597, 3843888195) },
        { "Nirvana Early 11G", (61437173, 3905325368) },
        { "Nirvana Early 12G", (64914749, 3970240117) },
        { "Nirvana Early 13G", (69551517, 4039791634) },
        { "Nirvana Early 14G", (71869901, 4111661535) },
        { "Nirvana Early 15G", (76506668, 4188168203) },
        { "Nirvana Early 16G", (81143436, 4269311639) },
        { "Nirvana Early 17G", (83461820, 4352773459) },
        { "Nirvana Early 18G", (88098588, 4440872047) },
        { "Nirvana Early 19G", (91576164, 4532448211) },
        { "Nirvana Early 20G", (95053740, 4627501951) },
        { "Nirvana Middle 1G", (22490444, 4649992395) },
        { "Nirvana Middle 2G", (28487896, 4678480291) },
        { "Nirvana Middle 3G", (32985984, 4711466275) },
        { "Nirvana Middle 4G", (40482799, 4751949074) },
        { "Nirvana Middle 5G", (44980888, 4796929962) },
        { "Nirvana Middle 6G", (52477702, 4849407664) },
        { "Nirvana Middle 7G", (56975791, 4906383455) },
        { "Nirvana Middle 8G", (62973243, 4969356698) },
        { "Nirvana Middle 9G", (68970694, 5038327392) },
        { "Nirvana Middle 10G", (74968146, 5113295538) },
        { "Nirvana Middle 11G", (79466235, 5192761773) },
        { "Nirvana Middle 12G", (83964324, 5276726097) },
        { "Nirvana Middle 13G", (89961775, 5366687872) },
        { "Nirvana Middle 14G", (92960501, 5459648373) },
        { "Nirvana Middle 15G", (98957953, 5558606326) },
        { "Nirvana Middle 16G", (104955405, 5663561731) },
        { "Nirvana Middle 17G", (107954130, 5771515861) },
        { "Nirvana Middle 18G", (113951582, 5885467443) },
        { "Nirvana Middle 19G", (118449671, 6003917114) },
        { "Nirvana Middle 20G", (122947760, 6126864874) },
        { "Nirvana Late 1G", (28752625, 6155617499) },
        { "Nirvana Late 2G", (36419992, 6192037491) },
        { "Nirvana Late 3G", (42170517, 6234208008) },
        { "Nirvana Late 4G", (51754726, 6285962734) },
        { "Nirvana Late 5G", (57505251, 6343467985) },
        { "Nirvana Late 6G", (67089459, 6410557444) },
        { "Nirvana Late 7G", (72839984, 6483397428) },
        { "Nirvana Late 8G", (80507351, 6563904779) },
        { "Nirvana Late 9G", (88174718, 6652079497) },
        { "Nirvana Late 10G", (95842085, 6747921582) },
        { "Nirvana Late 11G", (101592610, 6849514192) },
        { "Nirvana Late 12G", (107343135, 6956857327) },
        { "Nirvana Late 13G", (115010501, 7071867828) },
        { "Nirvana Late 14G", (118844185, 7190712013) },
        { "Nirvana Late 15G", (126511552, 7317223565) },
        { "Nirvana Late 16G", (134178918, 7451402483) },
        { "Nirvana Late 17G", (138012602, 7589415085) },
        { "Nirvana Late 18G", (145679969, 7735095054) },
        { "Nirvana Late 19G", (151430494, 7886525548) },
        { "Nirvana Late 20G", (157181019, 8043706567) },
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

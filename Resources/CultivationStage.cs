using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace OvermortalTools.Resources;

public partial class CultivationStage : Resource
{
    public readonly static List<CultivationStage> AllStages =
    [
        new CultivationStage("Novice", "---", "---", 2),
        new CultivationStage("Connection", "F1", "---", 13),
        new CultivationStage("Connection", "F2", "---", 73),
        new CultivationStage("Connection", "F3", "---", 176),
        new CultivationStage("Connection", "F4", "---", 500),
        new CultivationStage("Connection", "F5", "---", 1000),
        new CultivationStage("Connection", "F6", "---", 1600),
        new CultivationStage("Connection", "F7", "---", 2500),
        new CultivationStage("Connection", "F8", "---", 3000),
        new CultivationStage("Connection", "F9", "---", 3500),
        new CultivationStage("Connection", "F10", "---", 4500),
        new CultivationStage("Foundation", "Early", "1G", 12376),
        new CultivationStage("Foundation", "Early", "2G", 15624),
        new CultivationStage("Foundation", "Middle", "1G", 15708),
        new CultivationStage("Foundation", "Middle", "2G", 19873),
        new CultivationStage("Foundation", "Middle", "3G", 22223),
        new CultivationStage("Foundation", "Late", "1G", 36180),
        new CultivationStage("Foundation", "Late", "2G", 45426),
        new CultivationStage("Foundation", "Late", "3G", 55074),
        new CultivationStage("Foundation", "Late", "4G", 64320),
        new CultivationStage("Virtuoso", "Early", "1G", 39438),
        new CultivationStage("Virtuoso", "Early", "2G", 49517),
        new CultivationStage("Virtuoso", "Early", "3G", 60033),
        new CultivationStage("Virtuoso", "Early", "4G", 70112),
        new CultivationStage("Virtuoso", "Middle", "1G", 60915),
        new CultivationStage("Virtuoso", "Middle", "2G", 77190),
        new CultivationStage("Virtuoso", "Middle", "3G", 93000),
        new CultivationStage("Virtuoso", "Middle", "4G", 108810),
        new CultivationStage("Virtuoso", "Middle", "5G", 125085),
        new CultivationStage("Virtuoso", "Late", "1G", 158500),
        new CultivationStage("Virtuoso", "Late", "2G", 199301),
        new CultivationStage("Virtuoso", "Late", "3G", 241672),
        new CultivationStage("Virtuoso", "Late", "4G", 280905),
        new CultivationStage("Virtuoso", "Late", "5G", 324845),
        new CultivationStage("Virtuoso", "Late", "6G", 364077),
        new CultivationStage("Nascent Soul", "Early", "1G", 183679),
        new CultivationStage("Nascent Soul", "Early", "2G", 230962),
        new CultivationStage("Nascent Soul", "Early", "3G", 280065),
        new CultivationStage("Nascent Soul", "Early", "4G", 325529),
        new CultivationStage("Nascent Soul", "Early", "5G", 376450),
        new CultivationStage("Nascent Soul", "Early", "6G", 421915),
        new CultivationStage("Nascent Soul", "Middle", "1G", 308712),
        new CultivationStage("Nascent Soul", "Middle", "2G", 389749),
        new CultivationStage("Nascent Soul", "Middle", "3G", 470786),
        new CultivationStage("Nascent Soul", "Middle", "4G", 551823),
        new CultivationStage("Nascent Soul", "Middle", "5G", 632859),
        new CultivationStage("Nascent Soul", "Middle", "6G", 710038),
        new CultivationStage("Nascent Soul", "Middle", "7G", 801067),
        new CultivationStage("Nascent Soul", "Late", "1G", 719563),
        new CultivationStage("Nascent Soul", "Late", "2G", 918827),
        new CultivationStage("Nascent Soul", "Late", "3G", 1095950),
        new CultivationStage("Nascent Soul", "Late", "4G", 1305208),
        new CultivationStage("Nascent Soul", "Late", "5G", 1483697),
        new CultivationStage("Nascent Soul", "Late", "6G", 1684499),
        new CultivationStage("Nascent Soul", "Late", "7G", 1862988),
        new CultivationStage("Nascent Soul", "Late", "8G", 2063790),
        new CultivationStage("Incarnation", "Early", "1G", 789653),
        new CultivationStage("Incarnation", "Early", "2G", 1008326),
        new CultivationStage("Incarnation", "Early", "3G", 1202702),
        new CultivationStage("Incarnation", "Early", "4G", 1421375),
        new CultivationStage("Incarnation", "Early", "5G", 1615751),
        new CultivationStage("Incarnation", "Early", "6G", 1834424),
        new CultivationStage("Incarnation", "Early", "7G", 2039156),
        new CultivationStage("Incarnation", "Early", "8G", 2247473),
        new CultivationStage("Incarnation", "Middle", "1G", 1392023),
        new CultivationStage("Incarnation", "Middle", "2G", 1778696),
        new CultivationStage("Incarnation", "Middle", "3G", 2139589),
        new CultivationStage("Incarnation", "Middle", "4G", 2500485),
        new CultivationStage("Incarnation", "Middle", "5G", 2861380),
        new CultivationStage("Incarnation", "Middle", "6G", 3222274),
        new CultivationStage("Incarnation", "Middle", "7G", 3595169),
        new CultivationStage("Incarnation", "Middle", "8G", 3969842),
        new CultivationStage("Incarnation", "Middle", "9G", 4394107),
        new CultivationStage("Incarnation", "Late", "1G", 1483013),
        new CultivationStage("Incarnation", "Late", "2G", 1853766),
        new CultivationStage("Incarnation", "Late", "3G", 2286312),
        new CultivationStage("Incarnation", "Late", "4G", 2595272),
        new CultivationStage("Incarnation", "Late", "5G", 3027818),
        new CultivationStage("Incarnation", "Late", "6G", 3460363),
        new CultivationStage("Incarnation", "Late", "7G", 3769324),
        new CultivationStage("Incarnation", "Late", "8G", 4201870),
        new CultivationStage("Incarnation", "Late", "9G", 4572623),
        new CultivationStage("Incarnation", "Late", "10G", 4943376),
        new CultivationStage("Incarnation", "Late", "11G", 5252337),
        new CultivationStage("Incarnation", "Late", "12G", 5623090),
        new CultivationStage("Incarnation", "Late", "13G", 5932051),
        new CultivationStage("Incarnation", "Late", "14G", 6241012),
        new CultivationStage("Incarnation", "Late", "15G", 6549973),
        new CultivationStage("Voidbreak", "Early", "1G", 1020222),
        new CultivationStage("Voidbreak", "Early", "2G", 1292280),
        new CultivationStage("Voidbreak", "Early", "3G", 1496324),
        new CultivationStage("Voidbreak", "Early", "4G", 1836398),
        new CultivationStage("Voidbreak", "Early", "5G", 2040442),
        new CultivationStage("Voidbreak", "Early", "6G", 2380515),
        new CultivationStage("Voidbreak", "Early", "7G", 2584560),
        new CultivationStage("Voidbreak", "Early", "8G", 2856619),
        new CultivationStage("Voidbreak", "Early", "9G", 3128678),
        new CultivationStage("Voidbreak", "Early", "10G", 3400737),
        new CultivationStage("Voidbreak", "Early", "11G", 3604781),
        new CultivationStage("Voidbreak", "Early", "12G", 3808825),
        new CultivationStage("Voidbreak", "Early", "13G", 4080884),
        new CultivationStage("Voidbreak", "Early", "14G", 4216914),
        new CultivationStage("Voidbreak", "Early", "15G", 4488973),
        new CultivationStage("Voidbreak", "Early", "16G", 4761031),
        new CultivationStage("Voidbreak", "Early", "17G", 4897061),
        new CultivationStage("Voidbreak", "Early", "18G", 5169120),
        new CultivationStage("Voidbreak", "Early", "19G", 5373164),
        new CultivationStage("Voidbreak", "Early", "20G", 5577208),
        new CultivationStage("Voidbreak", "Middle", "1G", 2131409),
        new CultivationStage("Voidbreak", "Middle", "2G", 2699785),
        new CultivationStage("Voidbreak", "Middle", "3G", 3126066),
        new CultivationStage("Voidbreak", "Middle", "4G", 3836536),
        new CultivationStage("Voidbreak", "Middle", "5G", 4262817),
        new CultivationStage("Voidbreak", "Middle", "6G", 4973288),
        new CultivationStage("Voidbreak", "Middle", "7G", 5399569),
        new CultivationStage("Voidbreak", "Middle", "8G", 5967944),
        new CultivationStage("Voidbreak", "Middle", "9G", 6536321),
        new CultivationStage("Voidbreak", "Middle", "10G", 7104696),
        new CultivationStage("Voidbreak", "Middle", "11G", 7530977),
        new CultivationStage("Voidbreak", "Middle", "12G", 7957260),
        new CultivationStage("Voidbreak", "Middle", "13G", 8525635),
        new CultivationStage("Voidbreak", "Middle", "14G", 8809823),
        new CultivationStage("Voidbreak", "Middle", "15G", 9378199),
        new CultivationStage("Voidbreak", "Middle", "16G", 9946574),
        new CultivationStage("Voidbreak", "Middle", "17G", 10230762),
        new CultivationStage("Voidbreak", "Middle", "18G", 10799138),
        new CultivationStage("Voidbreak", "Middle", "19G", 11225420),
        new CultivationStage("Voidbreak", "Middle", "20G", 11651701),
        new CultivationStage("Voidbreak", "Late", "1G", 4615726),
        new CultivationStage("Voidbreak", "Late", "2G", 5846585),
        new CultivationStage("Voidbreak", "Late", "3G", 6769731),
        new CultivationStage("Voidbreak", "Late", "4G", 8308306),
        new CultivationStage("Voidbreak", "Late", "5G", 9231451),
        new CultivationStage("Voidbreak", "Late", "6G", 10770250),
        new CultivationStage("Voidbreak", "Late", "7G", 11693172),
        new CultivationStage("Voidbreak", "Late", "8G", 12924031),
        new CultivationStage("Voidbreak", "Late", "9G", 14154891),
        new CultivationStage("Voidbreak", "Late", "10G", 15385751),
        new CultivationStage("Voidbreak", "Late", "11G", 16308897),
        new CultivationStage("Voidbreak", "Late", "12G", 17232041),
        new CultivationStage("Voidbreak", "Late", "13G", 18462902),
        new CultivationStage("Voidbreak", "Late", "14G", 19078331),
        new CultivationStage("Voidbreak", "Late", "15G", 20309192),
        new CultivationStage("Voidbreak", "Late", "16G", 21540052),
        new CultivationStage("Voidbreak", "Late", "17G", 22155482),
        new CultivationStage("Voidbreak", "Late", "18G", 23386342),
        new CultivationStage("Voidbreak", "Late", "19G", 24309487),
        new CultivationStage("Voidbreak", "Late", "20G", 25232632),
        new CultivationStage("Wholeness", "Early", "1G", 2802204),
        new CultivationStage("Wholeness", "Early", "2G", 3549458),
        new CultivationStage("Wholeness", "Early", "3G", 4109898),
        new CultivationStage("Wholeness", "Early", "4G", 5043967),
        new CultivationStage("Wholeness", "Early", "5G", 5604407),
        new CultivationStage("Wholeness", "Early", "6G", 6538475),
        new CultivationStage("Wholeness", "Early", "7G", 7098915),
        new CultivationStage("Wholeness", "Early", "8G", 7846170),
        new CultivationStage("Wholeness", "Early", "9G", 8593424),
        new CultivationStage("Wholeness", "Early", "10G", 9340679),
        new CultivationStage("Wholeness", "Early", "11G", 9901119),
        new CultivationStage("Wholeness", "Early", "12G", 10461560),
        new CultivationStage("Wholeness", "Early", "13G", 11208814),
        new CultivationStage("Wholeness", "Early", "14G", 11582441),
        new CultivationStage("Wholeness", "Early", "15G", 12329696),
        new CultivationStage("Wholeness", "Early", "16G", 13076950),
        new CultivationStage("Wholeness", "Early", "17G", 13450577),
        new CultivationStage("Wholeness", "Early", "18G", 14197831),
        new CultivationStage("Wholeness", "Early", "19G", 14758272),
        new CultivationStage("Wholeness", "Early", "20G", 15318712),
        new CultivationStage("Wholeness", "Middle", "1G", 3959269),
        new CultivationStage("Wholeness", "Middle", "2G", 5015074),
        new CultivationStage("Wholeness", "Middle", "3G", 5806927),
        new CultivationStage("Wholeness", "Middle", "4G", 7126683),
        new CultivationStage("Wholeness", "Middle", "5G", 7918537),
        new CultivationStage("Wholeness", "Middle", "6G", 9238293),
        new CultivationStage("Wholeness", "Middle", "7G", 10030147),
        new CultivationStage("Wholeness", "Middle", "8G", 11085952),
        new CultivationStage("Wholeness", "Middle", "9G", 12141757),
        new CultivationStage("Wholeness", "Middle", "10G", 13197562),
        new CultivationStage("Wholeness", "Middle", "11G", 13989415),
        new CultivationStage("Wholeness", "Middle", "12G", 14781270),
        new CultivationStage("Wholeness", "Middle", "13G", 15837074),
        new CultivationStage("Wholeness", "Middle", "14G", 16364976),
        new CultivationStage("Wholeness", "Middle", "15G", 17420782),
        new CultivationStage("Wholeness", "Middle", "16G", 18476587),
        new CultivationStage("Wholeness", "Middle", "17G", 19004489),
        new CultivationStage("Wholeness", "Middle", "18G", 20060294),
        new CultivationStage("Wholeness", "Middle", "19G", 20852147),
        new CultivationStage("Wholeness", "Middle", "20G", 21644001),
        new CultivationStage("Wholeness", "Late", "1G", 4566759),
        new CultivationStage("Wholeness", "Late", "2G", 5784560),
        new CultivationStage("Wholeness", "Late", "3G", 6697912),
        new CultivationStage("Wholeness", "Late", "4G", 8220165),
        new CultivationStage("Wholeness", "Late", "5G", 9133517),
        new CultivationStage("Wholeness", "Late", "6G", 10655769),
        new CultivationStage("Wholeness", "Late", "7G", 11569121),
        new CultivationStage("Wholeness", "Late", "8G", 12786923),
        new CultivationStage("Wholeness", "Late", "9G", 14004725),
        new CultivationStage("Wholeness", "Late", "10G", 15222528),
        new CultivationStage("Wholeness", "Late", "11G", 16135879),
        new CultivationStage("Wholeness", "Late", "12G", 17049231),
        new CultivationStage("Wholeness", "Late", "13G", 18267033),
        new CultivationStage("Wholeness", "Late", "14G", 18875934),
        new CultivationStage("Wholeness", "Late", "15G", 20093737),
        new CultivationStage("Wholeness", "Late", "16G", 21311538),
        new CultivationStage("Wholeness", "Late", "17G", 21920440),
        new CultivationStage("Wholeness", "Late", "18G", 23138242),
        new CultivationStage("Wholeness", "Late", "19G", 24051593),
        new CultivationStage("Wholeness", "Late", "20G", 24964954),
        new CultivationStage("Perfection", "Early", "1G", 7047835),
        new CultivationStage("Perfection", "Early", "2G", 8927256),
        new CultivationStage("Perfection", "Early", "3G", 10336824),
        new CultivationStage("Perfection", "Early", "4G", 12686101),
        new CultivationStage("Perfection", "Early", "5G", 14095669),
        new CultivationStage("Perfection", "Early", "6G", 16444946),
        new CultivationStage("Perfection", "Early", "7G", 17854513),
        new CultivationStage("Perfection", "Early", "8G", 19733936),
        new CultivationStage("Perfection", "Early", "9G", 21613358),
        new CultivationStage("Perfection", "Early", "10G", 23492780),
        new CultivationStage("Perfection", "Early", "11G", 24902348),
        new CultivationStage("Perfection", "Early", "12G", 26311914),
        new CultivationStage("Perfection", "Early", "13G", 28191337),
        new CultivationStage("Perfection", "Early", "14G", 29131048),
        new CultivationStage("Perfection", "Early", "15G", 31010470),
        new CultivationStage("Perfection", "Early", "16G", 32889893),
        new CultivationStage("Perfection", "Early", "17G", 33829604),
        new CultivationStage("Perfection", "Early", "18G", 35709026),
        new CultivationStage("Perfection", "Early", "19G", 37118593),
        new CultivationStage("Perfection", "Early", "20G", 38528160),
        new CultivationStage("Perfection", "Middle", "1G", 10361621),
        new CultivationStage("Perfection", "Middle", "2G", 13124720),
        new CultivationStage("Perfection", "Middle", "3G", 15197044),
        new CultivationStage("Perfection", "Middle", "4G", 18650918),
        new CultivationStage("Perfection", "Middle", "5G", 20723242),
        new CultivationStage("Perfection", "Middle", "6G", 24177115),
        new CultivationStage("Perfection", "Middle", "7G", 26249440),
        new CultivationStage("Perfection", "Middle", "8G", 29012538),
        new CultivationStage("Perfection", "Middle", "9G", 31775638),
        new CultivationStage("Perfection", "Middle", "10G", 34538736),
        new CultivationStage("Perfection", "Middle", "11G", 36611061),
        new CultivationStage("Perfection", "Middle", "12G", 38683384),
        new CultivationStage("Perfection", "Middle", "13G", 41446484),
        new CultivationStage("Perfection", "Middle", "14G", 42828033),
        new CultivationStage("Perfection", "Middle", "15G", 45591132),
        new CultivationStage("Perfection", "Middle", "16G", 48354231),
        new CultivationStage("Perfection", "Middle", "17G", 49735780),
        new CultivationStage("Perfection", "Middle", "18G", 52498879),
        new CultivationStage("Perfection", "Middle", "19G", 54571204),
        new CultivationStage("Perfection", "Middle", "20G", 56643527),
        new CultivationStage("Perfection", "Late", "1G", 13731909),
        new CultivationStage("Perfection", "Late", "2G", 17393750),
        new CultivationStage("Perfection", "Late", "3G", 20140132),
        new CultivationStage("Perfection", "Late", "4G", 24717435),
        new CultivationStage("Perfection", "Late", "5G", 27463816),
        new CultivationStage("Perfection", "Late", "6G", 32041119),
        new CultivationStage("Perfection", "Late", "7G", 34787501),
        new CultivationStage("Perfection", "Late", "8G", 38449343),
        new CultivationStage("Perfection", "Late", "9G", 42111185),
        new CultivationStage("Perfection", "Late", "10G", 45773027),
        new CultivationStage("Perfection", "Late", "11G", 48519409),
        new CultivationStage("Perfection", "Late", "12G", 51265790),
        new CultivationStage("Perfection", "Late", "13G", 54927633),
        new CultivationStage("Perfection", "Late", "14G", 56758553),
        new CultivationStage("Perfection", "Late", "15G", 60420396),
        new CultivationStage("Perfection", "Late", "16G", 64082238),
        new CultivationStage("Perfection", "Late", "17G", 65913160),
        new CultivationStage("Perfection", "Late", "18G", 69575001),
        new CultivationStage("Perfection", "Late", "19G", 72321383),
        new CultivationStage("Perfection", "Late", "20G", 75067764),
        new CultivationStage("Nirvana", "Early", "1G", 17387879),
        new CultivationStage("Nirvana", "Early", "2G", 22024647),
        new CultivationStage("Nirvana", "Early", "3G", 25502223),
        new CultivationStage("Nirvana", "Early", "4G", 31298183),
        new CultivationStage("Nirvana", "Early", "5G", 34775758),
        new CultivationStage("Nirvana", "Early", "6G", 40571718),
        new CultivationStage("Nirvana", "Early", "7G", 44049294),
        new CultivationStage("Nirvana", "Early", "8G", 48686062),
        new CultivationStage("Nirvana", "Early", "9G", 53322830),
        new CultivationStage("Nirvana", "Early", "10G", 57959597),
        new CultivationStage("Nirvana", "Early", "11G", 61437173),
        new CultivationStage("Nirvana", "Early", "12G", 64914749),
        new CultivationStage("Nirvana", "Early", "13G", 69551517),
        new CultivationStage("Nirvana", "Early", "14G", 71869901),
        new CultivationStage("Nirvana", "Early", "15G", 76506668),
        new CultivationStage("Nirvana", "Early", "16G", 81143436),
        new CultivationStage("Nirvana", "Early", "17G", 83461820),
        new CultivationStage("Nirvana", "Early", "18G", 88098588),
        new CultivationStage("Nirvana", "Early", "19G", 91576164),
        new CultivationStage("Nirvana", "Early", "20G", 95053740),
        new CultivationStage("Nirvana", "Middle", "1G", 22490444),
        new CultivationStage("Nirvana", "Middle", "2G", 28487896),
        new CultivationStage("Nirvana", "Middle", "3G", 32985984),
        new CultivationStage("Nirvana", "Middle", "4G", 40482799),
        new CultivationStage("Nirvana", "Middle", "5G", 44980888),
        new CultivationStage("Nirvana", "Middle", "6G", 52477702),
        new CultivationStage("Nirvana", "Middle", "7G", 56975791),
        new CultivationStage("Nirvana", "Middle", "8G", 62973243),
        new CultivationStage("Nirvana", "Middle", "9G", 68970694),
        new CultivationStage("Nirvana", "Middle", "10G", 74968146),
        new CultivationStage("Nirvana", "Middle", "11G", 79466235),
        new CultivationStage("Nirvana", "Middle", "12G", 83964324),
        new CultivationStage("Nirvana", "Middle", "13G", 89961775),
        new CultivationStage("Nirvana", "Middle", "14G", 92960501),
        new CultivationStage("Nirvana", "Middle", "15G", 98957953),
        new CultivationStage("Nirvana", "Middle", "16G", 104955405),
        new CultivationStage("Nirvana", "Middle", "17G", 107954130),
        new CultivationStage("Nirvana", "Middle", "18G", 113951582),
        new CultivationStage("Nirvana", "Middle", "19G", 118449671),
        new CultivationStage("Nirvana", "Middle", "20G", 122947760),
        new CultivationStage("Nirvana", "Late", "1G", 28752625),
        new CultivationStage("Nirvana", "Late", "2G", 36419992),
        new CultivationStage("Nirvana", "Late", "3G", 42170517),
        new CultivationStage("Nirvana", "Late", "4G", 51754726),
        new CultivationStage("Nirvana", "Late", "5G", 57505251),
        new CultivationStage("Nirvana", "Late", "6G", 67089459),
        new CultivationStage("Nirvana", "Late", "7G", 72839984),
        new CultivationStage("Nirvana", "Late", "8G", 80507351),
        new CultivationStage("Nirvana", "Late", "9G", 88174718),
        new CultivationStage("Nirvana", "Late", "10G", 95842085),
        new CultivationStage("Nirvana", "Late", "11G", 101592610),
        new CultivationStage("Nirvana", "Late", "12G", 107343135),
        new CultivationStage("Nirvana", "Late", "13G", 115010501),
        new CultivationStage("Nirvana", "Late", "14G", 118844185),
        new CultivationStage("Nirvana", "Late", "15G", 126511552),
        new CultivationStage("Nirvana", "Late", "16G", 134178918),
        new CultivationStage("Nirvana", "Late", "17G", 138012602),
        new CultivationStage("Nirvana", "Late", "18G", 145679969),
        new CultivationStage("Nirvana", "Late", "19G", 151430494),
        new CultivationStage("Nirvana", "Late", "20G", 157181019),
        // new CultivationStage("Celestial", "Early", "1G", 43853988),
        // new CultivationStage("Celestial", "Early", "2G", 55548385),
        // new CultivationStage("Celestial", "Early", "3G", 64319183),
        // new CultivationStage("Celestial", "Early", "4G", 78937179),
        // new CultivationStage("Celestial", "Early", "5G", 87707976),
        // new CultivationStage("Celestial", "Early", "6G", 102325972),
        // new CultivationStage("Celestial", "Early", "7G", 111096770),
        // new CultivationStage("Celestial", "Early", "8G", 122791167),
        // new CultivationStage("Celestial", "Early", "9G", 134485564),
        // new CultivationStage("Celestial", "Early", "10G", 146179961),
        // new CultivationStage("Celestial", "Early", "11G", 154950758),
        // new CultivationStage("Celestial", "Early", "12G", 163721556),
        // new CultivationStage("Celestial", "Early", "13G", 175415953),
        // new CultivationStage("Celestial", "Early", "14G", 181263151),
        // new CultivationStage("Celestial", "Early", "15G", 192957548),
        // new CultivationStage("Celestial", "Early", "16G", 204651945),
        // new CultivationStage("Celestial", "Early", "17G", 210499143),
        // new CultivationStage("Celestial", "Early", "18G", 222193540),
        // new CultivationStage("Celestial", "Early", "19G", 230964338),
        // new CultivationStage("Celestial", "Early", "20G", 239735135),
        // new CultivationStage("Celestial", "Middle", "1G", 57847086),
        // new CultivationStage("Celestial", "Middle", "2G", 73272975),
        // new CultivationStage("Celestial", "Middle", "3G", 84842392),
        // new CultivationStage("Celestial", "Middle", "4G", 104124754),
        // new CultivationStage("Celestial", "Middle", "5G", 115694171),
        // new CultivationStage("Celestial", "Middle", "6G", 134976533),
        // new CultivationStage("Celestial", "Middle", "7G", 146545950),
        // new CultivationStage("Celestial", "Middle", "8G", 161971840),
        // new CultivationStage("Celestial", "Middle", "9G", 177397729),
        // new CultivationStage("Celestial", "Middle", "10G", 192823618),
        // new CultivationStage("Celestial", "Middle", "11G", 204393036),
        // new CultivationStage("Celestial", "Middle", "12G", 215962453),
        // new CultivationStage("Celestial", "Middle", "13G", 231388342),
        // new CultivationStage("Celestial", "Middle", "14G", 239101287),
        // new CultivationStage("Celestial", "Middle", "15G", 254527176),
        // new CultivationStage("Celestial", "Middle", "16G", 269953066),
        // new CultivationStage("Celestial", "Middle", "17G", 277666011),
        // new CultivationStage("Celestial", "Middle", "18G", 293091900),
        // new CultivationStage("Celestial", "Middle", "19G", 304661317),
        // new CultivationStage("Celestial", "Middle", "20G", 316230734),
        // new CultivationStage("Celestial", "Late", "1G", 78231250),
        // new CultivationStage("Celestial", "Late", "2G", 99092917),
        // new CultivationStage("Celestial", "Late", "3G", 114739167),
        // new CultivationStage("Celestial", "Late", "4G", 140816250),
        // new CultivationStage("Celestial", "Late", "5G", 156462500),
        // new CultivationStage("Celestial", "Late", "6G", 182539583),
        // new CultivationStage("Celestial", "Late", "7G", 198185833),
        // new CultivationStage("Celestial", "Late", "8G", 219047500),
        // new CultivationStage("Celestial", "Late", "9G", 239909166),
        // new CultivationStage("Celestial", "Late", "10G", 260770833),
        // new CultivationStage("Celestial", "Late", "11G", 276417083),
        // new CultivationStage("Celestial", "Late", "12G", 292063333),
        // new CultivationStage("Celestial", "Late", "13G", 312925000),
        // new CultivationStage("Celestial", "Late", "14G", 323355833),
        // new CultivationStage("Celestial", "Late", "15G", 344217500),
        // new CultivationStage("Celestial", "Late", "16G", 365079166),
        // new CultivationStage("Celestial", "Late", "17G", 375510000),
        // new CultivationStage("Celestial", "Late", "18G", 396371666),
        // new CultivationStage("Celestial", "Late", "19G", 412017916),
        // new CultivationStage("Celestial", "Late", "20G", 427664166),
        // new CultivationStage("Eternal", "Early", "1G", 126409270),
        // new CultivationStage("Eternal", "Early", "2G", 160118408),
        // new CultivationStage("Eternal", "Early", "3G", 185400262),
        // new CultivationStage("Eternal", "Early", "4G", 227536685),
        // new CultivationStage("Eternal", "Early", "5G", 252818539),
        // new CultivationStage("Eternal", "Early", "6G", 294954962),
        // new CultivationStage("Eternal", "Early", "7G", 320236816),
        // new CultivationStage("Eternal", "Early", "8G", 353945955),
        // new CultivationStage("Eternal", "Early", "9G", 387655093),
        // new CultivationStage("Eternal", "Early", "10G", 421364232),
        // new CultivationStage("Eternal", "Early", "11G", 446646086),
        // new CultivationStage("Eternal", "Early", "12G", 471927940),
        // new CultivationStage("Eternal", "Early", "13G", 505637078),
        // new CultivationStage("Eternal", "Early", "14G", 522491648),
        // new CultivationStage("Eternal", "Early", "15G", 556200786),
        // new CultivationStage("Eternal", "Early", "16G", 589909925),
        // new CultivationStage("Eternal", "Early", "17G", 606764494),
        // new CultivationStage("Eternal", "Early", "18G", 640473633),
        // new CultivationStage("Eternal", "Early", "19G", 665755487),
        // new CultivationStage("Eternal", "Early", "20G", 691037340),
        // new CultivationStage("Eternal", "Middle", "1G", 147741734),
        // new CultivationStage("Eternal", "Middle", "2G", 187139529),
        // new CultivationStage("Eternal", "Middle", "3G", 216687876),
        // new CultivationStage("Eternal", "Middle", "4G", 265935121),
        // new CultivationStage("Eternal", "Middle", "5G", 295483467),
        // new CultivationStage("Eternal", "Middle", "6G", 344730712),
        // new CultivationStage("Eternal", "Middle", "7G", 374279059),
        // new CultivationStage("Eternal", "Middle", "8G", 413676854),
        // new CultivationStage("Eternal", "Middle", "9G", 453074650),
        // new CultivationStage("Eternal", "Middle", "10G", 492472446),
        // new CultivationStage("Eternal", "Middle", "11G", 522020792),
        // new CultivationStage("Eternal", "Middle", "12G", 551569139),
        // new CultivationStage("Eternal", "Middle", "13G", 590966935),
        // new CultivationStage("Eternal", "Middle", "14G", 610665833),
        // new CultivationStage("Eternal", "Middle", "15G", 650063628),
        // new CultivationStage("Eternal", "Middle", "16G", 689461424),
        // new CultivationStage("Eternal", "Middle", "17G", 709160322),
        // new CultivationStage("Eternal", "Middle", "18G", 748558118),
        // new CultivationStage("Eternal", "Middle", "19G", 778106464),
        // new CultivationStage("Eternal", "Middle", "20G", 807654811),
        // new CultivationStage("Eternal", "Late", "1G", 192195528),
        // new CultivationStage("Eternal", "Late", "2G", 243447669),
        // new CultivationStage("Eternal", "Late", "3G", 281886774),
        // new CultivationStage("Eternal", "Late", "4G", 345951950),
        // new CultivationStage("Eternal", "Late", "5G", 384391056),
        // new CultivationStage("Eternal", "Late", "6G", 448456232),
        // new CultivationStage("Eternal", "Late", "7G", 486895338),
        // new CultivationStage("Eternal", "Late", "8G", 538147478),
        // new CultivationStage("Eternal", "Late", "9G", 589399619),
        // new CultivationStage("Eternal", "Late", "10G", 640651760),
        // new CultivationStage("Eternal", "Late", "11G", 679090866),
        // new CultivationStage("Eternal", "Late", "12G", 717529971),
        // new CultivationStage("Eternal", "Late", "13G", 768782112),
        // new CultivationStage("Eternal", "Late", "14G", 794408182),
        // new CultivationStage("Eternal", "Late", "15G", 845660323),
        // new CultivationStage("Eternal", "Late", "16G", 896912464),
        // new CultivationStage("Eternal", "Late", "17G", 922538534),
        // new CultivationStage("Eternal", "Late", "18G", 973790675),
        // new CultivationStage("Eternal", "Late", "19G", 1012229781),
        // new CultivationStage("Eternal", "Late", "20G", 1050668886),
    ];
    
    public static CultivationStage GetStage(string majorRealm, string minorRealm, string stage) =>
        AllStages
            .Where(s => s.MajorRealm == majorRealm)
            .Where(s => s.MinorRealm == minorRealm)
            .Where(s => s.Stage == stage)
            .FirstOrDefault();

    public static List<string> MajorRealms => [.. AllStages.Select(s => s.MajorRealm).Distinct()];

    public static List<string> MinorRealms(string majorRealm) => [.. AllStages.Where(s => s.MajorRealm == majorRealm).Select(s => s.MinorRealm).Distinct()];

    public static List<string> Stages(string majorRealm, string minorRealm) =>
        [.. AllStages.Where(s => s.MajorRealm == majorRealm && s.MinorRealm == minorRealm).Select(s => s.Stage).Distinct()];

    public readonly static Dictionary<string, long> MajorRealmXp = GetMajorRealmXp();
    private static Dictionary<string, long> GetMajorRealmXp()
    {
        var result = new Dictionary<string, long>();

        foreach (var realm in MajorRealms)
        {
            var values = AllStages.Where(s => s.MajorRealm == realm).Select(s => s.XPRequired);
        }

        return result;
    }

    public readonly static Dictionary<(string, string), long> MinorRealmXp = GetMinorRealmXp();
    private static Dictionary<(string, string), long> GetMinorRealmXp()
    {
        var result = new Dictionary<(string,string) , long>();

        foreach (var major in MajorRealms)
        {
            foreach (var minor in MinorRealms(major))
            {
                result.Add(
                    (major, minor),
                    AllStages
                        .Where(s => s.MajorRealm == major && s.MinorRealm == minor)
                        .Select(s => s.XPRequired).Sum());
            }
        }

        return result;
    }

    public static long GetTotalXpToCompletion(string majorRealm)
    {
        long xp = 0;
        foreach (var kvp in MajorRealms)
        {
            xp += MajorRealmXp[kvp];
            if (kvp == majorRealm) break;
        }
        return xp;
    }

    [Export] public string MajorRealm { get; set; }
    [Export] public string MinorRealm { get; set; }
    [Export] public string Stage { get; set; }
    [Export] public long XPRequired { get; set; }

    public CultivationStage()
    {
    }

    public CultivationStage(string major, string minor, string stage, long xp)
    {
        MajorRealm = major;
        MinorRealm = minor;
        Stage = stage;
        XPRequired = xp;
    }

    /// <summary>
    /// Returns the value of xp accrued for this stage based on the percent completed.
    /// </summary>
    /// <param name="percentComplete"></param>
    /// <returns></returns>
    public long GetCurrentXp(float percentComplete) => (long)Math.Floor(XPRequired * percentComplete);

    /// <summary>
    /// Returns the amount of XP required to get TO this stage, but does not include this stage.
    /// Returns 0 if the stage is not in the AllStages list.
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static long GetXpUntil(CultivationStage stage)
    {
        if (!AllStages.Contains(stage)) return 0;

        var index = AllStages.IndexOf(stage);

        return AllStages.Take(index).Select(s => s.XPRequired).Sum();
    }

    /// <summary>
    /// Returns the total amount of XP required to complete the given stage, or 0 if the stage is not in the AllStages list.
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static long GetXpToComplete(CultivationStage stage)
    {
        if (!AllStages.Contains(stage)) return 0;

        var index = AllStages.IndexOf(stage);

        return AllStages.Take(index).Select(s => s.XPRequired).Sum() + stage.XPRequired;
    }

    /// <summary>
    /// Returns the total amount of XP accumulated based on the percent through this stage.  Includes XP for all previous stages.
    /// </summary>
    /// <param name="percentComplete"></param>
    /// <returns></returns>
    public long GetTotalXp(float percentComplete) => GetXpUntil(this) + GetCurrentXp(percentComplete);

    public override string ToString() => $"{MajorRealm} {MinorRealm} {Stage}";
}

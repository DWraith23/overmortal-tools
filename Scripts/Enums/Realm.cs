using System.Collections.Generic;

namespace OvermortalTools.Scripts.Enums;

public static class Realm
{
        public enum Classification
        {
                Novice,
                Connection,
                Foundation,
                Virtuoso,
                NascentSoul,
                Incarnation,
                Voidbreak,
                Wholeness,
                Perfection,
                Nirvana,
                Celestial,
                Eternal,
        }

        public static readonly Dictionary<Classification, string> Names = new()
        {
                { Classification.Novice, "Novice" },
                { Classification.Connection, "Connection" },
                { Classification.Foundation, "Foundation" },
                { Classification.Virtuoso, "Virtuoso" },
                { Classification.NascentSoul, "Nascent Soul" },
                { Classification.Incarnation, "Incarnation" },
                { Classification.Voidbreak, "Voidbreak" },
                { Classification.Wholeness, "Wholeness" },
                { Classification.Perfection, "Perfection" },
                { Classification.Nirvana, "Nirvana" },
                { Classification.Celestial, "Celestial" },
                { Classification.Eternal, "Eternal" },
        };

        public static List<string> NamesList =>
        [
                "Novice",
                "Connection",
                "Foundation",
                "Virtuoso",
                "Nascent Soul",
                "Incarnation",
                "Voidbreak",
                "Wholeness",
                "Perfection",
                "Nirvana",
                "Celestial",
                "Eternal",
        ];
}

using System;

namespace RpgMapEditor.Modules.Utilities
{
    public enum ItemSlot
    {
        None,
        LeftHand,
        RightHand,
        Armor,
        Helmet,
        Legs,
        Bag,
        InsideBag
    }

    class Utility
    {
        public static int MinZ = 0;
        public static int MaxZ = 14;
        public static int GroundZ = 7;

        public static int MaxX = 3072;
        public static int MaxY = 3072;

        public static int GameX = 8;
        public static int GameY = 6;

        public static int ScreenX = 14;
        public static int ScreenY = 10;

        public static int MaxZOrder = 15;

        public static Coordinates CenterCoordinates = new Coordinates(Coordinates.Step * ScreenX, Coordinates.Step * ScreenY); // X: 8 Y: 6

        public static int ExperienceNeededForLevel(double level)
        {
            double neededExp = ((Math.Pow(level, 2) * 450) + 450 - (350 * (level / 2) * 4) - ((Math.Pow(level, 2) - 1) * 200) + (Math.Pow(level, 2) * 50) - ((Math.Pow(level, 2) + 2) * 50));
            return (int)neededExp;
        }
        public static int ManaSpentNeededForMagicStrength(double level)
        {
            double neededExp = level * 100 + (Math.Pow(level, 2) * 50);
            return (int)neededExp;
        }
    }
}

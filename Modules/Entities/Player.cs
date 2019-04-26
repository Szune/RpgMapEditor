using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Entities
{
    public class Player : Creature
    {

        public Player()
        {

        }

        public Player(string name)
        {
            Name = name;
            WalkState = "Standing";
            WalkDirection = "S";
        }

        public bool IsExperienceEnoughToLevelUp()
        {
            if (Experience >= Utility.ExperienceNeededForLevel(Level))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

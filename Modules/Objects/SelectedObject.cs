using RpgMapEditor.Modules.Entities;

namespace RpgMapEditor.Modules.Objects
{
    public class SelectedObject
    {
        public SpriteCategory SelectedType;
        public Tile SelectedTile;
        public Creature SelectedCreature;
        public Item SelectedItem;
        public bool IsSet;

        public SelectedObject()
        {
            IsSet = false;
        }

        public SelectedObject(SpriteCategory type, Tile selectedTile = null, Creature selectedCreature = null, Item selectedItem = null)
        {
            SelectedType = type;
            SelectedTile = selectedTile;
            SelectedCreature = selectedCreature;
            SelectedItem = selectedItem;
            IsSet = true;
        }

    }
}

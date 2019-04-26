using System.Collections.Generic;
using RpgMapEditor.Modules.Entities;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Objects
{
    public class SelectedObjects
    {
        public List<Tile> SelectedTiles = new List<Tile>();
        public Creature SelectedCreature = new Creature();
        public List<Item> SelectedItems = new List<Item>();
        public SpriteCategory SelectedType = SpriteCategory.None;
        public Coordinates StartPos = new Coordinates();
        public bool IsSet;

        public SelectedObjects()
        {
            IsSet = false;
        }

        public SelectedObjects(List<Tile> selectedTiles = null, Creature selectedCreatures = null, List<Item> selectedItems = null)
        {
            SelectedTiles = selectedTiles;
            SelectedCreature = selectedCreatures;
            SelectedItems = selectedItems;
            IsSet = true;
        }
    }
}

using System.Collections.Generic;
using RpgMapEditor.Modules.Entities;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor
{
    /* Used for undo/redo */

    public class ChangeAction
    {
        public Coordinates Pos;
        public MapAction MapAction;
        public SpriteObject Sprite;
        public Size TileAmount;
        public int Zorder;
        public List<Tile> TilesChanged;
        public List<Creature> CreaturesChanged;
        public List<Item> ItemsChanged;
        public SpriteCategory Category;

        public ChangeAction()
        {

        }

        public ChangeAction(Coordinates pos, int zorder, MapAction mapAction, SpriteObject sprite, Size tileAmount, SpriteCategory category, List<Tile> tilesChanged = null, List<Creature> creaturesChanged = null, List<Item> itemsChanged = null)
        {
            Category = category;
            Pos = pos;
            MapAction = mapAction;
            Sprite = sprite;
            TileAmount = tileAmount;
            Zorder = zorder;
            TilesChanged = tilesChanged;
            CreaturesChanged = creaturesChanged;
            ItemsChanged = itemsChanged;
        }
    }
}

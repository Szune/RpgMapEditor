using System.Collections.Generic;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Entities
{
    public class Tile : Entity
    {
        public bool Walkable;
        public int ZOrder = 0;
        public List<Tile> Zorder = new List<Tile>();
        public int MaxZOrder = 0;
        public Coordinates DrawPosition;
        public bool MovePlayer;
        public Coordinates RelativeMovePosition;
        public Coordinates AbsoluteMovePosition;
        public List<Item> ItemsOnTile;
        public int ExternalThingsBlockingTile;
        public bool WasReplaced;
        public SpriteObject Sprite;

        public Tile() { Id = -1; ZOrder = -1; }
        public Tile(string name, SpriteObject sprite, Coordinates pos, int tileId, bool visible = true, bool walkable = true, int zorder = 0, bool wasReplaced = false)
        {
            SpriteId = sprite.Id;
            Name = name;
            Id = tileId;
            Position = pos;
            Sprite = sprite;
            Visible = visible;
            Walkable = walkable;
            Type = EntityType.TileEntity;
            ZOrder = zorder;
            WasReplaced = wasReplaced;
        }
    }
}

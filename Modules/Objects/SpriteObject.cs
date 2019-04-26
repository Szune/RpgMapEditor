using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgMapEditor.Modules.Objects
{
    public class SpriteObject
    {
        public int Id;
        public string SpriteName;
        public Texture2D Sprite;
        public Rectangle SpritesheetPosition;
        public bool LoadFromSpritesheet;
        public int SpritesheetId;
        public SpriteCategory Category;
        public string ListCategory;
        public bool Walkthrough;
        public string SpriteCollection;
        public bool Show;
        public string RandomizeCategory; // Used for tiles that are similar enough that they can be interchanged for more random mapping

        public SpriteObject()
        {
            Id = -1;
        }

        public SpriteObject(Texture2D sprite, int id, SpriteCategory category, string spriteName, bool walkthrough, string randomizeCategory = null, string spriteCollection = null, bool show = true, string tileCategory = null)
        {
            Sprite = sprite;
            Id = id;
            Category = category;
            SpriteName = spriteName;
            Walkthrough = walkthrough;
            LoadFromSpritesheet = false;
            SpritesheetId = -1;
            SpriteCollection = spriteCollection;
            Show = show;
            ListCategory = tileCategory;
        }

        public SpriteObject(int id, SpriteCategory category, string spriteName, bool walkthrough, Rectangle spritesheetPosition, int spritesheetId, string randomizeCategory = null, string spriteCollection = null, bool show = true, string tileCategory = null)
        {
            Id = id;
            Category = category;
            SpriteName = spriteName;
            Walkthrough = walkthrough;
            SpritesheetPosition = spritesheetPosition;
            SpritesheetId = spritesheetId;
            LoadFromSpritesheet = true;
            RandomizeCategory = randomizeCategory;
            SpriteCollection = spriteCollection;
            Show = show;
            ListCategory = tileCategory;
        }
    }

    public enum SpriteCategory
    {
        Tiles,
        Creatures,
        Items,
        Spells,
        Ui,
        None
    }
}

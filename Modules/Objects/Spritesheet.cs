using Microsoft.Xna.Framework.Graphics;

namespace RpgMapEditor.Modules.Objects
{
    public class Spritesheet
    {
        public int Id;
        public string Name;
        public Texture2D Texture;
        public SpriteCategory Category;

        public Spritesheet()
        {
            Id = -1;
        }

        public Spritesheet(int id, string name, Texture2D texture, SpriteCategory category)
        {
            Id = id;
            Name = name;
            Texture = texture;
            Category = category;
        }
    }
}

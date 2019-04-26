using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor
{
    public static class Sprite
    {
        public static void Draw(SpriteBatch sb, SpriteObject sprite, Rectangle destinationRectangle, Color color)
        {

            if (!sprite.LoadFromSpritesheet)
            {
                if (destinationRectangle.Width == -1 || destinationRectangle.Height == -1)
                {
                    destinationRectangle.Width = sprite.Sprite.Width;
                    destinationRectangle.Height = sprite.Sprite.Height;
                }

                sb.Draw(sprite.Sprite, destinationRectangle, color);
            }
            else
            {
                if (destinationRectangle.Width == -1 || destinationRectangle.Height == -1)
                {
                    destinationRectangle.Width = sprite.SpritesheetPosition.Width;
                    destinationRectangle.Height = sprite.SpritesheetPosition.Height;
                }

                Texture2D spritesheet = Storage.Instance.GetSpritesheetById(sprite.SpritesheetId, sprite.Category).Texture;
                sb.Draw(spritesheet, destinationRectangle, sprite.SpritesheetPosition, color);
            }
        }

        public static Size GetSize(SpriteObject sprite)
        {
            Size spriteSize = new Size();

            if (!sprite.LoadFromSpritesheet)
            {
                spriteSize = new Size(sprite.Sprite.Width, sprite.Sprite.Height);
            }
            else
            {
                spriteSize = new Size(sprite.SpritesheetPosition.Width, sprite.SpritesheetPosition.Height);
            }

            return spriteSize;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class ProgressBar : Control
    {
        public Texture2D Sprite;
        public Texture2D BackgroundSprite;
        public int BorderSize;
        public double ProgressPercent = 1;

        public ProgressBar()
        {
            Id = -1;
        }

        public ProgressBar(int id, string name, Texture2D sprite, Texture2D backgroundSprite, int width, int height, Vector2 position, int borderSize, bool visible = false)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            BackgroundSprite = backgroundSprite;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Visible = visible;
            BorderSize = borderSize;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(BackgroundSprite, new Rectangle((int)OffsetPosition.X, (int)OffsetPosition.Y, Width + (BorderSize * 2), Height + (BorderSize * 2)), Color.White);
                spriteBatch.Draw(Sprite, new Rectangle((int)OffsetPosition.X + BorderSize, (int)OffsetPosition.Y + BorderSize, (int)((double)Width * ProgressPercent), Height), Color.White);
            }
        }

        public void DrawScaled(SpriteBatch spriteBatch, SpriteFont font, Vector2 scale)
        {
            if (Visible)
            {
                int scaleX = (int)(scale.X * OffsetPosition.X);
                int scaleY = (int)(scale.Y * OffsetPosition.Y);
                int scaleWidth = (int)(scale.X * (float)(Width + (BorderSize * 2)));
                int scaleHeight = (int)(scale.Y * (float)(Height + (BorderSize * 2)));
                spriteBatch.Draw(BackgroundSprite, new Rectangle(scaleX, scaleY, scaleWidth, scaleHeight), Color.White);
                spriteBatch.Draw(Sprite, new Rectangle(scaleX + BorderSize, scaleY + BorderSize, (int)((double)scaleWidth * ProgressPercent) - (BorderSize * 2), scaleHeight - (BorderSize * 2)), Color.White);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class Label : Control
    {
        public string Text;
        public bool BigFont;

        public Label()
        {
            Id = -1;
        }
        public Label(int id, string name, int width, int height, Vector2 position, string text = "", bool visible = false, bool bigFont = false)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Text = text;
            Visible = visible;
            BigFont = bigFont;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible && Text.Length > 0)
            {
                //_spriteBatch.DrawString(font, Text, Position, Color.White);
                try
                {
                    Vector2 position = new Vector2(0, 0);

                    if (ParentForm.Id == -1 && ParentPanel.Id == -1)
                    {
                        position = OffsetPosition;
                    }
                    else if (ParentPanel.Id != -1)
                    {
                        if (ParentPanel.ParentForm.Id != -1)
                        {
                            position = new Vector2(ParentPanel.ParentForm.Position.X + ParentPanel.OffsetPosition.X + OffsetPosition.X, ParentPanel.ParentForm.Position.Y + ParentPanel.OffsetPosition.Y + OffsetPosition.Y);
                        }
                        else
                        {
                            position = new Vector2(ParentPanel.OffsetPosition.X + OffsetPosition.X, ParentPanel.OffsetPosition.Y + OffsetPosition.Y);
                        }
                    }
                    else if (ParentForm.Id != -1)
                    {
                        position = new Vector2(ParentForm.Position.X + OffsetPosition.X, ParentForm.Position.Y + OffsetPosition.Y);
                    }

                    SpriteFont font;
                    if (BigFont)
                    {
                        font = Storage.Instance.BigFont;
                    }
                    else
                    {
                        font = Storage.Instance.Font;
                    }
                    DrawOutlinedString(spriteBatch, font, Text, position, Color.White);
                }
                catch
                {

                }
            }
        }

        public void DrawOutlinedString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            if (ParentPanel.Id != -1)
            {
                int offsetY = (ParentPanel.PanelScrollbar.CurrentStep * 10);
                RasterizerState r = new RasterizerState();
                r.ScissorTestEnable = true;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r);
                Rectangle scrollRect = new Rectangle();
                scrollRect.Width = ParentPanel.Width;
                scrollRect.Height = ParentPanel.Height - 20;
                if (ParentPanel.ParentForm.Id != -1)
                {
                    scrollRect.X = ParentPanel.ParentViewport.Viewport.X + (int)ParentPanel.OffsetPosition.X + (int) ParentPanel.ParentForm.Position.X;
                    scrollRect.Y = ParentPanel.ParentViewport.Viewport.Y + (int)ParentPanel.OffsetPosition.Y + 10 + (int) ParentPanel.ParentForm.Position.Y;
                }
                else
                {
                    scrollRect.X = ParentPanel.ParentViewport.Viewport.X + (int)ParentPanel.OffsetPosition.X;
                    scrollRect.Y = ParentPanel.ParentViewport.Viewport.Y + (int)ParentPanel.OffsetPosition.Y + 10;
                }
                spriteBatch.GraphicsDevice.ScissorRectangle = scrollRect;
                spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y + offsetY), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y + offsetY), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1 + offsetY), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1 + offsetY), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + offsetY), color);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            }
            else
            {
                spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), Color.Black);
                spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), Color.Black);
                spriteBatch.DrawString(font, text, position, color);
            }
        }

    }
}

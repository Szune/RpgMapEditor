using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class Tab : Control
    {
        public string Text;
        public ChatChannel ParentChannel;
        public Texture2D BackgroundSprite;
        public const int Stroke = 2;
        private bool _clicked = false;

        public Tab()
        {
            Id = -1;
        }

        public Tab(string text, int width, int height, ChatChannel parentChannel, Texture2D backgroundSprite)
        {
            Text = text;
            ParentChannel = parentChannel;
            BackgroundSprite = backgroundSprite;
            Width = width;
            Height = height;
            CountClick = ClickState.Try;
        }

        public void IsClicked(MouseState state, Vector2 cursorPos, int index)
        {
            int x = (int)ParentChannel.ParentChatPanel.OffsetPosition.X + (Width * index);
            int y = (int)ParentChannel.ParentChatPanel.OffsetPosition.Y;
            Rectangle tabArea = new Rectangle(x, y, Width, Height);
            if (ParentChannel.ParentChatPanel.Visible)
            {
                if (!_clicked && (state.LeftButton == ButtonState.Pressed))
                {
                    if (CountClick == ClickState.Try)
                    {
                        if (tabArea.Contains(cursorPos.X - this.ParentChannel.ParentChatPanel.ParentViewport.Viewport.X, cursorPos.Y - this.ParentChannel.ParentChatPanel.ParentViewport.Viewport.Y))
                        {
                            _clicked = true;
                            CountClick = ClickState.Count;
                        }
                        else
                        {
                            CountClick = ClickState.Dismiss;
                        }
                    }
                }
                else if (CountClick == ClickState.Count && _clicked && state.LeftButton == ButtonState.Released)
                {
                    _clicked = false;
                    CountClick = ClickState.Try;
                    this.ParentChannel.ParentChatPanel.SetFocusedChannel(this.ParentChannel.ChannelId);
                    //ButtonEventArgs args = new ButtonEventArgs(this.ID, this.Name, this.ParentForm.ID, this.ParentScreen.ID);
                    //OnClick(args);
                }
                else if (CountClick == ClickState.Dismiss && state.LeftButton == ButtonState.Released)
                {
                    CountClick = ClickState.Try;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int index, bool focused)
        {
            Color focusColor;
            if (!focused)
            {
                focusColor = Color.Gray;
            }
            else
            {
                focusColor = Color.White;
            }
            int x = (int)ParentChannel.ParentChatPanel.OffsetPosition.X + (Width * index);
            int y = (int)ParentChannel.ParentChatPanel.OffsetPosition.Y;
            if (Stroke > 0)
            {
                spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)x, (int)y, Width + (Stroke * 2), Height + (Stroke * 2)), Color.White);
            }
            spriteBatch.Draw(BackgroundSprite, new Rectangle((int)x + Stroke, (int)y + Stroke, Width, Height), Color.White);
            DrawOutlinedString(spriteBatch, font, Text, new Vector2(Stroke + x + (Width / 2) - (font.MeasureString(Text).X / 2), Stroke + y + (Height / 2) - (font.MeasureString(Text).Y / 2)), focusColor);
            //throw new NotImplementedException("Not implemented yet!");
        }

        public void DrawOutlinedString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class TextBox : Control
    {
        public Texture2D Sprite;
        private Rectangle _textboxArea;
        public bool ReadOnly;
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (_text == null)
                    _text = "";

                if (_text != "")
                {
                    String filtered = "";
                    foreach (char c in value)
                    {
                        if (Storage.Instance.Font.Characters.Contains(c))
                            filtered += c;
                    }

                    _text = filtered;
                }
            }
        }
        private bool _clicked = false;
        public bool Password;

        public event EventHandler Click;

        public TextBox()
        {
            Id = -1;
        }

        public TextBox(int id, string name, Texture2D sprite, int width, int height, Vector2 position, string text = "", bool visible = false, bool readOnly = false)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Text = text;
            _textboxArea.Width = width;
            _textboxArea.Height = height;
            _textboxArea.X = (int)position.X;
            _textboxArea.Y = (int)position.Y;
            Visible = visible;
            CountClick = ClickState.Try;
            ReadOnly = readOnly;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, bool isFocused)
        {
            if (Visible)
            {
                string drawText = Text;
                SpriteFont font = Storage.Instance.Font;

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

                if (Password)
                {
                    drawText = new string('*', Text.Length);
                }
                spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)position.X, (int)position.Y, Width, Height), Color.White);
                spriteBatch.Draw(Sprite, new Rectangle((int)position.X + 2, (int)position.Y + 2, Width - 4, Height - 4), Color.White);
                bool caret = false;
                if (gameTime.TotalGameTime.Seconds % 2 == 0) { caret = true; }

                spriteBatch.End();

                RasterizerState r = new RasterizerState();
                r.ScissorTestEnable = true;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r);
                if (HasParentForm())
                {
                    spriteBatch.GraphicsDevice.Viewport = ParentForm.ParentViewport.Viewport;
                }
                else if (HasParentViewport())
                {
                    spriteBatch.GraphicsDevice.Viewport = ParentViewport.Viewport;
                }

                Rectangle clipRect;
                clipRect.X = (int)position.X + 3 + spriteBatch.GraphicsDevice.Viewport.X;
                clipRect.Y = (int)position.Y + 3 + spriteBatch.GraphicsDevice.Viewport.Y;
                clipRect.Width = Width - 6;
                clipRect.Height = Height;
                spriteBatch.GraphicsDevice.ScissorRectangle = clipRect;
                spriteBatch.DrawString(font, drawText, new Vector2(position.X + 2, position.Y + 2), Color.Black);
                if (caret && isFocused)
                {
                    spriteBatch.DrawString(font, "|", new Vector2(position.X + 1 + font.MeasureString(drawText).X, position.Y + 2), Color.Black);
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            }
        }

        protected virtual void OnClick(EventArgs e)
        {
            EventHandler handler = Click;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public TextBox IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (Visible)
            {
                if (!_clicked && state.LeftButton == ButtonState.Pressed)
                {
                    if (CountClick == ClickState.Try)
                    {
                        if (_textboxArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
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
                    EventArgs args = new EventArgs();
                    OnClick(args);
                    return this;
                }
                else if (CountClick == ClickState.Dismiss && state.LeftButton == ButtonState.Released)
                {
                    CountClick = ClickState.Try;
                }
            }
            TextBox noClick = new TextBox();
            return noClick;
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Objects.ControlStyles;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class Button : Control
    {
        public SpriteObject SpriteObj;
        public SpriteObject PressedSprite;
        public SpriteObject NotPressedSprite;
        private Rectangle _buttonArea;
        public string Text;
        private bool _buttonPressed = false;
        public bool AllowRightClick;
        public int Stroke;
        public Color OriginalColor = Color.White;
        public Color DrawColor;
        public Color HoverColor = Color.Aqua;
        ButtonStyle _style = new ButtonStyle();

        public delegate void ButtonEventHandler(object sender, ButtonEventArgs e);

        public event ButtonEventHandler Click;

        public Button()
        {
            Id = -1;
        }

        public Button(int id, string name, ButtonStyle style, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            Id = id;
            Name = name;
            _style = style;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Text = text;
            _buttonArea.Width = width;
            _buttonArea.Height = height;
            _buttonArea.X = (int)position.X;
            _buttonArea.Y = (int)position.Y;
            Visible = visible;

            CountClick = ClickState.Try;
            DrawColor = Color.White;
        }

        public Button(int id, string name, SpriteObject sprite, int width, int height, Vector2 position, string text = "", bool visible = false, SpriteObject pressedSprite = null, int stroke = 0, bool allowRightClick = false)
        {
            Id = id;
            Name = name;
            NotPressedSprite = sprite;
            SpriteObj = sprite;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Text = text;
            _buttonArea.Width = width;
            _buttonArea.Height = height;
            _buttonArea.X = (int)position.X;
            _buttonArea.Y = (int)position.Y;
            Visible = visible;
            AllowRightClick = allowRightClick;
            Stroke = stroke;
            CountClick = ClickState.Try;
            DrawColor = Color.White;
            if (pressedSprite == null)
            {
                PressedSprite = sprite;
            }
            else
            {
                PressedSprite = pressedSprite;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Vector2 position = GetCorrectPosition();

                _buttonArea.X = (int)position.X;
                _buttonArea.Y = (int)position.Y;

                SpriteFont font = Storage.Instance.Font;
                if (_style.Id != -1)
                {
                    DrawButtonStyle(spriteBatch, position, font);
                }
                else
                {
                    if (Stroke > 0)
                    {
                        spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)position.X, (int)position.Y, Width + (Stroke * 2), Height + (Stroke * 2)), Color.White);
                    }
                    Sprite.Draw(spriteBatch, SpriteObj, new Rectangle((int)position.X + Stroke, (int)position.Y + Stroke, Width, Height), DrawColor);
                    if (Text != null && Text != "")
                    {
                        DrawOutlinedString(spriteBatch, font, Text, new Vector2(Stroke + position.X + (Width / 2) - (font.MeasureString(Text).X / 2), Stroke + position.Y + (Height / 2) - (font.MeasureString(Text).Y / 2)), Color.White);
                    }
                }
            }
        }

        /*private Vector2 GetCorrectPosition()
        {
            Vector2 Position = new Vector2(0, 0);

            if (ParentForm.ID == -1 && ParentPanel.ID == -1)
            {
                Position = OffsetPosition;
            }
            else if (ParentPanel.ID != -1)
            {
                if (ParentPanel.ParentForm.ID != -1)
                {
                    Position = new Vector2(ParentPanel.ParentForm.Position.X + ParentPanel.OffsetPosition.X + OffsetPosition.X, ParentPanel.ParentForm.Position.Y + ParentPanel.OffsetPosition.Y + OffsetPosition.Y);
                }
                else
                {
                    Position = new Vector2(ParentPanel.OffsetPosition.X + OffsetPosition.X, ParentPanel.OffsetPosition.Y + OffsetPosition.Y);
                }
            }
            else if (ParentForm.ID != -1)
            {
                Position = new Vector2(ParentForm.Position.X + OffsetPosition.X, ParentForm.Position.Y + OffsetPosition.Y);
            }

            return Position;
        }*/

        private void DrawButtonStyle(SpriteBatch spriteBatch, Vector2 position, SpriteFont font)
        {
            Texture2D spritesheet = Storage.Instance.SpritesheetUi.Texture;
            spriteBatch.Draw(spritesheet, new Rectangle((int)position.X, (int)position.Y, _style.LeftBorder.Width, Height), _style.LeftBorder, DrawColor);
            int middleWidth = Width - _style.LeftBorder.Width - _style.RightBorder.Width;
            int amountOfMiddleSpritesNeeded = (middleWidth / _style.Middle.Width);

            if (middleWidth - (amountOfMiddleSpritesNeeded * _style.Middle.Width) > 0)
            {
                amountOfMiddleSpritesNeeded++;
            }

            for (int i = 0; i < amountOfMiddleSpritesNeeded; i++)
            {
                int newX = (int)position.X + _style.LeftBorder.Width + (i * _style.Middle.Width);
                int currentWidth = _style.Middle.Width;
                if (i == amountOfMiddleSpritesNeeded - 1)
                {
                    currentWidth = (middleWidth - (i * _style.Middle.Width));
                }
                spriteBatch.Draw(spritesheet, new Rectangle(newX, (int)position.Y, currentWidth, Height), _style.Middle, DrawColor);
            }
            //_spriteBatch.Draw(Spritesheet, new Rectangle((int)(Position.X + Style.LeftBorder.Width), (int)Position.Y, middleWidth, Height), Style.Middle, drawColor);
            spriteBatch.Draw(spritesheet, new Rectangle((int)(position.X + Width - _style.RightBorder.Width), (int)position.Y, _style.RightBorder.Width, Height), _style.RightBorder, DrawColor);
            DrawOutlinedString(spriteBatch, font, Text, new Vector2(position.X + (Width / 2) - (font.MeasureString(Text).X / 2), position.Y + (Height / 2) - (font.MeasureString(Text).Y / 2)), Color.White);
        }

        public void DrawOutlinedString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
             spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        }

        protected virtual void OnClick(ButtonEventArgs e)
        {
            ButtonEventHandler handler = Click;
            if (handler != null)
            {
                if (ParentScreen.Name == Storage.Instance.CurrentScreen.Name)
                {
                    handler(this, e);
                }
            }
        }

        public void IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (Visible)
            {
                if (_buttonArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                {
                    DrawColor = HoverColor;
                }
                else
                {
                    DrawColor = OriginalColor;
                }

                if (!_buttonPressed && (state.LeftButton == ButtonState.Pressed || (AllowRightClick && state.RightButton == ButtonState.Pressed)))
                {
                    if (CountClick == ClickState.Try)
                    {
                        if (_buttonArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                        {
                            SpriteObj = PressedSprite;
                            _buttonPressed = true;
                            CountClick = ClickState.Count;
                        }
                        else
                        {
                            CountClick = ClickState.Dismiss;
                        }
                    }
                }
                else if (CountClick == ClickState.Count && _buttonPressed && state.LeftButton == ButtonState.Released && state.RightButton == ButtonState.Released)
                {
                    SpriteObj = NotPressedSprite;
                    _buttonPressed = false;
                    CountClick = ClickState.Try;
                    ButtonEventArgs args = new ButtonEventArgs(this.Id, this.Name, this.ParentForm.Id, this.ParentScreen.Id, this);
                    OnClick(args);
                }
                else if (CountClick == ClickState.Dismiss && state.LeftButton == ButtonState.Released && state.RightButton == ButtonState.Released)
                {
                    CountClick = ClickState.Try;
                }
            }
        }

    }

    public class ButtonEventArgs : EventArgs
    {
        private int _id;
        private string _name;
        private int _parentFormId;
        private int _parentScreenId;
        private Button _buttonControl;

        public ButtonEventArgs(int id, string name, int parentFormId, int parentScreenId, Button button)
        {
            this._id = id;
            this._name = name;
            this._parentFormId = parentFormId;
            this._parentScreenId = parentScreenId;
            this._buttonControl = button;
        }

        public Button Button { get { return this._buttonControl; } }

        public int ButtonId { get { return this._id; } }

        public string ButtonName { get { return this._name; } }

        public int ButtonParentFormId { get { return this._parentFormId; } }

        public int ButtonParentScreenId { get { return this._parentScreenId; } }
    }
}

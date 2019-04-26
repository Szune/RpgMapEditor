using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class ComboBox : Control
    {
        public List<string> Items = new List<string>();
        public Color DrawColor;
        public Rectangle ComboBoxArea = new Rectangle();
        public Rectangle ScrollbarMiddle = new Rectangle();
        public Rectangle ScrollbarUp = new Rectangle();
        public Rectangle ScrollbarDown = new Rectangle();
        public int HoverId = -1;
        public int ClickedId = -1;
        public bool Clicked = false;
        public string SelectedItemText = "";
        public Vector2 CurrentPosition = new Vector2(0, 0);
        public SpriteObject BackgroundSprite;
        public bool Open = false;
        public int MaxHeight = -1;
        public Scrollbar ComboScroll;
        public bool ShowScrollbar;
        public bool Scrollable;
        private int _offsetY;

        public delegate void ComboBoxEventHandler(object sender, ComboBoxEventArgs e);

        public event ComboBoxEventHandler Click;

        public ComboBox()
        {
            Id = -1;
        }

        public ComboBox(int id, string name, int width, int height, Vector2 position, SpriteObject background, bool visible, bool forceScrollbar = false)
        {
            Id = id;
            Name = name;
            //Width = _Width;
            Height = height;
            OffsetPosition = position;
            Visible = visible;
            DrawColor = Color.White;

            Width = width + 20;

            ComboBoxArea.X = (int)position.X;
            ComboBoxArea.Y = (int)position.Y;
            ComboBoxArea.Width = Width;
            ComboBoxArea.Height = height;
            BackgroundSprite = background;

            ComboScroll = new Scrollbar(1, name + "Scrollbar", Height, Height - 14, new Vector2(OffsetPosition.X + Width - 10, OffsetPosition.Y + 16), this, ParentViewport, Storage.Instance.GetSpriteByName("UI_Scrollbar"), Storage.Instance.GetSpriteByName("UI_Arrow_Up"), Storage.Instance.GetSpriteByName("UI_Arrow_Down"));
        }

        public new void Hide()
        {
            ClickedId = -1;
            SelectedItemText = "";
            Visible = false;
        }

        public new void Show()
        {
            Visible = true;
            ComboScroll.CurrentStep = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                if(Items.Count > 6)
                {
                    MaxHeight = (7 * 16) + 40;
                    ShowScrollbar = true;
                }
                else
                {
                    MaxHeight = (Items.Count * 16) + 40;
                    ShowScrollbar = false;
                }

                int scrollMaxHeight = (Items.Count * 16) + 40;

                ComboScroll.UpdateHeight(MaxHeight, scrollMaxHeight);
                Vector2 position = GetCorrectPosition();
                CurrentPosition = position;

                _offsetY = ComboScroll.CurrentStep * 10;
                //CurrentPosition.Y += OffsetY;

                SpriteFont font = Storage.Instance.Font;

                Scrollable = Open;

                if (Open)
                {
                    ComboBoxArea.Height = MaxHeight;
                    ComboBoxArea.X = (int)position.X;
                    ComboBoxArea.Y = (int)position.Y;
                    //ComboScroll.MaxHeight = MaxHeight - 14;
                }
                else
                {
                    ComboBoxArea.Height = Height;
                    ComboBoxArea.X = (int)position.X;
                    ComboBoxArea.Y = (int)position.Y;
                }

                if (Open)
                {
                    spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)position.X, (int)position.Y, Width, MaxHeight), Color.White);
                    Sprite.Draw(spriteBatch, BackgroundSprite, new Rectangle((int)position.X + 2, (int)position.Y + 2, Width - 4, MaxHeight - 4), Color.White);

                    RasterizerState r = new RasterizerState();
                    r.ScissorTestEnable = true;
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r);
                    Rectangle scrollRect = new Rectangle();
                    scrollRect.Width = Width;
                    scrollRect.Height = MaxHeight - 40;
                    scrollRect.X = (int)position.X + ParentViewport.Viewport.X;
                    scrollRect.Y = (int)position.Y + 20 + ParentViewport.Viewport.Y;

                    spriteBatch.GraphicsDevice.ScissorRectangle = scrollRect;

                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (HoverId == i)
                        {
                            DrawColor = Color.Green;
                        }
                        else if (ClickedId == i)
                        {
                            DrawColor = Color.Gold;
                        }
                        else
                        {
                            DrawColor = Color.White;
                        }
                        DrawOutlinedString(spriteBatch, font, Items[i], new Vector2(position.X + 15, position.Y + 20 + (i * 16) + _offsetY), DrawColor);
                    }

                    if (ShowScrollbar)
                    {
                        ComboScroll.Draw(spriteBatch);
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);


                }
                else
                {
                    DrawColor = Color.White;
                    spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)position.X, (int)position.Y, Width, Height), Color.White);
                    Sprite.Draw(spriteBatch, BackgroundSprite, new Rectangle((int)position.X + 2, (int)position.Y + 2, Width - 4, Height - 4), Color.White);
                    DrawOutlinedString(spriteBatch, font, SelectedItemText, new Vector2(position.X + 10, position.Y + (Height / 2) - 8), DrawColor);
                    SpriteObject spr = Storage.Instance.GetSpriteByName("UI_ConfirmButton");
                    Sprite.Draw(spriteBatch, spr, new Rectangle((int)position.X + Width - 15, (int)position.Y + (Height / 2) - 4, spr.SpritesheetPosition.Width, spr.SpritesheetPosition.Height), Color.White);
                }
            }
        }

        public void DrawOutlinedString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }

        protected virtual void OnClick(ComboBoxEventArgs e)
        {
            ComboBoxEventHandler handler = Click;
            if (handler != null)
            {
                if (ParentScreen.Name == Storage.Instance.CurrentScreen.Name)
                {
                    handler(this, e);
                }
            }
        }

        public void UpdateClicked(int id)
        {
            ClickedId = id;
            SelectedItemText = Items[id];
        }

        public bool IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (ComboScroll.ParentScreen == null)
            {
                ComboScroll.ParentScreen = this.ParentScreen;
            }
            else if (ComboScroll.ParentViewport == null)
            {
                ComboScroll.ParentViewport = this.ParentViewport;
            }

            if (Visible)
            {
                HoverId = -1;
                if (!ComboScroll.IsClicked(state, cursorPos, Visible))
                {
                    if (Open)
                    {
                        if (Items.Count > 4)
                        {
                            ShowScrollbar = true;
                        }
                        else
                        {
                            ShowScrollbar = false;
                        }

                        if (ComboBoxArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                        {
                            for (int i = 0; i < Items.Count; i++)
                            {
                                Rectangle tmpRect = new Rectangle((int)CurrentPosition.X + 15, (int)CurrentPosition.Y + 20 + (i * 16) + _offsetY, Width, 16);

                                if (!Clicked && state.LeftButton == ButtonState.Pressed)
                                {
                                    if (tmpRect.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                                    {
                                        Clicked = true;
                                    }
                                }
                                else if (Clicked && state.LeftButton == ButtonState.Released)
                                {
                                    if (tmpRect.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                                    {
                                        UpdateClicked(i);
                                        ComboBoxEventArgs args = new ComboBoxEventArgs(this.Id, this.Name, this.ParentForm.Id, this.ParentScreen.Id, Items[i]);
                                        OnClick(args);
                                        Clicked = false;
                                        Open = false;
                                    }
                                }

                                if (tmpRect.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                                {
                                    HoverId = i;
                                }
                            }
                        }
                        else
                        {
                            Clicked = false;
                        }
                    }
                    else
                    {
                        if (!Storage.Instance.ComboBoxOpen)
                        {
                            if (ComboBoxArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                            {
                                if (!Clicked && state.LeftButton == ButtonState.Pressed)
                                {
                                    Clicked = true;
                                }
                                else if (Clicked && state.LeftButton == ButtonState.Released)
                                {
                                    Open = true;
                                    Clicked = false;
                                }
                            }
                            else
                            {
                                Clicked = false;
                            }
                        }
                    }

                    if (Open && !Clicked && state.LeftButton == ButtonState.Pressed)
                    {
                        Open = false;
                    }
                }
            }

            return Open;
        }

        public class ComboBoxEventArgs : EventArgs
        {
            private int _id;
            private string _name;
            private int _parentFormId;
            private int _parentScreenId;
            private string _selectedItemText;

            public ComboBoxEventArgs(int id, string name, int parentFormId, int parentScreenId, string selectedItemText = "")
            {
                this._id = id;
                this._name = name;
                this._parentFormId = parentFormId;
                this._parentScreenId = parentScreenId;
                this._selectedItemText = selectedItemText;
            }

            public int ComboBoxId { get { return this._id; } }

            public string ComboBoxName { get { return this._name; } }

            public int ComboBoxParentFormId { get { return this._parentFormId; } }

            public int ComboBoxParentScreenId { get { return this._parentScreenId; } }

            public string ComboBoxSelectedItemText { get { return this._selectedItemText; } }
        }
    }
}

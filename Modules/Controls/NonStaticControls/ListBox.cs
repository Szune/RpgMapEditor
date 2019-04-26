using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class ListBox : Control
    {
        public List<string> Items = new List<string>();
        public Color DrawColor;
        public Rectangle ListBoxArea = new Rectangle();
        public int HoverId = -1;
        public int ClickedId = -1;
        public bool Clicked = false;
        public string SelectedItemText = "";
        public Vector2 CurrentPosition = new Vector2(0, 0);
        public Scrollbar ListScroll;
        public bool ShowScrollbar;
        public bool Scrollable;
        public int MaxHeight = -1;
        private int _offsetY;

        public delegate void ListBoxEventHandler(object sender, ListBoxEventArgs e);

        public event ListBoxEventHandler Click;

        public ListBox()
        {
            Id = -1;
        }

        public ListBox(int id, string name, int width, int height, Vector2 position, bool visible)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Visible = visible;
            DrawColor = Color.White;

            ListBoxArea.X = (int)position.X;
            ListBoxArea.Y = (int)position.Y;
            ListBoxArea.Width = width;
            ListBoxArea.Height = height;
            ListScroll = new Scrollbar(1, name + "Scrollbar", Height, Height - 14, new Vector2(OffsetPosition.X + Width - 10, OffsetPosition.Y), this, ParentViewport, Storage.Instance.GetSpriteByName("UI_Scrollbar"), Storage.Instance.GetSpriteByName("UI_Arrow_Up"), Storage.Instance.GetSpriteByName("UI_Arrow_Down"));
        }

        public new void Hide()
        {
            ClickedId = -1;
            SelectedItemText = "";
            Visible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Vector2 position = GetCorrectPosition();

                CurrentPosition = position;
                _offsetY = ListScroll.CurrentStep * 10;

                SpriteFont font = Storage.Instance.Font;

                Scrollable = true;
                int maxHeight = (Items.Count * 16);

                ListBoxArea.X = (int)position.X;
                ListBoxArea.Y = (int)position.Y;
                ListBoxArea.Height = maxHeight;

                RasterizerState r = new RasterizerState();
                r.ScissorTestEnable = true;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r);
                Rectangle scrollRect = new Rectangle();
                scrollRect.Width = Width;
                scrollRect.Height = Height;
                scrollRect.X = (int)position.X + ParentViewport.Viewport.X - 2;
                scrollRect.Y = (int)position.Y + ParentViewport.Viewport.Y;

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
                    DrawOutlinedString(spriteBatch, font, Items[i], new Vector2(position.X, position.Y + (i * 16) + _offsetY), DrawColor);
                }

                if (maxHeight > Height)
                {
                    Scrollable = true;
                    ListScroll.UpdateWidth(40);
                    ListScroll.UpdateHeight(Height + 40, maxHeight);
                    ListScroll.Draw(spriteBatch);
                }
                else
                {
                    Scrollable = false;
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
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

        protected virtual void OnClick(ListBoxEventArgs e)
        {
            ListBoxEventHandler handler = Click;
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

        public void IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (Visible)
            {
                HoverId = -1;
                if (!ListScroll.IsClicked(state, cursorPos, Visible))
                {
                    if (ListBoxArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                    {
                        for (int i = 0; i < Items.Count; i++)
                        {
                            Rectangle tmpRect = new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y + (i * 16) + _offsetY, Width, 16);

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
                                    ListBoxEventArgs args = new ListBoxEventArgs(this.Id, this.Name, this.ParentForm.Id, this.ParentScreen.Id, Items[i]);
                                    OnClick(args);
                                    Clicked = false;
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
            }
        }

        public class ListBoxEventArgs : EventArgs
        {
            private int _id;
            private string _name;
            private int _parentFormId;
            private int _parentScreenId;
            private string _selectedItemText;

            public ListBoxEventArgs(int id, string name, int parentFormId, int parentScreenId, string selectedItemText = "")
            {
                this._id = id;
                this._name = name;
                this._parentFormId = parentFormId;
                this._parentScreenId = parentScreenId;
                this._selectedItemText = selectedItemText;
            }

            public int ListBoxId { get { return this._id; } }

            public string ListBoxName { get { return this._name; } }

            public int ListBoxParentFormId { get { return this._parentFormId; } }

            public int ListBoxParentScreenId { get { return this._parentScreenId; } }

            public string ListBoxSelectedItemText {  get { return this._selectedItemText; } }
        }
    }
}
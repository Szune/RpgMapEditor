using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class ImageListBox : Control
    {
        public List<ImageListItem> Items = new List<ImageListItem>();
        public Color DrawColor;
        public Rectangle ListBoxArea = new Rectangle();
        public int HoverId = -1;
        public int ClickedId = -1;
        public bool Clicked;
        public ImageListItem SelectedItem = new ImageListItem();
        public Vector2 CurrentPosition = new Vector2(0, 0);
        public Scrollbar ImageScroll;
        public bool ShowScrollbar;
        public bool Scrollable;
        public int MaxHeight = -1;
        private int _offsetY;

        public int Scale = 2;

        public delegate void ImageListBoxEventHandler(object sender, ImageListBoxEventArgs e);

        public event ImageListBoxEventHandler Click;

        public ImageListBox()
        {
            Id = -1;
        }

        public ImageListBox(int id, string name, int width, int height, Vector2 position, bool visible)
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
            ImageScroll = new Scrollbar(1, name + "Scrollbar", Height, Height - 14, new Vector2(OffsetPosition.X + Width - 10, OffsetPosition.Y), this, ParentViewport, Storage.Instance.GetSpriteByName("UI_Scrollbar"), Storage.Instance.GetSpriteByName("UI_Arrow_Up"), Storage.Instance.GetSpriteByName("UI_Arrow_Down"));
        }

        public new void Hide()
        {
            ClickedId = -1;
            SelectedItem = new ImageListItem();
            Visible = false;
            ResetScrollPosition();
        }

        public new void Show()
        {
            Visible = true;
            ResetScrollPosition();
        }

        public void ResetScrollPosition()
        {
            ImageScroll.CurrentStep = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Vector2 position = GetCorrectPosition();

                CurrentPosition = position;
                _offsetY = ImageScroll.CurrentStep * 10;
                //CurrentPosition.Y += OffsetY;

                SpriteFont font = Storage.Instance.Font;

                Scrollable = true;
                int maxHeight = (Items.Count * (16 * Scale));

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
                scrollRect.X = (int)position.X + ParentViewport.Viewport.X;
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
                    Size spriteSize = Sprite.GetSize(Items[i].Sprite);
                    Sprite.Draw(spriteBatch, Items[i].Sprite, new Rectangle((int)position.X, (int)position.Y + (i * (16 * Scale)) + _offsetY, spriteSize.Width * Scale, spriteSize.Height * Scale), DrawColor);
                    DrawOutlinedString(spriteBatch, font, Items[i].Text, new Vector2(position.X + (spriteSize.Width * Scale) + 5, position.Y + (i * (16 * Scale)) + _offsetY), DrawColor);
                }

                if (maxHeight > Height)
                {
                    Scrollable = true;
                    ImageScroll.UpdateWidth(30);
                    ImageScroll.UpdateHeight(Height, maxHeight);
                    ImageScroll.Draw(spriteBatch);
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

        protected virtual void OnClick(ImageListBoxEventArgs e)
        {
            ImageListBoxEventHandler handler = Click;
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
                HoverId = -1;
                if (!ImageScroll.IsClicked(state, cursorPos, Visible))
                {
                    if (ListBoxArea.Contains(cursorPos.X - this.ParentViewport.Viewport.X, cursorPos.Y - this.ParentViewport.Viewport.Y))
                    {
                        for (int i = 0; i < Items.Count; i++)
                        {
                            Rectangle tmpRect = new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y + (i * (16 * Scale)) + _offsetY, Width, (16 * Scale));

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
                                    ClickedId = i;
                                    SelectedItem = Items[i];
                                    ImageListBoxEventArgs args = new ImageListBoxEventArgs(this.Id, this.Name, this.ParentForm.Id, this.ParentScreen.Id, Items[i]);
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



        public class ImageListBoxEventArgs : EventArgs
        {
            private int _id;
            private string _name;
            private int _parentFormId;
            private int _parentScreenId;
            private ImageListItem _selectedItem;

            public ImageListBoxEventArgs(int id, string name, int parentFormId, int parentScreenId, ImageListItem selectedItem)
            {
                this._id = id;
                this._name = name;
                this._parentFormId = parentFormId;
                this._parentScreenId = parentScreenId;
                this._selectedItem = selectedItem;
            }

            public int ImageListBoxId { get { return this._id; } }

            public string ImageListBoxName { get { return this._name; } }

            public int ImageListBoxParentFormId { get { return this._parentFormId; } }

            public int ImageListBoxParentScreenId { get { return this._parentScreenId; } }

            public ImageListItem ImageListBoxSelectedItem { get { return this._selectedItem; } }
        }
    }
}

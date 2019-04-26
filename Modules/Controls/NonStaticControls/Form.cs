using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Controls.HelperObjects;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Objects.ControlStyles;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class Form
    {
        public int Width;
        public int Height;
        public Rectangle Hitbox;
        public Vector2 Position;
        public Texture2D BackgroundSprite;
        public string Title;
        public int Id;
        public string Name;
        public List<Label> Labels = new List<Label>();
        public List<Button> Buttons = new List<Button>();
        public List<TextBox> Textboxes = new List<TextBox>();
        public List<ListBox> ListBoxes = new List<ListBox>();
        public List<ComboBox> ComboBoxes = new List<ComboBox>();
        public List<ImageListBox> ImageListBoxes = new List<ImageListBox>();
        public List<ProgressBar> ProgressBars = new List<ProgressBar>();
        public List<Panel> Panels = new List<Panel>();
        public Screen ParentScreen = new Screen();
        public EViewport ParentViewport = new EViewport();
        public Button TitleBackground;
        public bool Visible;
        public int Stroke;
        public bool Moving = false;
        private int _clickedWidthX;
        private int _clickedWidthY;
        FormStyle _style = new FormStyle();


        public Form()
        {
            Id = -1;
        }

        public Form(int id, string name, FormStyle style, ButtonStyle titleStyle, int width, int height, Vector2 position, string title = "", bool visible = false, int stroke = 0)
        {
            Id = id;
            Name = name;
            _style = style;
            Width = width;
            Height = height;
            // (int)font.MeasureString(Title).Y) = 17
            Hitbox = new Rectangle((int)position.X, (int)position.Y, width, 17 + (stroke * 2));
            Position = position;
            Title = title;
            Visible = visible;

            int titleSize = (int)Storage.Instance.Font.MeasureString(title).X + 64;
            TitleBackground = new Button(1, title, titleStyle, titleSize, 32, new Vector2((width / 2) - titleSize / 2, 20), title, true);
            TitleBackground.ParentForm = this;
            TitleBackground.ParentScreen = this.ParentScreen;
        }

        public Form(int id, string name, Texture2D sprite, int width, int height, Vector2 position, string title = "", bool visible = false, int stroke = 0)
        {
            Id = id;
            Name = name;
            BackgroundSprite = sprite;
            Width = width;
            Height = height;
            // (int)font.MeasureString(Title).Y) = 17
            Hitbox = new Rectangle((int)position.X,(int) position.Y, width, 17 + (stroke * 2));
            Position = position;
            Title = title;
            Visible = visible;
            Stroke = stroke;
        }

        public Button AddButton(int id, string name, ButtonStyle style, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            Buttons.Add(new Button(id, name, style, width, height, position, text, visible));
            Button getButton = Buttons[Buttons.Count - 1];
            getButton.ParentForm = this;
            getButton.ParentScreen = this.ParentScreen;
            return getButton;
        }

        public Button AddButton(int id, string name, SpriteObject sprite, int width, int height, Vector2 position, string text = "", bool visible = false, SpriteObject pressedSprite = null, int stroke = 0, bool allowRightClick = false)
        {
            Buttons.Add(new Button(id, name, sprite, width, height, position, text, visible, pressedSprite, stroke, allowRightClick));
            Button getButton = Buttons[Buttons.Count - 1];
            getButton.ParentForm = this;
            getButton.ParentScreen = this.ParentScreen;
            return getButton;
        }

        public ComboBox AddComboBox(int id, string name, int width, int height, Vector2 position, SpriteObject background, bool visible = false)
        {
            ComboBoxes.Add(new ComboBox(id, name, width, height, position, background, visible));
            ComboBox getComboBox = ComboBoxes[ComboBoxes.Count - 1];
            getComboBox.ParentForm = this;
            getComboBox.ParentScreen = this.ParentScreen;
            getComboBox.ParentViewport = this.ParentViewport;
            return getComboBox;
        }

        public ImageListBox AddImageListBox(int id, string name, int width, int height, Vector2 position, bool visible = false)
        {
            ImageListBoxes.Add(new ImageListBox(id, name, width, height, position, visible));
            ImageListBox getImageListBox = ImageListBoxes[ImageListBoxes.Count - 1];
            getImageListBox.ParentForm = this;
            getImageListBox.ParentScreen = this.ParentScreen;
            getImageListBox.ParentViewport = this.ParentViewport;
            return getImageListBox;
        }

        public ListBox AddListBox(int id, string name, int width, int height, Vector2 position, bool visible = false)
        {
            ListBoxes.Add(new ListBox(id, name, width, height, position, visible));
            ListBox getListBox = ListBoxes[ListBoxes.Count - 1];
            getListBox.ParentForm = this;
            getListBox.ParentScreen = this.ParentScreen;
            getListBox.ParentViewport = this.ParentViewport;
            return getListBox;
        }

        public Label AddLabel(int id, string name, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            Labels.Add(new Label(id, name, width, height, position, text, visible));
            Label getLabel = Labels[Labels.Count - 1];
            getLabel.ParentForm = this;
            getLabel.ParentScreen = this.ParentScreen;
            return getLabel;
        }

        public TextBox AddTextbox(int id, string name, Texture2D sprite, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            Textboxes.Add(new TextBox(id, name, sprite, width, height, position, text, visible));
            TextBox getTextbox = Textboxes[Textboxes.Count - 1];
            getTextbox.ParentForm = this;
            getTextbox.ParentScreen = this.ParentScreen;
            return getTextbox;
        }

        public Panel AddPanel(int id, string name, FormStyle style, int width, int height, Vector2 position, bool showScrollbar, string text = "", bool visible = false, int stroke = 0)
        {
            Panels.Add(new Panel(id, name, style, width, height, position, showScrollbar, text, visible, stroke));
            Panel getPanel = Panels[Panels.Count - 1];
            getPanel.ParentForm = this;
            getPanel.ParentScreen = this.ParentScreen;
            getPanel.ParentViewport = this.ParentViewport;
            return getPanel;
        }

        public Panel AddPanel(int id, string name, Texture2D sprite, int width, int height, Vector2 position, bool showScrollbar, string text = "", bool visible = false, int stroke = 0)
        {
            Panels.Add(new Panel(id, name, sprite, width, height, position, showScrollbar, text, visible, stroke));
            Panel getPanel = Panels[Panels.Count - 1];
            getPanel.ParentForm = this;
            getPanel.ParentScreen = this.ParentScreen;
            getPanel.ParentViewport = this.ParentViewport;
            return getPanel;
        }

        public ProgressBar AddProgressBar(int id, string name, Texture2D sprite, Texture2D backgroundSprite, int width, int height, Vector2 position, int borderSize, bool visible = false)
        {
            ProgressBars.Add(new ProgressBar(id, name, sprite, backgroundSprite, width, height, position, borderSize, visible));
            ProgressBar getProgressBar = ProgressBars[ProgressBars.Count - 1];
            getProgressBar.ParentForm = this;
            getProgressBar.ParentScreen = this.ParentScreen;
            return getProgressBar;
        }

        public Button GetButtonByName(string name)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i].Name.ToLower() == name.ToLower())
                {
                    return Buttons[i];
                }
            }
            return new Button();
        }

        public Button GetButtonById(int id)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i].Id == id)
                {
                    return Buttons[i];
                }
            }
            return new Button();
        }

        public Label GetLabelByName(string name)
        {
            for (int i = 0; i < Labels.Count; i++)
            {
                if (Labels[i].Name.ToLower() == name.ToLower())
                {
                    return Labels[i];
                }
            }
            return new Label();
        }

        public Label GetLabelById(int id)
        {
            for (int i = 0; i < Labels.Count; i++)
            {
                if (Labels[i].Id == id)
                {
                    return Labels[i];
                }
            }
            return new Label();
        }

        public ComboBox GetComboBoxByName(string name)
        {
            for (int i = 0; i < ComboBoxes.Count; i++)
            {
                if (ComboBoxes[i].Name.ToLower() == name.ToLower())
                {
                    return ComboBoxes[i];
                }
            }
            return new ComboBox();
        }

        public ComboBox GetComboBoxById(int id)
        {
            for (int i = 0; i < ComboBoxes.Count; i++)
            {
                if (ComboBoxes[i].Id == id)
                {
                    return ComboBoxes[i];
                }
            }
            return new ComboBox();
        }

        public ListBox GetListBoxByName(string name)
        {
            for (int i = 0; i < ListBoxes.Count; i++)
            {
                if (ListBoxes[i].Name.ToLower() == name.ToLower())
                {
                    return ListBoxes[i];
                }
            }
            return new ListBox();
        }

        public ListBox GetListBoxById(int id)
        {
            for (int i = 0; i < ListBoxes.Count; i++)
            {
                if (ListBoxes[i].Id == id)
                {
                    return ListBoxes[i];
                }
            }
            return new ListBox();
        }

        public TextBox GetTextboxByName(string name)
        {
            for (int i = 0; i < Textboxes.Count; i++)
            {
                if (Textboxes[i].Name.ToLower() == name.ToLower())
                {
                    return Textboxes[i];
                }
            }
            return new TextBox();
        }

        public TextBox GetTextboxById(int id)
        {
            for (int i = 0; i < Textboxes.Count; i++)
            {
                if (Textboxes[i].Id == id)
                {
                    return Textboxes[i];
                }
            }
            return new TextBox();
        }

        public Panel GetPanelByName(string name)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                if (Panels[i].Name.ToLower() == name.ToLower())
                {
                    return Panels[i];
                }
            }
            return new Panel();
        }

        public Panel GetPanelById(int id)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                if (Panels[i].Id == id)
                {
                    return Panels[i];
                }
            }
            return new Panel();
        }

        public ProgressBar GetProgressBarByName(string name)
        {
            for (int i = 0; i < ProgressBars.Count; i++)
            {
                if (ProgressBars[i].Name.ToLower() == name.ToLower())
                {
                    return ProgressBars[i];
                }
            }
            return new ProgressBar();
        }

        public ProgressBar GetProgressBarById(int id)
        {
            for (int i = 0; i < ProgressBars.Count; i++)
            {
                if (ProgressBars[i].Id == id)
                {
                    return ProgressBars[i];
                }
            }
            return new ProgressBar();
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, TextBox focusedTextbox)
        {
            if (Visible)
            {
                SpriteFont font = Storage.Instance.Font;
                if (_style.Id != -1)
                {
                    DrawFormStyle(spriteBatch, font);
                }
                else
                {
                    if (Stroke > 0)
                    {
                        spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)Position.X, (int)Position.Y, Width + (Stroke * 2), Height + (Stroke * 2)), Color.White);
                    }
                    spriteBatch.Draw(BackgroundSprite, new Rectangle((int)Position.X + Stroke, (int)Position.Y + Stroke, Width, Height), Color.White);
                    //_spriteBatch.Draw(BackgroundSprite, new Rectangle((int)Position.X + Stroke, (int)Position.Y + Stroke, Width, (int)font.MeasureString(Title).Y), Color.Gray);
                    DrawOutlinedString(spriteBatch, font, Title, new Vector2(Position.X + Stroke + 1, Position.Y + 1), Color.White);
                }

                for (int i = 0; i < Labels.Count; i++)
                {
                    Labels[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ListBoxes.Count; i++)
                {
                    ListBoxes[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ImageListBoxes.Count; i++)
                {
                    ImageListBoxes[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ComboBoxes.Count; i++)
                {
                    ComboBoxes[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ProgressBars.Count; i++)
                {
                    ProgressBars[i].Draw(spriteBatch);
                }
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].Draw(spriteBatch);
                }
                if(_style.Id != -1)
                {
                    TitleBackground.Draw(spriteBatch);
                }

                bool isFocused = false;
                for (int i = 0; i < Textboxes.Count; i++)
                {
                    if (focusedTextbox.Name == Textboxes[i].Name) { isFocused = true; } else { isFocused = false; }
                    Textboxes[i].Draw(spriteBatch, gameTime, isFocused);
                }
                for(int i = 0; i < Panels.Count; i++)
                {
                    Panels[i].Draw(spriteBatch, font, gameTime, focusedTextbox);
                }
            }
        }

        private void DrawFormStyle(SpriteBatch spriteBatch, SpriteFont font)
        {
            //_spriteBatch.Draw(BackgroundSprite, new Rectangle((int)Position.X + Stroke, (int)Position.Y + Stroke, Width, Height), Color.White);

            Texture2D spritesheet = Storage.Instance.SpritesheetUi.Texture;
            Color titleBackgroundColor = Color.Thistle;

            // Draw top left corner
            spriteBatch.Draw(spritesheet, new Rectangle((int)Position.X, (int)Position.Y, _style.TopLeftCorner.Width, _style.TopLeftCorner.Height), _style.TopLeftCorner, titleBackgroundColor);
            // Draw top right corner
            spriteBatch.Draw(spritesheet, new Rectangle((int)Position.X + Width - _style.TopRightCorner.Width, (int)Position.Y, _style.TopRightCorner.Width, _style.TopRightCorner.Height), _style.TopRightCorner, titleBackgroundColor);
            // Draw bottom left corner
            spriteBatch.Draw(spritesheet, new Rectangle((int)Position.X, (int)Position.Y + Height - _style.BottomLeftCorner.Height, _style.BottomLeftCorner.Width, _style.BottomLeftCorner.Height), _style.BottomLeftCorner, Color.White);
            // Draw bottom right corner
            spriteBatch.Draw(spritesheet, new Rectangle((int)Position.X + Width - _style.BottomRightCorner.Width, (int)Position.Y + Height - _style.BottomRightCorner.Height, _style.BottomRightCorner.Width, _style.BottomRightCorner.Height), _style.BottomRightCorner, Color.White);

            int middleWidth = Width - _style.LeftBorder.Width - _style.RightBorder.Width;
            //int middleHeight = Height - Style.TopBorder.Height - Style.BottomBorder.Height;
            int middleHeight = Height;
            int amountOfHorizontalSpritesNeeded = (middleWidth / _style.Middle.Width);
            int amountOfVerticalSpritesNeeded = (middleHeight / _style.Middle.Height);

            if (middleWidth - (amountOfHorizontalSpritesNeeded * _style.Middle.Width) > 0)
            {
                amountOfHorizontalSpritesNeeded++;
            }

            if (middleHeight - (amountOfVerticalSpritesNeeded * _style.LeftBorder.Height) > 0)
            {
                amountOfHorizontalSpritesNeeded++;
            }

            for (int y = 0; y < amountOfVerticalSpritesNeeded; y++)
            {
                // Don't draw over corner sprites, and draw different sprites for left border and right border!
                if (y > 0 && y < amountOfVerticalSpritesNeeded - 1)
                {
                    spriteBatch.Draw(spritesheet, new Rectangle((int)Position.X, (int)Position.Y + (y * _style.LeftBorder.Height), _style.LeftBorder.Width, _style.LeftBorder.Height), _style.LeftBorder, Color.White);
                    spriteBatch.Draw(spritesheet, new Rectangle((int)Position.X + Width - _style.RightBorder.Width, (int)Position.Y + (y * _style.RightBorder.Height), _style.RightBorder.Width, _style.RightBorder.Height), _style.RightBorder, Color.White);
                }

                for (int x = 0; x < amountOfHorizontalSpritesNeeded; x++)
                {
                    int newX = (int)Position.X + _style.LeftBorder.Width + (x * _style.Middle.Width);
                    int currentWidth = _style.Middle.Width;
                    if (x == amountOfHorizontalSpritesNeeded - 1)
                    {
                        currentWidth = (middleWidth - (x * _style.Middle.Width));
                    }


                    // Don't draw over corner sprites, and draw different sprites for top border and bottom border!
                    if (y == 0)
                    {
                        spriteBatch.Draw(spritesheet, new Rectangle(newX, (int)Position.Y + (y * _style.TopBorder.Height), currentWidth, _style.TopBorder.Height), _style.TopBorder, titleBackgroundColor);
                    }
                    else if (y == amountOfVerticalSpritesNeeded - 1)
                    {
                        spriteBatch.Draw(spritesheet, new Rectangle(newX, (int)Position.Y + (y * _style.BottomBorder.Height), currentWidth, _style.BottomBorder.Height), _style.BottomBorder, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(spritesheet, new Rectangle(newX, (int)Position.Y + (y * _style.Middle.Height), currentWidth, _style.Middle.Height), _style.Middle, Color.White);
                    }
                    // 1. Draw all corner sprites
                    // 2. Draw horizontal (----) and vertical (||||) sprites
                }
            }

        }



        public void IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (Visible)
            {
                if(state.LeftButton == ButtonState.Pressed && !Moving)
                {
                    if (Hitbox.Contains(state.Position))
                    {
                        Moving = true;
                        _clickedWidthX = (int)state.Position.X - (int)Position.X;
                        _clickedWidthY = (int)state.Position.Y - (int)Position.Y;
                    }
                }
                else if(state.LeftButton == ButtonState.Pressed && Moving)
                {
                    Move(cursorPos);
                }
                else if(state.LeftButton == ButtonState.Released)
                {
                    Moving = false;
                }
            }
        }

        public void Move(Vector2 position)
        {
            Rectangle newPos = new Rectangle((int) position.X - _clickedWidthX, (int)position.Y - _clickedWidthY, Width, (int)Storage.Instance.Font.MeasureString(Title).Y + (Stroke * 2));
            Hitbox = newPos;
            Position = new Vector2((float)newPos.X, (float)newPos.Y);
        }

        public void DrawOutlinedString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }

        public ClickEventObject DoEvents(MouseState state, Vector2 cursorPos)
        {
            ClickEventObject tmpClicked = new ClickEventObject();
            ClickEventObject returnClicked = new ClickEventObject();
            bool tmpOpen = false;
            TextBox focusedTextbox = new TextBox();
            if (Visible)
            {
                IsClicked(state, cursorPos);
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].IsClicked(state, cursorPos);
                }
                for (int i = 0; i < ImageListBoxes.Count; i++)
                {
                    ImageListBoxes[i].IsClicked(state, cursorPos);
                }
                for (int i = 0; i < ListBoxes.Count; i++)
                {
                    ListBoxes[i].IsClicked(state, cursorPos);
                }
                for (int i = 0; i < ComboBoxes.Count; i++)
                {
                    tmpOpen = ComboBoxes[i].IsClicked(state, cursorPos);
                    if(tmpOpen)
                    {
                        returnClicked.ComboBoxOpen = true;
                    }
                }

                for (int i = 0; i < Textboxes.Count; i++)
                {
                    focusedTextbox = Textboxes[i].IsClicked(state, cursorPos);
                    if (focusedTextbox.Id != -1)
                    {
                        returnClicked.ClickedTextbox = focusedTextbox;
                    }
                }

                for (int i = 0; i < Panels.Count; i++)
                {
                    tmpClicked = Panels[i].DoEvents(state, cursorPos);
                    if(tmpClicked.ClickedTextbox.Id != -1)
                    {
                        returnClicked.ClickedTextbox = tmpClicked.ClickedTextbox;
                    }

                    if(tmpClicked.ComboBoxOpen)
                    {
                        returnClicked.ComboBoxOpen = true;
                    }
                }
            }
            return returnClicked;
        }
    }
}

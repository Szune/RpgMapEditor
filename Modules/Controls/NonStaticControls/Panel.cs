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
    public class Panel
    {
        public int Width;
        public int Height;
        public Vector2 OffsetPosition;
        public Texture2D BackgroundSprite;
        public string Title;
        public int Id;
        public string Name;
        public List<Label> Labels = new List<Label>();
        public List<Button> Buttons = new List<Button>();
        public List<TextBox> TextBoxes = new List<TextBox>();
        public List<ListBox> ListBoxes = new List<ListBox>();
        public List<ComboBox> ComboBoxes = new List<ComboBox>();
        public List<ImageListBox> ImageListBoxes = new List<ImageListBox>();
        public List<ProgressBar> ProgressBars = new List<ProgressBar>();
        public Screen ParentScreen = new Screen();
        public EViewport ParentViewport = new EViewport();
        public Form ParentForm = new Form();
        public bool Visible;
        public int Stroke;
        public Scrollbar PanelScrollbar;
        public bool ShowScrollbar;
        FormStyle _style = new FormStyle();

        public Panel()
        {
            Id = -1;
        }

        public Panel(int id, string name, FormStyle style, int width, int height, Vector2 position, bool showScrollbar, string title = "", bool visible = false, int stroke = 0)
        {
            Id = id;
            Name = name;
            _style = style;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Title = title;
            Visible = visible;
            Stroke = stroke;
            ShowScrollbar = showScrollbar;
            PanelScrollbar = new Scrollbar(1, name + "Scrollbar", Height - Stroke - 14, Height - Stroke - 14, new Vector2(OffsetPosition.X + Width - 10 - Stroke, OffsetPosition.Y + Stroke + 16), null, ParentViewport, Storage.Instance.GetSpriteByName("UI_Scrollbar"), Storage.Instance.GetSpriteByName("UI_Arrow_Up"), Storage.Instance.GetSpriteByName("UI_Arrow_Down"), this);
            //PanelScrollbar.ParentPanel = this;
        }

        public Panel(int id, string name, Texture2D sprite, int width, int height, Vector2 position, bool showScrollbar, string title = "", bool visible = false, int stroke = 0)
        {
            Id = id;
            Name = name;
            BackgroundSprite = sprite;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Title = title;
            Visible = visible;
            Stroke = stroke;
            ShowScrollbar = showScrollbar;
            PanelScrollbar = new Scrollbar(1, name + "Scrollbar", Height - Stroke - 14, Height - Stroke - 14, new Vector2(OffsetPosition.X + Width - 10 - Stroke, OffsetPosition.Y + Stroke + 16), this, ParentViewport, Storage.Instance.GetSpriteByName("UI_Scrollbar"), Storage.Instance.GetSpriteByName("UI_Arrow_Up"), Storage.Instance.GetSpriteByName("UI_Arrow_Down"), this);
            PanelScrollbar.ParentPanel = this;
        }

        public Button AddButton(int id, string name, ButtonStyle style, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            Buttons.Add(new Button(id, name, style, width, height, position, text, visible));
            Button getButton = Buttons[Buttons.Count - 1];
            getButton.ParentPanel = this;
            getButton.ParentScreen = this.ParentScreen;
            getButton.ParentViewport = this.ParentViewport;
            return getButton;
        }

        public Button AddButton(int id, string name, SpriteObject sprite, int width, int height, Vector2 position, string text = "", bool visible = false, SpriteObject pressedSprite = null, int stroke = 0, bool allowRightClick = false)
        {
            Buttons.Add(new Button(id, name, sprite, width, height, position, text, visible, pressedSprite, stroke, allowRightClick));
            Button getButton = Buttons[Buttons.Count - 1];
            getButton.ParentPanel = this;
            getButton.ParentScreen = this.ParentScreen;
            getButton.ParentViewport = this.ParentViewport;
            return getButton;
        }

        public Label AddLabel(int id, string name, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            Labels.Add(new Label(id, name, width, height, position, text, visible));
            Label getLabel = Labels[Labels.Count - 1];
            getLabel.ParentPanel = this;
            getLabel.ParentScreen = this.ParentScreen;
            getLabel.ParentViewport = this.ParentViewport;
            return getLabel;
        }

        public ComboBox AddComboBox(int id, string name, int width, int height, Vector2 position, SpriteObject background, bool visible = false)
        {
            ComboBoxes.Add(new ComboBox(id, name, width, height, position, background, visible));
            ComboBox getComboBox = ComboBoxes[ComboBoxes.Count - 1];
            getComboBox.ParentPanel = this;
            getComboBox.ParentScreen = this.ParentScreen;
            getComboBox.ParentViewport = this.ParentViewport;
            return getComboBox;
        }

        public ListBox AddListBox(int id, string name, int width, int height, Vector2 position, bool visible = false)
        {
            ListBoxes.Add(new ListBox(id, name, width, height, position, visible));
            ListBox getListBox = ListBoxes[ListBoxes.Count - 1];
            getListBox.ParentPanel = this;
            getListBox.ParentScreen = this.ParentScreen;
            getListBox.ParentViewport = this.ParentViewport;
            return getListBox;
        }

        public ImageListBox AddImageListBox(int id, string name, int width, int height, Vector2 position, bool visible = false)
        {
            ImageListBoxes.Add(new ImageListBox(id, name, width, height, position, visible));
            ImageListBox getImageListBox = ImageListBoxes[ImageListBoxes.Count - 1];
            getImageListBox.ParentPanel = this;
            getImageListBox.ParentScreen = this.ParentScreen;
            getImageListBox.ParentViewport = this.ParentViewport;
            return getImageListBox;
        }

        public TextBox AddTextbox(int id, string name, Texture2D sprite, int width, int height, Vector2 position, string text = "", bool visible = false)
        {
            TextBoxes.Add(new TextBox(id, name, sprite, width, height, position, text, visible));
            TextBox getTextbox = TextBoxes[TextBoxes.Count - 1];
            getTextbox.ParentPanel = this;
            getTextbox.ParentScreen = this.ParentScreen;
            getTextbox.ParentViewport = this.ParentViewport;
            return getTextbox;
        }

        public ProgressBar AddProgressBar(int id, string name, Texture2D sprite, Texture2D backgroundSprite, int width, int height, Vector2 position, int borderSize, bool visible = false)
        {
            ProgressBars.Add(new ProgressBar(id, name, sprite, backgroundSprite, width, height, position, borderSize, visible));
            ProgressBar getProgressBar = ProgressBars[ProgressBars.Count - 1];
            getProgressBar.ParentPanel = this;
            getProgressBar.ParentScreen = this.ParentScreen;
            getProgressBar.ParentViewport = this.ParentViewport;
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

        public ImageListBox GetImageListBoxByName(string name)
        {
            for (int i = 0; i < ImageListBoxes.Count; i++)
            {
                if (ImageListBoxes[i].Name.ToLower() == name.ToLower())
                {
                    return ImageListBoxes[i];
                }
            }
            return new ImageListBox();
        }

        public ImageListBox GetImageListBoxById(int id)
        {
            for (int i = 0; i < ImageListBoxes.Count; i++)
            {
                if (ImageListBoxes[i].Id == id)
                {
                    return ImageListBoxes[i];
                }
            }
            return new ImageListBox();
        }

        public TextBox GetTextboxByName(string name)
        {
            for (int i = 0; i < TextBoxes.Count; i++)
            {
                if (TextBoxes[i].Name.ToLower() == name.ToLower())
                {
                    return TextBoxes[i];
                }
            }
            return new TextBox();
        }

        public TextBox GetTextboxById(int id)
        {
            for (int i = 0; i < TextBoxes.Count; i++)
            {
                if (TextBoxes[i].Id == id)
                {
                    return TextBoxes[i];
                }
            }
            return new TextBox();
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
        public ClickEventObject DoEvents(MouseState state, Vector2 cursorPos)
        {
            ClickEventObject returnClicked = new ClickEventObject();
            TextBox focusedTextbox = new TextBox();
            bool tmpOpen = false;
            if (Visible)
            {
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

                for (int i = 0; i < TextBoxes.Count; i++)
                {
                    focusedTextbox = TextBoxes[i].IsClicked(state, cursorPos);
                    if (focusedTextbox.Id != -1)
                    {
                        returnClicked.ClickedTextbox = focusedTextbox;
                    }
                }
                //PanelScrollbar.IsClicked(state, cursorPos, Visible);
            }
            return returnClicked;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameTime gameTime, TextBox focusedTextbox)
        {
            if (Visible)
            {
                Vector2 position;

                if (ParentForm == null)
                {
                    position = OffsetPosition;
                }
                else
                {
                    position = new Vector2(ParentForm.Position.X + OffsetPosition.X, ParentForm.Position.Y + OffsetPosition.Y);
                }

                if (Stroke > 0)
                {
                    spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)position.X, (int)position.Y, Width + (Stroke * 2), Height + (Stroke * 2)), Color.White);
                }

                if (_style.Id != -1)
                {
                    Texture2D spritesheet = Storage.Instance.SpritesheetUi.Texture;
                    spriteBatch.Draw(spritesheet, new Rectangle((int)position.X + Stroke, (int)position.Y + Stroke, Width, Height), new Rectangle(_style.Middle.X + 1, _style.Middle.Y + 1, _style.Middle.Width - 2, _style.Middle.Height - 2), Color.White);
                }
                else
                {
                    spriteBatch.Draw(BackgroundSprite, new Rectangle((int)position.X + Stroke, (int)position.Y + Stroke, Width, Height), Color.White);
                    //_spriteBatch.Draw(BackgroundSprite, new Rectangle((int)Position.X + Stroke, (int)Position.Y + Stroke, Width, (int)font.MeasureString(Title).Y), Color.Gray);
                    //DrawOutlinedString(_spriteBatch, font, Title, new Vector2(Position.X + Stroke + 1, Position.Y + 1), Color.White);
                }
                for (int i = 0; i < Labels.Count; i++)
                {
                    Labels[i].Draw(spriteBatch);
                }
                for(int i = 0; i < ImageListBoxes.Count; i++)
                {
                    ImageListBoxes[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ComboBoxes.Count; i++)
                {
                    ComboBoxes[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ListBoxes.Count; i++)
                {
                    ListBoxes[i].Draw(spriteBatch);
                }
                for (int i = 0; i < ProgressBars.Count; i++)
                {
                    ProgressBars[i].Draw(spriteBatch);
                }
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].Draw(spriteBatch);
                }
                bool isFocused = false;
                for (int i = 0; i < TextBoxes.Count; i++)
                {
                    if (focusedTextbox.Name == TextBoxes[i].Name) { isFocused = true; } else { isFocused = false; }
                    TextBoxes[i].Draw(spriteBatch, gameTime, isFocused);
                }
                if (ShowScrollbar)
                {
                    //PanelScrollbar.Draw(_spriteBatch, Height);
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
    }
}

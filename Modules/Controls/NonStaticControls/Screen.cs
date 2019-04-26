using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Controls.HelperObjects;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Objects.ControlStyles;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class Screen
    {
        public int Id;
        public string Name;
        public List<Form> Forms = new List<Form>();
        public List<Panel> Panels = new List<Panel>();
        public List<Label> Labels = new List<Label>();
        public List<Button> Buttons = new List<Button>();
        public List<TextBox> Textboxes = new List<TextBox>();
        public List<ComboBox> ComboBoxes = new List<ComboBox>();
        public List<ListBox> ListBoxes = new List<ListBox>();
        public List<ImageListBox> ImageListBoxes = new List<ImageListBox>();
        public List<EViewport> Viewports = new List<EViewport>();
        public List<ChatPanel> ChatPanels = new List<ChatPanel>();
        public List<ProgressBar> ProgressBars = new List<ProgressBar>();
        public bool Visible;

        public Screen()
        {
            Id = -1;
        }

        public Screen(int id, string name, bool visible)
        {
            Id = id;
            Name = name;
            Visible = visible;
        }

        public EViewport AddViewport(int id, string name, int x, int y, int width, int height, bool allowResize = false, int minHeight = 0)
        {
            Viewports.Add(new EViewport(id, name, x, y, width, height, allowResize, minHeight));
            EViewport getViewport = Viewports[Viewports.Count - 1];
            getViewport.ParentScreen = this;
            return getViewport;

        }
        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Clear()
        {
            Forms.Clear();
            Panels.Clear();
            Labels.Clear();
            Buttons.Clear();
            Textboxes.Clear();
            ChatPanels.Clear();
            ProgressBars.Clear();
            ListBoxes.Clear();
            ImageListBoxes.Clear();
            ComboBoxes.Clear();
    }

        public EViewport CanResizeViewport(Vector2 cursorPos)
        {
            for (int i = 0; i < Viewports.Count; i++)
            {
                if (Viewports[i].AllowResize)
                {
                    if (cursorPos.Y >= Viewports[i].Viewport.Height - 2 && cursorPos.Y <= Viewports[i].Viewport.Height + 2 && cursorPos.X <= Viewports[i].Viewport.Width + Viewports[i].Viewport.X && cursorPos.X >= Viewports[i].Viewport.X)
                    {
                        return Viewports[i];
                    }
                }
            }
            return new EViewport();
        }

        public Form AddForm(int id, string name, FormStyle style, ButtonStyle titleStyle, int width, int height, Vector2 position, EViewport viewport, string text = "", bool visible = false)
        {
            Forms.Add(new Form(id, name, style, titleStyle, width, height, position, text, visible));
            Form getForm = Forms[Forms.Count - 1];
            getForm.ParentScreen = this;
            getForm.ParentViewport = viewport;
            return getForm;
        }

        public Form AddForm(int id, string name, Texture2D sprite, int width, int height, Vector2 position, EViewport viewport, string text = "", bool visible = false, int stroke = 0)
        {
            Forms.Add(new Form(id, name, sprite, width, height, position, text, visible, stroke));
            Form getForm = Forms[Forms.Count - 1];
            getForm.ParentScreen = this;
            getForm.ParentViewport = viewport;
            return getForm;
        }

        public Button AddButton(int id, string name, ButtonStyle style, int width, int height, Vector2 position, EViewport viewport, string text = "", bool visible = false)
        {
            Buttons.Add(new Button(id, name, style, width, height, position, text, visible));
            Button getButton = Buttons[Buttons.Count - 1];
            getButton.ParentScreen = this;
            getButton.ParentViewport = viewport;
            return getButton;
        }

        public Button AddButton(int id, string name, SpriteObject sprite, int width, int height, Vector2 position, EViewport viewport, string text = "", bool visible = false, SpriteObject pressedSprite = null, int stroke = 0, bool allowRightClick = false)
        {
            Buttons.Add(new Button(id, name, sprite, width, height, position, text, visible, pressedSprite, stroke, allowRightClick));
            Button getButton = Buttons[Buttons.Count - 1];
            getButton.ParentScreen = this;
            getButton.ParentViewport = viewport;
            return getButton;
        }

        public Label AddLabel(int id, string name, int width, int height, Vector2 position, EViewport viewport, string text = "", bool visible = false, bool bigFont = false)
        {
            Labels.Add(new Label(id, name, width, height, position, text, visible, bigFont));
            Label getLabel = Labels[Labels.Count - 1];
            getLabel.ParentScreen = this;
            getLabel.ParentViewport = viewport;
            return getLabel;
        }


        public ImageListBox AddImageListBox(int id, string name, int width, int height, Vector2 position, EViewport viewport, bool visible = false)
        {
            ImageListBoxes.Add(new ImageListBox(id, name, width, height, position, visible));
            ImageListBox getImageListBox = ImageListBoxes[ImageListBoxes.Count - 1];
            getImageListBox.ParentScreen = this;
            getImageListBox.ParentViewport = viewport;
            return getImageListBox;
        }

        public ComboBox AddComboBox(int id, string name, int width, int height, Vector2 position, SpriteObject background, EViewport viewport, bool visible = false)
        {
            ComboBoxes.Add(new ComboBox(id, name, width, height, position, background, visible));
            ComboBox getComboBox = ComboBoxes[ComboBoxes.Count - 1];
            getComboBox.ParentScreen = this;
            getComboBox.ParentViewport = viewport;
            return getComboBox;
        }
        public ListBox AddListBox(int id, string name, int width, int height, Vector2 position, EViewport viewport, bool visible = false)
        {
            ListBoxes.Add(new ListBox(id, name, width, height, position, visible));
            ListBox getListBox = ListBoxes[ListBoxes.Count - 1];
            getListBox.ParentScreen = this;
            getListBox.ParentViewport = viewport;
            return getListBox;
        }



        public TextBox AddTextbox(int id, string name, Texture2D sprite, int width, int height, Vector2 position, EViewport viewport, string text = "", bool visible = false, bool readOnly = false)
        {
            Textboxes.Add(new TextBox(id, name, sprite, width, height, position, text, visible, readOnly));
            TextBox getTextbox = Textboxes[Textboxes.Count - 1];
            getTextbox.ParentScreen = this;
            getTextbox.ParentViewport = viewport;
            return getTextbox;
        }

        public ProgressBar AddProgressBar(int id, string name, Texture2D sprite, Texture2D backgroundSprite, int width, int height, Vector2 position, EViewport viewport, int borderSize, bool visible = false)
        {
            ProgressBars.Add(new ProgressBar(id, name, sprite, backgroundSprite, width, height, position, borderSize, visible));
            ProgressBar getProgressBar = ProgressBars[ProgressBars.Count - 1];
            getProgressBar.ParentScreen = this;
            getProgressBar.ParentViewport = viewport;
            return getProgressBar;
        }

        public ChatPanel AddChatPanel(int id, string name, Texture2D backgroundSprite, EViewport viewport, int width, int height, Vector2 position, bool visible = false)
        {
            ChatPanels.Add(new ChatPanel(id, name, backgroundSprite, width, height, position, viewport, visible));
            ChatPanel getChatPanel = ChatPanels[ChatPanels.Count - 1];
            getChatPanel.ParentScreen = this;
            return getChatPanel;
        }

        public Panel AddPanel(int id, string name, FormStyle style, int width, int height, Vector2 position, EViewport viewport, bool showScrollbar, string text = "", bool visible = false, int stroke = 0)
        {
            Panels.Add(new Panel(id, name, style, width, height, position, showScrollbar, text, visible, stroke));
            Panel getPanel = Panels[Panels.Count - 1];
            getPanel.ParentScreen = this;
            getPanel.ParentViewport = viewport;
            return getPanel;
        }

        public Panel AddPanel(int id, string name, Texture2D backgroundSprite, int width, int height, Vector2 position, EViewport viewport, bool showScrollbar, string text = "", bool visible = false, int stroke = 0)
        {
            Panels.Add(new Panel(id, name, backgroundSprite, width, height, position, showScrollbar, text, visible, stroke));
            Panel getPanel = Panels[Panels.Count - 1];
            getPanel.ParentScreen = this;
            getPanel.ParentViewport = viewport;
            return getPanel;
        }

        public EViewport GetViewportByName(string name)
        {
            for (int i = 0; i < Viewports.Count; i++)
            {
                if (Viewports[i].Name.ToLower() == name.ToLower())
                {
                    return Viewports[i];
                }
            }
            return new EViewport();
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

        public EViewport GetViewportById(int id)
        {
            for (int i = 0; i < Viewports.Count; i++)
            {
                if (Viewports[i].Id == id)
                {
                    return Viewports[i];
                }
            }
            return new EViewport();
        }

        public ChatPanel GetChatPanelByName(string name)
        {
            for (int i = 0; i < ChatPanels.Count; i++)
            {
                if (ChatPanels[i].Name.ToLower() == name.ToLower())
                {
                    return ChatPanels[i];
                }
            }
            return new ChatPanel();
        }

        public ChatPanel GetChatPanelById(int id)
        {
            for (int i = 0; i < ChatPanels.Count; i++)
            {
                if (ChatPanels[i].Id == id)
                {
                    return ChatPanels[i];
                }
            }
            return new ChatPanel();
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

        public Form GetFormByName(string name)
        {
            for (int i = 0; i < Forms.Count; i++)
            {
                if (Forms[i].Name.ToLower() == name.ToLower())
                {
                    return Forms[i];
                }
            }
            return new Form();
        }

        public Form GetFormById(int id)
        {
            for (int i = 0; i < Forms.Count; i++)
            {
                if (Forms[i].Id == id)
                {
                    return Forms[i];
                }
            }
            return new Form();
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

        public void RemoveButton(string name)
        {
            for(int i = 0; i < Buttons.Count; i++)
            {
                if(Buttons[i].Name == name)
                {
                    Buttons.RemoveAt(i);
                }
            }
        }

        public void RemoveLabel(string name)
        {
            for (int i = 0; i < Labels.Count; i++)
            {
                if (Labels[i].Name == name)
                {
                    Labels.RemoveAt(i);
                }
            }
        }

        public ClickEventObject DoEvents(MouseState state, Vector2 cursorPos)
        {
            ClickEventObject tmpClicked = new ClickEventObject();
            ClickEventObject returnClicked = new ClickEventObject();
            bool tmpOpen = false;
            TextBox tmpTextbox = new TextBox();
            if (Visible)
            {
                for (int i = 0; i < Viewports.Count; i++)
                {
                    Viewports[i].IsClicked(state, cursorPos);
                }

                for (int i = 0; i < ChatPanels.Count; i++)
                {
                    ChatPanels[i].DoEvents(state, cursorPos);
                }

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

                for (int i = 0; i < Textboxes.Count; i++)
                {
                    tmpTextbox = Textboxes[i].IsClicked(state, cursorPos);
                    if (tmpTextbox.Id != -1)
                    {
                        returnClicked.ClickedTextbox = tmpTextbox;
                    }
                }

                for(int i = 0; i < ComboBoxes.Count; i++)
                {
                    tmpOpen = ComboBoxes[i].IsClicked(state, cursorPos);
                    if (tmpOpen)
                    {
                        returnClicked.ComboBoxOpen = true;
                    }
                }

                for (int i = 0; i < Forms.Count; i++)
                {
                    tmpClicked = Forms[i].DoEvents(state, cursorPos);
                    if (tmpClicked.ClickedTextbox.Id != -1)
                    {
                        returnClicked.ClickedTextbox = tmpClicked.ClickedTextbox;
                    }

                    if (tmpClicked.ComboBoxOpen)
                    {
                        returnClicked.ComboBoxOpen = true;
                    }
                }

                for (int i = 0; i < Panels.Count; i++)
                {
                    tmpClicked = Panels[i].DoEvents(state, cursorPos);
                    if (tmpClicked.ClickedTextbox.Id != -1)
                    {
                        returnClicked.ClickedTextbox = tmpClicked.ClickedTextbox;
                    }
                    if (tmpClicked.ComboBoxOpen)
                    {
                        returnClicked.ComboBoxOpen = true;
                    }
                }
            }
            return returnClicked; // Change asap
        }

        public void SetupViewport(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.GraphicsDevice.Viewport = viewport;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, SpriteFont bigFont, GameTime gameTime, TextBox focusedTextbox)
        {
            if (Visible)
            {
                for (int f = 0; f < Viewports.Count; f++)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                    spriteBatch.GraphicsDevice.Viewport = Viewports[f].Viewport;
                    for (int i = 0; i < Panels.Count; i++)
                    {
                        if (Panels[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            Panels[i].Draw(spriteBatch, font, gameTime, focusedTextbox);
                        }
                    }
                    for (int i = 0; i < ChatPanels.Count; i++)
                    {
                        if (ChatPanels[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            ChatPanels[i].Draw(spriteBatch, font);
                        }
                    }
                    for (int i = 0; i < ProgressBars.Count; i++)
                    {
                        if (ProgressBars[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            ProgressBars[i].Draw(spriteBatch);
                        }
                    }
                    for (int i = 0; i < Labels.Count; i++)
                    {
                        if (Labels[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            Labels[i].Draw(spriteBatch);
                        }
                    }
                    for (int i = 0; i < ImageListBoxes.Count; i++)
                    {
                        if (ImageListBoxes[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            ImageListBoxes[i].Draw(spriteBatch);
                        }
                    }
                    for (int i = 0; i < ListBoxes.Count; i++)
                    {
                        if (ListBoxes[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            ListBoxes[i].Draw(spriteBatch);
                        }
                    }
                    for (int i = 0; i < Buttons.Count; i++)
                    {
                        if (Buttons[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            Buttons[i].Draw(spriteBatch);
                        }
                    }
                    for (int i = 0; i < Forms.Count; i++)
                    {
                        if (Forms[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            Forms[i].Draw(spriteBatch, gameTime, focusedTextbox);
                        }
                    }
                    bool isFocused = false;
                    for (int i = 0; i < Textboxes.Count; i++)
                    {
                        if (Textboxes[i].ParentViewport.Id == Viewports[f].Id)
                        {
                            if (focusedTextbox.Id != -1 && focusedTextbox.Name.ToLower() == Textboxes[i].Name.ToLower())
                            {
                                isFocused = true;
                            }
                            else
                            {
                                isFocused = false;
                            }
                            Textboxes[i].Draw(spriteBatch, gameTime, isFocused);
                        }
                    }
                }
            }
        }
    }
}

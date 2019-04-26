using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Controls.NonStaticControls;
using RpgMapEditor.Modules.Objects.ControlStyles;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.StaticControls
{
    /*
        Example of how to set up a dialog:

        Dialog.Show("Yes or no?", "Title here", DialogButtons.YesNo);
        Dialog.Click += DoDialog;
        
        private void DoDialog(object sender, EventArgs args)
        {
            DialogButton b = (DialogButton)sender;

            if (b == DialogButton.Yes)
            {
                // User clicked yes
            }
            else if(b == DialogButton.No)
            {
                // User clicked no
            }

            Dialog.Click -= DoDialog;
        }
    */
    public static class Dialog
    {
        public enum DialogButtons
        {
            YesNo,
            YesNoCancel
        }

        public enum DialogButton
        {
            Yes,
            No,
            Cancel,
            None
        }

        public static bool Visible = false;
        private static readonly Storage Storage = Storage.Instance;
        private static readonly Form MsgForm = new Form(1, "MessageBox", Storage.GetSpriteByName("UI_MapEditorBackground2").Sprite, 300, 200, new Vector2(330, 180), "Information", true, 3);
        private static readonly Label MsgLabel = MsgForm.AddLabel(0, "", 200, 150, new Vector2(16, 20), "", true);
        public static Button YesButton = MsgForm.AddButton(1, "Yes", ButtonStyles.LightSmooth, 64, 32, new Vector2(30, 150), "Yes", true);
        public static Button NoButton = MsgForm.AddButton(1, "No", ButtonStyles.LightSmooth, 64, 32, new Vector2(98, 150), "No", true);
        public static Button CancelButton = MsgForm.AddButton(1, "Cancel", ButtonStyles.LightSmooth, 64, 32, new Vector2(214, 150), "Cancel", false);
        private static DialogButton _lastClickedButton;

        public static event EventHandler Click;

        public static void Show(string msg, string title = null, DialogButtons buttons = DialogButtons.YesNo)
        {
            if(title != null)
            {
                MsgForm.Title = title;
            }
            _lastClickedButton = DialogButton.None;
            byte[] bytes = Encoding.Default.GetBytes(msg);
            string text = Encoding.UTF8.GetString(bytes);
            MsgLabel.Text = text;
            YesButton.Click += YesButton_Click;
            NoButton.Click += NoButton_Click;
            CancelButton.Click += CancelButton_Click;

            if (buttons == DialogButtons.YesNoCancel)
            {
                CancelButton.Visible = true;
            }
            else
            {
                CancelButton.Visible = false;
            }

            Visible = true;
        }

        private static void CancelButton_Click(object sender, ButtonEventArgs e)
        {
            Visible = false;
            _lastClickedButton = DialogButton.Cancel;
            OnClick(new EventArgs());
        }

        private static void NoButton_Click(object sender, ButtonEventArgs e)
        {
            Visible = false;
            _lastClickedButton = DialogButton.No;
            OnClick(new EventArgs());
        }

        private static void YesButton_Click(object sender, EventArgs e)
        {
            OnYesButtonClicked();
        }

        public static void OnYesButtonClicked()
        {
            Visible = false;
            _lastClickedButton = DialogButton.Yes;
            OnClick(new EventArgs());
        }

        public static void SetUpScreens(Screen currentScreen)
        {
            YesButton.ParentScreen = currentScreen;
            NoButton.ParentScreen = currentScreen;
            CancelButton.ParentScreen = currentScreen;
        }

        private static void OnClick(EventArgs e)
        {
            EventHandler handler = Click;
            if(handler != null)
            {
                handler(_lastClickedButton, e);
            }
        }

        public static bool IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (Visible)
            {
                MsgForm.IsClicked(state, cursorPos);
                YesButton.IsClicked(state, cursorPos);
                NoButton.IsClicked(state, cursorPos);
                CancelButton.IsClicked(state, cursorPos);
            }
            return false;
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime, TextBox focusedTextbox)
        {
            if (Visible)
            {
                MsgForm.Draw(spriteBatch, gameTime, focusedTextbox);
            }
        }
    }
}

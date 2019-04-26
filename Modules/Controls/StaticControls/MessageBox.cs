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
    public static class MessageBox
    {
        public static bool Visible = false;
        private static Storage _storage = Storage.Instance;
        private static Form _msgForm = new Form(1, "MessageBox", _storage.GetSpriteByName("UI_MapEditorBackground2").Sprite, 300, 200, new Vector2(350, 200), "Information", true, 3);
        private static Label _msgLabel = _msgForm.AddLabel(0, "", 200, 150, new Vector2(16, 20), "", true);
        public static Button MsgButton = _msgForm.AddButton(1, "OK", ButtonStyles.LightSmooth, 64, 32, new Vector2(30, 150), "OK", true);

        public static void SetUpScreens(Screen currentScreen)
        {
            MsgButton.ParentScreen = currentScreen;
        }


        public static void Show(string msg, string title = null)
        {
            if (title != null)
            {
                _msgForm.Title = title;
            }
            byte[] bytes = Encoding.Default.GetBytes(msg);
            string text = Encoding.UTF8.GetString(bytes);
            _msgLabel.Text = text;
            MsgButton.Click += OK_Click;
            Visible = true;
        }

        private static void OK_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        public static bool IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (Visible)
            {
                _msgForm.IsClicked(state, cursorPos);
                MsgButton.IsClicked(state, cursorPos);
            }
            return false;
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime, TextBox focusedTextbox)
        {
            if (Visible)
            {
                _msgForm.Draw(spriteBatch, gameTime, focusedTextbox);
            }
        }
    }
}

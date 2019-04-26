using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Controls.NonStaticControls;

namespace RpgMapEditor.Modules.Controls.StaticControls
{
    public static class StaticControlsManager
    {
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime, TextBox focusedTextbox)
        {
            MessageBox.Draw(spriteBatch, gameTime, focusedTextbox);
            Dialog.Draw(spriteBatch, gameTime, focusedTextbox);
        }

        public static void SetUpScreens(Screen currentScreen)
        {
            MessageBox.SetUpScreens(currentScreen);
            Dialog.SetUpScreens(currentScreen);
        }

        public static bool DoEvents(MouseState state, Vector2 mousePos)
        {
            if (MessageBox.IsClicked(state, mousePos))
            {
                return true;
            }
            if (Dialog.IsClicked(state, mousePos))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnyVisible()
        {
            if(MessageBox.Visible)
            {
                return true;
            }
            if (Dialog.Visible)
            {
                return true;
            }

            return false;
        }

        public static void OnEnterKeyPress()
        {
            if (MessageBox.Visible)
            {
                MessageBox.Visible = false;
            }
            if (Dialog.Visible)
            {
                Dialog.OnYesButtonClicked();
            }
        }
    }
}

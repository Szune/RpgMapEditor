using Microsoft.Xna.Framework;
using RpgMapEditor.Modules.Controls.NonStaticControls;

namespace RpgMapEditor.Modules.Controls
{
    public enum ClickState
    {
        Try,
        Count,
        Dismiss
    }
    public class Control
    {
        public int Id;
        public string Name;
        public int Width;
        public int Height;
        public Vector2 OffsetPosition;
        public Form ParentForm = new Form();
        public Screen ParentScreen = new Screen();
        public EViewport ParentViewport = new EViewport();
        public Panel ParentPanel = new Panel();
        public ClickState CountClick = ClickState.Try;
        public bool Visible;

        public bool HasParentForm()
        {
            return (ParentForm.Id != -1);
        }

        public bool HasParentScreen()
        {
            return (ParentScreen.Id != -1);
        }

        public bool HasParentViewport()
        {
            return (ParentViewport.Id != -1);
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public Vector2 GetCorrectPosition()
        {
            Vector2 position = new Vector2(0, 0);

            if (ParentForm.Id == -1 && ParentPanel.Id == -1)
            {
                position = OffsetPosition;
            }
            else if (ParentPanel.Id != -1)
            {
                if (ParentPanel.ParentForm.Id != -1)
                {
                    position = new Vector2(ParentPanel.ParentForm.Position.X + ParentPanel.OffsetPosition.X + OffsetPosition.X, ParentPanel.ParentForm.Position.Y + ParentPanel.OffsetPosition.Y + OffsetPosition.Y);
                }
                else
                {
                    position = new Vector2(ParentPanel.OffsetPosition.X + OffsetPosition.X, ParentPanel.OffsetPosition.Y + OffsetPosition.Y);
                }
            }
            else if (ParentForm.Id != -1)
            {
                position = new Vector2(ParentForm.Position.X + OffsetPosition.X, ParentForm.Position.Y + OffsetPosition.Y);
            }

            return position;
        }
    }
}

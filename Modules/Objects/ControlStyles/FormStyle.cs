using Microsoft.Xna.Framework;

namespace RpgMapEditor.Modules.Objects.ControlStyles
{
    public class FormStyle : Style
    {
        public Rectangle TopLeftCorner;
        public Rectangle TopRightCorner;
        public Rectangle BottomLeftCorner;
        public Rectangle BottomRightCorner;
        public Rectangle TopBorder;
        public Rectangle BottomBorder;
        public Rectangle LeftBorder;
        public Rectangle RightBorder;

        public FormStyle()
        {
            Id = -1;
        }
    }
}

using RpgMapEditor.Modules.Controls.NonStaticControls;

namespace RpgMapEditor.Modules.Controls.HelperObjects
{
    public class ClickEventObject
    {
        public TextBox ClickedTextbox;
        public bool ComboBoxOpen;

        public ClickEventObject()
        {
            ClickedTextbox = new TextBox();
            ComboBoxOpen = false;
        }

        public ClickEventObject(TextBox clickedTextbox, bool comboBoxOpen)
        {
            ClickedTextbox = clickedTextbox;
            ComboBoxOpen = comboBoxOpen;
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using RpgMapEditor.Modules.Entities;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Objects
{
    public class ChatMessage
    {
        public int Id;
        public string Text;
        public Creature ChatPlayer;
        public int LineCount;

        public ChatMessage(int id, Creature chatPlayer, string text)
        {
            Id = id;
            Text = text;
            ChatPlayer = chatPlayer;
        }

        public ChatMessage(int id, Creature chatPlayer, string text, SpriteFont font, float maxLineWidth)
        {
            Id = id;
            Text = text;
            ChatPlayer = chatPlayer;
            LineCount = WordWrap.WrapText(font, text, maxLineWidth).Split('\n').Length - 1;
        }
    }
}

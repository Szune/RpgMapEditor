namespace RpgMapEditor.Modules.Objects
{
    public class ImageListItem
    {
        public int Id;
        public string Text;
        public SpriteObject Sprite;
        public SpriteCollection SpriteCollection;

        public ImageListItem()
        {
            Id = -1;
        }

        public ImageListItem(int id, string text, SpriteObject sprite, SpriteCollection spriteCollection = null)
        {
            Id = id;
            Text = text;
            Sprite = sprite;
            SpriteCollection = spriteCollection;
        }

    }
}

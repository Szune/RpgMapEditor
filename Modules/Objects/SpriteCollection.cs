namespace RpgMapEditor.Modules.Objects
{
    public class SpriteCollection
    {
        public int Id;
        public string Name;
        public int Columns;
        public int Rows;
        public SpriteObject[,] Sprite;
        public SpriteCategory Category;

        public SpriteCollection()
        {
            Id = -1;
        }

        public SpriteCollection(int id, string name, int columns, int rows, SpriteCategory category)
        {
            Id = id;
            Name = name;
            Columns = columns;
            Rows = rows;
            Sprite = new SpriteObject[rows,columns];
            Category = category;
        }

        public void AddSprite(SpriteObject sprite, int column, int row)
        {
            Sprite[row, column] = sprite;
        }
    }
}

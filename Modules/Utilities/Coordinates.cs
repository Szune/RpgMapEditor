namespace RpgMapEditor.Modules.Utilities
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public const int Step = 16;

        public Coordinates Parent { get; set; }

        public Coordinates() { X = 0; Y = -1; Z = 0; }

        public Coordinates(Node node)
        {
            X = node.X;
            Y = node.Y;
            Z = node.Z;
        }

        public Coordinates(int x, int y) { X = x; Y = y; }

        public Coordinates(int x, int y, int z) { X = x; Y = y; Z = z; }

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }

        public static Coordinates Parse(Node node)
        {
            return new Coordinates(node);
        }
    }
}

namespace RpgMapEditor.Modules.Utilities
{
    public class Node
    {

        public override int GetHashCode()
        {
            int hash = 14;
            hash = hash * 13 * X;
            hash = hash * 13 * Y;
            return hash;
        }
        public override bool Equals(object obj)
        {
            return Equals((Node)obj);
        }

        public bool Equals(Node obj)
        {
            return (X == obj.X && Y == obj.Y);
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int G { get; set; }
        public int F { get; set; } // H + G (total movement cost)

        public Node Parent { get; set; }

        public Node() { X = 0; Y = 0; Z = 0; }

        public Node(int x, int y) { X = x; Y = y; }

        public Node(int x, int y, int z) { X = x; Y = y; Z = z; }

        public Node(int x, int y, int z, int f, int g, Node parent) { X = x; Y = y; Z = z; F = f + g; G = g; Parent = parent; }

        public bool HasParent()
        {
            return !object.Equals(Parent, default(Node));
        }
    }
}

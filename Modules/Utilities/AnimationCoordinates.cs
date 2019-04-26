namespace RpgMapEditor.Modules.Utilities
{
    public class AnimationCoordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public const double Step = 32.0;

        public AnimationCoordinates Parent { get; set; }

        public AnimationCoordinates() { X = 0; Y = -1; Z = 0; }

        public AnimationCoordinates(Node node)
        {
            X = node.X;
            Y = node.Y;
            Z = node.Z;
        }


        public AnimationCoordinates(double x, double y) { X = x; Y = y; }

        public AnimationCoordinates(double x, double y, double z) { X = x; Y = y; Z = z; }


        public override string ToString()
        {
            return "X: " + X.ToString() + " Y: " + Y.ToString() + " Z: " + Z.ToString();
        }

        public static Coordinates Parse(Node node)
        {
            return new Coordinates(node);
        }
    }
}

using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Objects
{

    public enum Direction
    {
        None,
        North,
        East,
        South,
        West
    }

    public class Movement
    {
        public Animation CurrentAnimation = new Animation();
        public Direction MoveDirection = Direction.None;
        public Coordinates Destination = new Coordinates();

        public Movement(Direction moveDirection, Coordinates destination, Animation currentAnimation)
        {
            MoveDirection = moveDirection;
            Destination = destination;
            CurrentAnimation = currentAnimation;
        }
    }
}

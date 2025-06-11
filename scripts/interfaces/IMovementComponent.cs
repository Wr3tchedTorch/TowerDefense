using Godot;

namespace Game.scripts
{
    public interface IMovementComponent
    {
        public void Stop();
        public void Move(Vector2 targetPosition);
    }
}
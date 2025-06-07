using Godot;

public partial class GunBarrel : Node2D
{
    [Export] private float recoilDistance = 10f;
    [Export] private float returnBackVelocity = 0.2f;

    private Vector2 originalPosition;
    private Vector2 recoilPosition;
    private bool isInRecoil;

    public override void _Ready()
    {
        originalPosition = Position;
    }

    public override void _Process(double delta)
    {
        if (!isInRecoil)
        {
            if (!Position.IsEqualApprox(originalPosition))
            {
                Position = Position.Lerp(originalPosition, returnBackVelocity);
            }
            return;
        }

        if (Position.IsEqualApprox(recoilPosition))
        {
            isInRecoil = false;
            return;
        }

        var newPosition = Position.Lerp(recoilPosition, returnBackVelocity*3);
        var maxDistance = originalPosition.DistanceTo(recoilPosition);
        
        if (newPosition.DistanceTo(originalPosition) <= maxDistance)
        {
            Position = newPosition;
        }
        else
        {
            isInRecoil = false;
        }
    }

    public void OnShooting()
    {
        if (isInRecoil)
        {
            return;
        }

        var angleRadians = Mathf.DegToRad(GlobalRotation);
        var direction = Vector2.Right.Rotated(angleRadians).Normalized();

        var clampedRecoilDistance = Mathf.Min(recoilDistance, 50f);
        recoilPosition = Position - direction * clampedRecoilDistance;

        if (recoilPosition.DistanceTo(originalPosition) > clampedRecoilDistance * 1.5f)
        {
            recoilPosition = Position - direction * (clampedRecoilDistance * 0.5f);
        }
        
        isInRecoil = true;
    }
}

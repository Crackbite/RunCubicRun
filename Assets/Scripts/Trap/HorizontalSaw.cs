public class HorizontalSaw : Saw
{
    protected override void CompleteCollision()
    {
        IsSideCollision = false;
    }
}

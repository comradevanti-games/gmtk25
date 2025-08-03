namespace GMTK25.Bullets.Return
{
    public enum ReturnAction
    {
        Consume,
        BecomePickup,
        Return
    }

    public interface IReturnFilter
    {
        public ReturnAction ShouldReturn(BulletHit hit);
    }
}
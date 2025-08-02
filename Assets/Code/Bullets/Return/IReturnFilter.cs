namespace GMTK25.Bullets.Return
{
    public interface IReturnFilter
    {
        public bool ShouldReturn(BulletHit hit);
    }
}
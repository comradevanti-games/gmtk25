namespace GMTK25.Bullets
{
    public interface IContinueFilter
    {
        public bool ShouldContinue(BulletHit hit);
    }
}
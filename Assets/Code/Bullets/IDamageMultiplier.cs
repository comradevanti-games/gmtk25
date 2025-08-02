namespace GMTK25.Bullets
{
    public interface IDamageMultiplier
    {
        public float CalcMultiplier(BulletHit hit);
    }
}
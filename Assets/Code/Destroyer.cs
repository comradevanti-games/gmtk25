using UnityEngine;

namespace GMTK25
{
    public sealed class Destroyer : MonoBehaviour
    {
        public void DestroyMePleaseThanks()
        {
            Destroy(gameObject);
        }
    }
}
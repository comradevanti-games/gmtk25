using UnityEngine;
using UnityEngine.Events;

namespace GMTK25
{
    public sealed class ShootTrigger : MonoBehaviour
    {
        public UnityEvent shot = new UnityEvent();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Bullet"))
                shot.Invoke();
        }
    }
}
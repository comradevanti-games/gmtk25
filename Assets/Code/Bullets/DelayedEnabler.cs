using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class DelayedEnabler : MonoBehaviour
    {
        [SerializeField] private int delayFrames;
        [SerializeField] private Behaviour component = null!;

        private int frame;

        private void Update()
        {
            frame++;
            if (delayFrames != frame) return;

            component.enabled = true;
            Destroy(this);
        }
    }
}
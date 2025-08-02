using UnityEngine;
using UnityEngine.UI;

namespace GMTK25.UI
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Image heartImage = null!;

        public void OnHealthChanged(float newHealth)
        {
            heartImage.fillAmount =
                System.Convert.ToSingle(1) / 3 * (int)newHealth;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace GMTK25 {

    public class HealthDisplayUI : MonoBehaviour {

        [SerializeField] private Image heartImage = null!;
        [SerializeField] private int initialHealth = 3;

        public void OnHealthChanged(float newHealth) {
            heartImage.fillAmount = (System.Convert.ToSingle(1) / 3) * (int)newHealth;
        }

    }

}
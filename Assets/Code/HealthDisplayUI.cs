using UnityEngine;
using UnityEngine.UI;

namespace GMTK25 {

    public class HealthDisplayUI : MonoBehaviour {

        [SerializeField] private Image heartImage = null!;

        public void OnHealthChanged(float newHealth) {
            heartImage.fillAmount = (System.Convert.ToSingle(1) / 3) * (int)newHealth;
        }

    }

}
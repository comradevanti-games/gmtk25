using TMPro;
using UnityEngine;

namespace GMTK25.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class FloatDisplay : MonoBehaviour
    {
        [SerializeField] private string format = "";

        private TMP_Text displayText = null!;

        public float Value
        {
            set => displayText.text = value.ToString(format);
        }

        private void Awake()
        {
            displayText = GetComponent<TMP_Text>();
        }
    }
}
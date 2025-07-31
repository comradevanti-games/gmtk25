using UnityEngine;

namespace GMTK25
{
    [CreateAssetMenu(fileName = "New Enemy-type",
        menuName = "GMTK25/Enemy-type")]
    public sealed class EnemyType : ScriptableObject
    {
        [SerializeField] private GameObject prefab = null!;

        public GameObject Prefab => prefab;
    }
}
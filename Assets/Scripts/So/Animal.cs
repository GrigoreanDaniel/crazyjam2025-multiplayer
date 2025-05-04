
using UnityEngine;

namespace SO_Characters
{
    [CreateAssetMenu(fileName = "Animal", menuName = "Characters /Animal")]
    public class Animal : ScriptableObject
    {
        [Header("Animal Information")]
        public string animalName; // Name of the animal
        public Sprite animalSprite; // Reference to the animal sprite
        public GameObject animalPrefab; // Reference to the animal prefab

    }
}

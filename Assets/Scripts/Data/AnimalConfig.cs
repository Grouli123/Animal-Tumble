using UnityEngine;
using UnityEngine.Serialization;

namespace Animal.Data
{
    [CreateAssetMenu(fileName = "AnimalConfig", menuName = "Game/AnimalConfig")]
    public class AnimalConfig : ScriptableObject
    {
        [FormerlySerializedAs("shape")] public AnimalShape animalShape;
        [FormerlySerializedAs("frameColor")] public AnimalFrameColor animalFrameColor;
        public AnimalType animalType;

        public Sprite sprite;
        public GameObject prefab;
    }
}
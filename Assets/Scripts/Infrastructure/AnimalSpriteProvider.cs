using UnityEngine;
using Animal.Data;

namespace Animal.Infrastructure
{
    [CreateAssetMenu(
        fileName = "AnimalSpriteProvider",
        menuName = "Game/Infrastructure/Animal Sprite Provider")]
    public sealed class AnimalSpriteProvider : ScriptableObject, IAnimalSpriteProvider
    {
        [SerializeField] private AnimalConfigDatabase configDatabase;

        public Sprite GetSprite(AnimalData data)
        {
            if (configDatabase == null)
            {
                Debug.LogWarning("[AnimalSpriteProvider] ConfigDatabase not assigned.");
                return null;
            }

            var cfg = configDatabase.GetConfig(data);
            return cfg != null ? cfg.sprite : null;
        }
    }
}
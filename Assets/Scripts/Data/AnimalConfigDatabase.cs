using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Animal.Data
{
    [CreateAssetMenu(menuName = "Animal/Create Animal Config Database")]
    public class AnimalConfigDatabase : ScriptableObject
    {
        [SerializeField] private List<AnimalConfig> configs;

        public AnimalConfig GetConfig(AnimalData data)
        {
            return configs.FirstOrDefault(cfg =>
                cfg.animalShape == data.shape &&
                cfg.animalFrameColor == data.frameColor &&
                cfg.animalType == data.animalType);
        }

        public List<AnimalConfig> GetAllConfigs()
        {
            return new List<AnimalConfig>(configs);
        }
    }
}
using UnityEngine;
using Animal.Data;

namespace Animal.Infrastructure
{
    public interface IAnimalSpriteProvider
    {
        Sprite GetSprite(AnimalData data);
    }
}
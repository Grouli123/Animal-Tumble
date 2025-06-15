using System;

namespace Animal.Data
{
    [Serializable]
    public struct AnimalData
    {
        public AnimalShape shape;
        public AnimalFrameColor frameColor;
        public AnimalType animalType;

        public AnimalData(AnimalShape shape, AnimalFrameColor frameColor, AnimalType animalType)
        {
            this.shape = shape;
            this.frameColor = frameColor;
            this.animalType = animalType;
        }

        public override bool Equals(object obj)
        {
            return obj is AnimalData other &&
                   shape == other.shape &&
                   frameColor == other.frameColor &&
                   animalType == other.animalType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(shape, frameColor, animalType);
        }
    }
}
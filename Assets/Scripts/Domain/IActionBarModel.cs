using System;
using System.Collections.Generic;
using Animal.Data;

namespace Animal.Domain
{
    public interface IActionBarModel
    {
        IReadOnlyList<AnimalData> Data { get; }
        event Action Changed;

        bool TryAdd(AnimalData data);
        void RemoveLast();
        bool TryMatchThree();      
        void Clear();
        
        AnimalData? PopLast();
        void ClearBar();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Animal.Data;

namespace Animal.Domain
{
    public sealed class ActionBarModel : IActionBarModel
    {
        public ActionBarModel(int capacity) => this.capacity = capacity;
        public IReadOnlyList<AnimalData> Data => data;
        public event Action Changed;
        
        private readonly List<AnimalData> data = new();
        private readonly int capacity;

        public bool TryAdd(AnimalData d)
        {
            if (data.Count >= capacity) return false;
            data.Add(d);
            Changed?.Invoke();
            return true;
        }

        public void RemoveLast()
        {
            if (data.Count == 0) return;
            data.RemoveAt(data.Count - 1);
            Changed?.Invoke();
        }

        public bool TryMatchThree()
        {
            var grp = data.GroupBy(x => x).FirstOrDefault(g => g.Count() >= 3);
            if (grp == null) return false;

            for (int i = 0, removed = 0; i < data.Count && removed < 3; ++i)
            {
                if (!data[i].Equals(grp.Key)) continue;
                data.RemoveAt(i--);
                removed++;
            }
            Changed?.Invoke();
            return true;
        }

        public void Clear()
        {
            data.Clear();
            Changed?.Invoke();
        }

        public AnimalData? PopLast()
        {
            if (data.Count == 0) return null;

            var last = data[^1];
            data.RemoveAt(data.Count - 1);
            Changed?.Invoke();
            return last;
        }

        public void ClearBar()
        {
            data.Clear();
            Changed?.Invoke();
        }
    }
}
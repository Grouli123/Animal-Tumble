using UnityEngine;
using UnityEngine.UI;
using Animal.Domain;      
using Animal.Infrastructure;  
using Animal.Data;
using Animal.Services;

namespace Animal.Presentation
{
    public sealed class ActionBarView : MonoBehaviour
    {
        public static ActionBarView Instance { get; private set; }

        [Header("UI-слоты")]
        [SerializeField] private Image[] slots;         
        [SerializeField] private Sprite emptySlotSprite; 

        [Header("Сервис спрайтов")]
        [SerializeField] private AnimalSpriteProvider spriteProvider; 

        private IActionBarModel model;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public int SlotCount => slots.Length;

        public void Init(IActionBarModel barModel)
        {
            model = barModel;
            model.Changed += Refresh;
            Refresh();                 
        }

        private void Refresh()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                if (i < model.Data.Count)
                {
                    AnimalData d = model.Data[i];
                    slots[i].sprite = spriteProvider.GetSprite(d);
                }
                else
                {
                    slots[i].sprite = emptySlotSprite;
                }
            }
        }

        public bool TryAdd(AnimalData data)
        {
            if (!model.TryAdd(data)) return false;

            bool wasMatch = model.TryMatchThree();
            Refresh();

            if (wasMatch)
            {
                Debug.Log("✅ Собрана тройка и удалена!");
            }

            if (!wasMatch && model.Data.Count >= slots.Length)
            {
                Debug.Log("Поражение! Бар полностью забит.");
                GameService.Instance.TriggerLose();
                return true;
            }
            if (!GameService.Instance.IsRefilling && model.Data.Count == 0 && GameService.Instance.NoFiguresLeft())
            {
                Debug.Log("Победа! Всё очищено.");
                GameService.Instance.TriggerWin();
            }

            return true;
        }

        public bool BarIsEmpty()
        {
            return model.Data.Count == 0;
        }

        public AnimalData? PopLast() => model.PopLast();

        public void ClearBar() => model.Clear();
    }
}
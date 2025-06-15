using UnityEngine;
using Animal.Presentation;
using Animal.Services;

namespace Animal.Data
{
    public class AnimalView : MonoBehaviour
    {
        private AnimalData animalData;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Init(AnimalData data, Sprite sprite)
        {
            animalData = data;

            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
                spriteRenderer.sprite = sprite;
        }

        public AnimalData Data => animalData;

        private void OnMouseDown()
        {
            if (ActionBarView.Instance != null)
            {
                ActionBarView.Instance.TryAdd(animalData);
                Destroy(gameObject);
            }
        }
        
        private void OnDestroy()
        {
            if (GameService.Instance == null)
                return;

            GameService.Instance.Decrement();

            if (GameService.Instance.NoFiguresLeft() && ActionBarView.Instance != null)
            {
                bool barIsEmpty = ActionBarView.Instance.BarIsEmpty(); 
                if (barIsEmpty)
                {
                    GameService.Instance.TriggerWin();
                }
            }
        }
    }
}
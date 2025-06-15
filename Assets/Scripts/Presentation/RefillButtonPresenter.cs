using UnityEngine;
using UnityEngine.UI;
using Animal.Services;

namespace Animal.Presentation
{
    public sealed class RefillButtonPresenter : MonoBehaviour
    {
        [SerializeField] private Button button;
        private IRefillService refill;

        public void Init(IRefillService refillService)
        {
            refill = refillService;
            button.onClick.AddListener(OnClick);
        }

        private void OnClick() => refill.Refill();

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
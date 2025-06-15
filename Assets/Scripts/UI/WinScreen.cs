using UnityEngine;
using UnityEngine.UI;
using Animal.GameCore;

namespace UI
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private GameObject screenRoot;
        [SerializeField] private Button continueButton;

        public void Show()
        {
            screenRoot.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinue);
        }

        private void OnContinue()
        {
            screenRoot.SetActive(false);
            GameManager.Instance.ShowRefillButton();
        }
    }
}
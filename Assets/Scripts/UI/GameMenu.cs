using Presentation;
using UnityEngine;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private GameplayPresenter _gameplayPresenter;
        [SerializeField] private GameObject _startGameButton;
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private GameObject _gameOverText;
        [SerializeField] private GameObject _upgradeMenu;

        public void Start()
        {
            gameObject.SetActive(true);
            _gameplayPresenter.OnLose += ShowGameOver;
            _gameplayPresenter.OnPause += ShowPause;
        }

        public void OnStartGameClick()
        {
            gameObject.SetActive(false);
            _gameplayPresenter.StartGame();
            _upgradeMenu.SetActive(true);
        }
        
        public void OnContinueGameClick()
        {
            gameObject.SetActive(false);
            _gameplayPresenter.ContinueGame();
            _upgradeMenu.SetActive(true);
        }

        private void ShowGameOver()
        {
            gameObject.SetActive(true);
            _startGameButton.SetActive(true);
            _continueButton.SetActive(false);
            _gameOverText.SetActive(true);
            _upgradeMenu.SetActive(false);
        }
        
        private void ShowPause()
        {
            gameObject.SetActive(true);
            _startGameButton.SetActive(false);
            _continueButton.SetActive(true);
            _gameOverText.SetActive(false);
            _upgradeMenu.SetActive(false);
        }
    }
}


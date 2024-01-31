using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace DO.Asteroids.Hybrid
{
    [RequireComponent(typeof(UIDocument))]
    public class GameOverScreen : MonoBehaviour
    {
        private VisualElement _gameOverContainer;
        private Label _gameOverLabel;
        private VisualElement _scoreContainer;
        private TextField _initialsTextField;
        private Button _submitScoreButton;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _gameOverContainer = root.Q<VisualElement>("game-over__container");
            _gameOverLabel = root.Q<Label>("game-over-label");
            _scoreContainer = root.Q<VisualElement>("game-over-score-container");
            _initialsTextField = root.Q<TextField>("initials-text-field");
            _submitScoreButton = root.Q<Button>("submit-score-button");
        }

        private void Start()
        {
            RegisterCallbacks();
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            if (HybridSignalBus.Instance != null)
            {
                HybridSignalBus.Instance.OnGameStateChange += OnGameStateChange;
            }
            
            _submitScoreButton.RegisterCallback<ClickEvent>(ev =>
            {
                // TODO: Submit score to the server 
                HybridSignalBus.Instance.OnGameStateChange?.Invoke(GameState.Menu);
                
                _gameOverContainer.style.display = DisplayStyle.None;
                _gameOverLabel.style.display = DisplayStyle.Flex;
            });
        }

        private void UnregisterCallbacks()
        {
            HybridSignalBus.Instance.OnGameStateChange -= OnGameStateChange;
        }

        private void OnGameStateChange(GameState gameState)
        {
            if (gameState == GameState.GameOver)
            {
                _gameOverContainer.style.display = DisplayStyle.Flex;
                _scoreContainer.style.display = DisplayStyle.None;
                StartCoroutine(ShowScoreContainer());
            }
        }

        private IEnumerator ShowScoreContainer()
        {
            yield return new WaitForSeconds(2f);
            _gameOverLabel.style.display = DisplayStyle.None;
            _scoreContainer.style.display = DisplayStyle.Flex;
            _initialsTextField.Focus();
        }
    }
}
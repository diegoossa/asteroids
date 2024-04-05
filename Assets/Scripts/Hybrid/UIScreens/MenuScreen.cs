using UnityEngine;
using UnityEngine.UIElements;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// Class that represents the menu screen.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class MenuScreen : MonoBehaviour
    {
        private VisualElement _menuContainer;
        private Button _playButton;
        private Button _highScoresButton;
        private Button _exitButton;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _menuContainer = root.Q<VisualElement>("menu__container");
            _playButton = root.Q<Button>("play-button");
            _highScoresButton = root.Q<Button>("high-scores-button");
            _exitButton = root.Q<Button>("exit-button");
        }
        
        private void Start()
        {
            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
            _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
            HybridSignalBus.Instance.OnGameStateChange += OnGameStateChange;
        }

        private void OnExitButtonClicked(ClickEvent evt)
        {
            // TODO: Add modal confirmation dialog
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnPlayButtonClicked(ClickEvent evt)
        {
            HybridSignalBus.Instance.OnGameStateChange?.Invoke(GameState.Play);
            _menuContainer.style.display = DisplayStyle.None;
        }
        
        private void OnGameStateChange(GameState gameState)
        {
            if (gameState == GameState.Menu)
            {
                _menuContainer.style.display = DisplayStyle.Flex;
            }
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
            HybridSignalBus.Instance.OnGameStateChange -= OnGameStateChange;
        }
    }
}
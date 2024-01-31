using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    [RequireComponent(typeof(UIDocument))]
    public class MenuScreen : MonoBehaviour
    {
        private SignalBus _signalBus;

        private VisualElement _menuContainer;
        private Button _playButton;
        private Button _highScoresButton;
        private Button _exitButton;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _menuContainer = root.Q<VisualElement>("menu__container");
            _playButton = root.Q<Button>("play-button");
            _highScoresButton = root.Q<Button>("high-scores-button");
            _exitButton = root.Q<Button>("exit-button");

            RegisterCallbacks();
        }

        #region Events

        private void RegisterCallbacks()
        {
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
            _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
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
            _menuContainer.style.display = DisplayStyle.None;
            HybridSignalBus.Instance.OnGameStateChange?.Invoke(GameState.Play);
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
        }

        #endregion
    }
}
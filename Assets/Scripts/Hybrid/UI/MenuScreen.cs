using System;
using MessagePipe;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace DO.Asteroids.Hybrid
{
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
            
            RegisterCallbacks();
        }

        #region Events
        
        private void RegisterCallbacks()
        {
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked(ClickEvent evt)
        {
            _menuContainer.style.display = DisplayStyle.None;
            HybridMessageBus.Instance.OnGameStateChange?.Invoke(GameState.Play);
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


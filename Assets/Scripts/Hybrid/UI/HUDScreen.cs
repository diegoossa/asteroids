using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    [RequireComponent(typeof(UIDocument))]
    public class HUDScreen : MonoBehaviour
    {
        private VisualElement _hudContainer;
        private VisualElement _lifeContainer;
        private Label _scoreLabel;
        [SerializeField] private VisualTreeAsset lifeIconTemplate;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _hudContainer = root.Q<VisualElement>("hud__container");
            _lifeContainer = root.Q<VisualElement>("life-container");
            _scoreLabel = root.Q<Label>("score-value-label");
        }

        private void Start()
        {
            RegisterCallbacks();
        }

        #region Events

        private void OnDisable()
        {
            UnregisterCallbacks();
        }
        
        private void RegisterCallbacks()
        {
            if (HybridSignalBus.Instance != null)
            {
                HybridSignalBus.Instance.OnGameStateChange += OnGameStateChange;
                HybridSignalBus.Instance.OnLivesChange += OnLivesChange;
                HybridSignalBus.Instance.OnScoreChange += OnScoreChange;
            }
        }

        private void UnregisterCallbacks()
        {
            HybridSignalBus.Instance.OnGameStateChange -= OnGameStateChange;
            HybridSignalBus.Instance.OnLivesChange -= OnLivesChange;
            HybridSignalBus.Instance.OnScoreChange -= OnScoreChange;
        }

        #endregion
        
        private void OnGameStateChange(GameState state)
        {
            _hudContainer.style.display = state == GameState.Play ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private void OnScoreChange(int value)
        {
            _scoreLabel.text = value.ToString();
            _scoreLabel.experimental.animation.Scale(1.2f, 200).Ease(Easing.OutBack)
                .OnCompleted(() => _scoreLabel.experimental.animation.Scale(1f, 50));
        }

        private void OnLivesChange(int value)
        {
            _lifeContainer.Clear();
            for (var i = 0; i < value; i++)
            {
                _lifeContainer.Add(lifeIconTemplate.CloneTree());
            }
        }
    }
}
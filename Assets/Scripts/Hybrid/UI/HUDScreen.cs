using System;
using MessagePipe;
using UnityEngine;
using UnityEngine.UIElements;

namespace DO.Asteroids.Hybrid
{
    [RequireComponent(typeof(UIDocument))]
    public class HUDScreen : MonoBehaviour
    {
        private VisualElement _hudContainer;
        
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _hudContainer = root.Q<VisualElement>("hud__container");
            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            HybridMessageBus.Instance.OnGameStateChange += OnGameStarted;
        }
       
        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            HybridMessageBus.Instance.OnGameStateChange -= OnGameStarted;
        }

        private void OnGameStarted(GameState gameState)
        {
            _hudContainer.style.display = gameState == GameState.Play ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
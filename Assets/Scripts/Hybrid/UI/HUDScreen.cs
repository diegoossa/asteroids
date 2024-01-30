using UnityEngine;
using UnityEngine.UIElements;

namespace DO.Asteroids.Hybrid
{
    [RequireComponent(typeof(UIDocument))]
    public class HUDScreen : MonoBehaviour
    {
        // private SignalBus _signalBus;
        
        private VisualElement _hudContainer;
        
        // [Inject]
        // public void Construct(SignalBus signalBus)
        // {
        //     _signalBus = signalBus;
        // }

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _hudContainer = root.Q<VisualElement>("hud__container");
            
            RegisterCallbacks();
        }

        #region Events
        
        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            //_signalBus.Unsubscribe<StartGameSignal>(OnGameStarted);
        }

        #endregion
        private void RegisterCallbacks()
        {
            //_signalBus.Subscribe<StartGameSignal>(OnGameStarted);
        }

        private void OnGameStarted()
        {
            _hudContainer.style.display = DisplayStyle.Flex;
        }
    }
}
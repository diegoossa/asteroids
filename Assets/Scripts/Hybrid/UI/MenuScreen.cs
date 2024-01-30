using UnityEngine;
using UnityEngine.UIElements;

namespace DO.Asteroids.Hybrid
{
    [RequireComponent(typeof(UIDocument))]
    public class MenuScreen : MonoBehaviour
    {
        // private SignalBus _signalBus;
        
        private VisualElement _menuContainer;
        private Button _playButton;
        private Button _highScoresButton;
        private Button _exitButton;
        
        // [Inject]
        // public void Construct(SignalBus signalBus)
        // {
        //     _signalBus = signalBus;
        // }

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
            //_signalBus.Fire<StartGameSignal>();
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


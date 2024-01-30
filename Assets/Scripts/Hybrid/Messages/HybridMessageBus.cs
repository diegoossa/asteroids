using System;
using UnityEngine;
using VContainer;

namespace DO.Asteroids.Hybrid
{
    public class HybridMessageBus : MonoBehaviour
    {
        public static HybridMessageBus Instance { get; private set; }
        public Action OnStartGame;
        public Action<GameState> OnGameStateChange;

        private void Awake()
        {
            Instance = this;
        }
        
        [Inject]
        public void Construct()
        {
        }
    }
}
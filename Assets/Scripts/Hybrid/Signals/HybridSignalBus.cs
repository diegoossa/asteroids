using System;
using UnityEngine;

namespace DO.Asteroids.Hybrid
{
    public enum GameState
    {
        None,
        Menu,
        Play,
        Pause,
        GameOver
    }
    
    /// <summary>
    /// Public static delegates to communicate between ECS and Hybrid systems (MonoBehaviours)
    /// </summary>
    public class HybridSignalBus : MonoBehaviour
    {
        public Action<GameState> OnGameStateChange;
        public Action OnSpawnShip;
        public Action<int> OnScoreChange;
        public Action<int> OnLivesChange;
        public Action<Vector2> OnSpawnExplosion;
        
        public static HybridSignalBus Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
using System;
using UnityEngine;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// Public static delegates to communicate between ECS and Hybrid systems (MonoBehaviours)
    /// </summary>
    public static class HybridSignalBus
    {
        public static Action<GameState> OnGameStateChange;
        public static Action OnSpawnShip;
        public static Action<int> OnScoreChange;
        public static Action<int> OnLivesChange;
        public static Action<Vector2> OnSpawnExplosion;
    }
}
﻿using System;
using Unity.Mathematics;
using UnityEngine;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// HybridSignalBus is used to communicate between ECS and Hybrid Components
    /// </summary>
    public class HybridSignalBus : MonoBehaviour
    {
        public static HybridSignalBus Instance { get; private set; }
        
        public Action<GameState> OnGameStateChange;
        public Action<float2> OnSpawnExplosion;

        private void Awake()
        {
            Instance = this;
        }
    }
}
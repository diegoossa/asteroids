﻿using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// System that updates the score of the player.
    /// </summary>
    [UpdateAfter(typeof(AsteroidDamageSystem))]
    public partial struct UpdateScoreSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Score>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var score = SystemAPI.GetSingletonRW<Score>();
            if (score.ValueRO.Value != score.ValueRO.LastScore)
            {
                score.ValueRW.LastScore = score.ValueRO.Value;
                HybridSignalBus.Instance.OnScoreChange?.Invoke(score.ValueRO.Value);
            }
        }
    }
}
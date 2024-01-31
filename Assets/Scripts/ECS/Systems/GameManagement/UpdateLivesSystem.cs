using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    [UpdateAfter(typeof(ShipDamageSystem))]
    public partial struct UpdateLivesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Lives>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var lives = SystemAPI.GetSingletonRW<Lives>();
            if (lives.ValueRO.CurrentLives != lives.ValueRO.LastLives)
            {
                lives.ValueRW.LastLives = lives.ValueRO.CurrentLives;
                if (HybridSignalBus.Instance != null)
                {
                    HybridSignalBus.Instance.OnLivesChange?.Invoke(lives.ValueRO.CurrentLives);
                }
            }
        }
    }
}
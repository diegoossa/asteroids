using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// System that spawns the ship.
    /// </summary>
    public partial struct SpawnShipSystem : ISystem
    {
        private bool _shouldSpawnCached;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameManager>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var gameManager = SystemAPI.GetSingletonRW<GameManager>();
            if (!gameManager.ValueRO.ShouldSpawn)
                return;

            if (_shouldSpawnCached != gameManager.ValueRO.ShouldSpawn)
            {
                HybridSignalBus.Instance.OnSpawnShip?.Invoke();
            }

            if (gameManager.ValueRO.CurrentTimer < gameManager.ValueRO.TimeToSpawn)
            {
                gameManager.ValueRW.CurrentTimer += SystemAPI.Time.DeltaTime;
            }
            else
            {
                state.EntityManager.Instantiate(gameManager.ValueRO.ShipPrefab);
                gameManager.ValueRW.CurrentTimer = 0f;
                gameManager.ValueRW.ShouldSpawn = false;
            }

            _shouldSpawnCached = gameManager.ValueRO.ShouldSpawn;
        }
    }
}
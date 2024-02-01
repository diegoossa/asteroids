using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    [UpdateBefore(typeof(AsteroidSpawnSystem))]
    public partial struct DifficultySystem : ISystem
    {
        private EntityQuery _asteroidQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Ship>();
            state.RequireForUpdate<AsteroidSpawner>();
            _asteroidQuery = state.GetEntityQuery(ComponentType.ReadOnly<Asteroid>());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var asteroidCount = _asteroidQuery.CalculateEntityCount();
            if (asteroidCount != 0)
                return;

            var asteroidSpawner = SystemAPI.GetSingletonRW<AsteroidSpawner>();
            asteroidSpawner.ValueRW.ShouldSpawn = true;
            asteroidSpawner.ValueRW.IncreaseDifficulty = true;
        }
    }
}
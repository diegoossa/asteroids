using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    public partial struct AsteroidSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<LevelBounds>();
            state.RequireForUpdate<AsteroidRandomSpawner>();
            state.RequireForUpdate<AsteroidSettings>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawner = SystemAPI.GetSingletonRW<AsteroidRandomSpawner>();
            if (!spawner.ValueRO.ShouldSpawn)
                return;

            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var levelBounds = SystemAPI.GetSingleton<LevelBounds>();

            var settings = SystemAPI.GetSingleton<AsteroidSettings>();
            var random = new Random(spawner.ValueRO.RandomSeed);

            var avoidPositions = new NativeList<float3>(Allocator.TempJob) {float3.zero};

            var jobHandle = new SpawnAsteroidsJob
            {
                CommandBuffer = commandBuffer,
                AsteroidPrefab = spawner.ValueRO.AsteroidPrefab,
                NumAsteroids = spawner.ValueRO.NumAsteroids,
                Bounds = levelBounds.Bounds,
                Rnd = random,
                AvoidPositions = avoidPositions,
                Settings = settings
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
            state.Dependency.Complete();
            avoidPositions.Dispose();

            // After spawning, set the spawner to not spawn
            spawner.ValueRW.ShouldSpawn = false;
        }
    }

    [BurstCompile]
    internal struct SpawnAsteroidsJob : IJob
    {
        public EntityCommandBuffer CommandBuffer;
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public float4 Bounds;
        public Random Rnd;
        public NativeList<float3> AvoidPositions;
        public AsteroidSettings Settings;

        public void Execute()
        {
            for (var i = 0; i < NumAsteroids; i++)
            {
                var asteroidEntity = CommandBuffer.Instantiate(AsteroidPrefab);

                if (TryGetValidPosition(ref Rnd, ref AvoidPositions, 5, Settings.Radius, out var randomPosition))
                {
                    CommandBuffer.SetComponent(asteroidEntity, LocalTransform.FromPositionRotation(randomPosition, Rnd.NextQuaternionRotation()));
                    CommandBuffer.SetComponent(asteroidEntity, new RotationSpeed {Value = Settings.RotationSpeed});
                    CommandBuffer.SetComponent(asteroidEntity, new Direction {Value = Rnd.NextFloat2Direction()});
                    CommandBuffer.SetComponent(asteroidEntity, new Speed {Value = Settings.Speed});
                }
            }
        }

        /// <summary>
        /// Try to get a valid position for the asteroid
        /// </summary>
        private bool TryGetValidPosition(ref Random random, ref NativeList<float3> avoidPositions,
            int maxAttempts, float radius, out float3 position)
        {
            position = float3.zero;
            var minDistance = radius + radius;

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                position = new float3(random.NextFloat(Bounds.x, Bounds.y), random.NextFloat(Bounds.z, Bounds.w), 0);
                var valid = true;
                foreach (var avoidPosition in avoidPositions)
                {
                    var distance = math.distance(position, avoidPosition);
                    if (distance < minDistance)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    // If we find a valid position, add it to the list of positions to avoid
                    avoidPositions.Add(position);
                    return true;
                }
            }

            return false;
        }
    }
}
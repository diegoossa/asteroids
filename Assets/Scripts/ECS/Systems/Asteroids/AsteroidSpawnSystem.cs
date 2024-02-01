using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    /// <summary>
    /// System that spawns asteroids at random positions
    /// </summary>
    public partial struct AsteroidSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<LevelBounds>();
            state.RequireForUpdate<AsteroidSpawner>();
            state.RequireForUpdate<DifficultyLevel>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawner = SystemAPI.GetSingletonRW<AsteroidSpawner>();
            if (!spawner.ValueRO.ShouldSpawn)
                return;

            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var levelBounds = SystemAPI.GetSingleton<LevelBounds>();
            var random = new Random(spawner.ValueRO.RandomSeed);
            var avoidPositions = new NativeList<float3>(Allocator.TempJob) {float3.zero};
            var initialStage = SystemAPI.GetSingletonBuffer<Stage>()[0];
            var difficultyLevel = SystemAPI.GetSingletonRW<DifficultyLevel>();

            // Increase the difficulty level from the spawner to avoid multiple increases
            if (spawner.ValueRO.IncreaseDifficulty)
            {
                difficultyLevel.ValueRW.CurrentLevel++;
                spawner.ValueRW.IncreaseDifficulty = false;
            }

            // Calculate the number of asteroids to spawn
            var numAsteroids = spawner.ValueRO.NumAsteroids + difficultyLevel.ValueRO.CurrentLevel * difficultyLevel.ValueRO.CountMultiplier;
            var speedMultiplier = difficultyLevel.ValueRO.CurrentLevel * difficultyLevel.ValueRO.SpeedMultiplier;

            var jobHandle = new SpawnAsteroidsJob
            {
                CommandBuffer = commandBuffer,
                AsteroidPrefab = spawner.ValueRO.AsteroidPrefab,
                NumAsteroids = numAsteroids,
                Bounds = levelBounds.Bounds,
                Rnd = random,
                AvoidPositions = avoidPositions,
                StageSettings = initialStage,
                SpeedMultiplier = speedMultiplier
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
            state.Dependency.Complete();
            avoidPositions.Dispose();

            // After spawning, set the spawner to not spawn
            spawner.ValueRW.ShouldSpawn = false;
        }
    }

    [BurstCompile]
    public struct SpawnAsteroidsJob : IJob
    {
        public EntityCommandBuffer CommandBuffer;
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public float4 Bounds;
        public Stage StageSettings;
        public Random Rnd;
        public NativeList<float3> AvoidPositions;
        public float SpeedMultiplier;

        public void Execute()
        {
            for (var i = 0; i < NumAsteroids; i++)
            {
                var asteroidEntity = CommandBuffer.Instantiate(AsteroidPrefab);

                if (TryGetValidPosition(StageSettings.Scale, out var randomPosition))
                {
                    CommandBuffer.SetComponent(asteroidEntity,
                        LocalTransform.FromPositionRotationScale(randomPosition, Rnd.NextQuaternionRotation(),
                            StageSettings.Scale));
                    CommandBuffer.SetComponent(asteroidEntity, new RotationSpeed {Value = StageSettings.RotationSpeed});
                    CommandBuffer.SetComponent(asteroidEntity, new Direction {Value = Rnd.NextFloat2Direction()});
                    CommandBuffer.SetComponent(asteroidEntity, new Speed {Value = StageSettings.Speed + SpeedMultiplier});
                    CommandBuffer.SetComponent(asteroidEntity, new Radius {Value = StageSettings.Scale / 2f});
                }
            }
        }

        /// <summary>
        /// Try to get a valid position for the asteroid
        /// </summary>
        private bool TryGetValidPosition(float radius, out float3 position, int maxAttempts = 5)
        {
            var minDistance = radius + radius;
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                position = new float3(Rnd.NextFloat(Bounds.x, Bounds.y), Rnd.NextFloat(Bounds.z, Bounds.w), 0);
                var valid = true;
                foreach (var avoidPosition in AvoidPositions)
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
                    AvoidPositions.Add(position);
                    return true;
                }
            }

            // If we can't find a valid position, just return a random position
            position = new float3(Rnd.NextFloat(Bounds.x, Bounds.y), Rnd.NextFloat(Bounds.z, Bounds.w), 0);
            return false;
        }
    }
}
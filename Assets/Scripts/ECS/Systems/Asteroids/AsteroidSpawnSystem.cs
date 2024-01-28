﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace DO.Asteroids
{
    public partial struct AsteroidSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<LevelBounds>();
            state.RequireForUpdate<AsteroidSpawner>();
            state.RequireForUpdate<AsteroidSettings>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var levelBounds = SystemAPI.GetSingleton<LevelBounds>();
            var spawner = SystemAPI.GetSingleton<AsteroidSpawner>();
            var settings = SystemAPI.GetSingleton<AsteroidSettings>();
            var random = new Random(spawner.RandomSeed);

            var avoidPositions = new NativeList<float3>(Allocator.TempJob) {float3.zero};

            var jobHandle = new SpawnAsteroidsJob
            {
                CommandBuffer = commandBuffer,
                AsteroidPrefab = spawner.AsteroidPrefab,
                NumAsteroids = spawner.NumAsteroids,
                Bounds = levelBounds.Bounds,
                Rnd = random,
                AvoidPositions = avoidPositions,
                Settings = settings
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
            state.Dependency.Complete();
            avoidPositions.Dispose();
            state.Enabled = false;
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
                    CommandBuffer.SetComponent(asteroidEntity, new RotationSpeed {Value = Rnd.NextFloat3(Settings.MinMaxRotationSpeed.x, Settings.MinMaxRotationSpeed.y)});
                    CommandBuffer.SetComponent(asteroidEntity, new Direction {Value = Rnd.NextFloat2Direction()});
                    CommandBuffer.SetComponent(asteroidEntity, new Speed {Value = Rnd.NextFloat(Settings.MinMaxSpeed.x, Settings.MinMaxSpeed.y)});
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
using Unity.Burst;
using Unity.Entities;
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
            state.RequireForUpdate<AsteroidsSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var levelBounds = SystemAPI.GetSingleton<LevelBounds>();
            var spawner = SystemAPI.GetSingleton<AsteroidsSpawner>();

            var job = new SpawnAsteroidsJob
            {
                AsteroidPrefab = spawner.AsteroidPrefab,
                CommandBuffer = commandBuffer,
                Bounds = levelBounds.Bounds,
                NumAsteroids = spawner.NumAsteroids
            };

            state.Dependency = job.Schedule(state.Dependency);
            state.Enabled = false;
        }
    }

    [BurstCompile]
    partial struct SpawnAsteroidsJob : IJobEntity
    {
        public Entity AsteroidPrefab;
        public EntityCommandBuffer CommandBuffer;
        public float4 Bounds;
        public int NumAsteroids;

        public void Execute([EntityIndexInQuery] int index, ref LocalTransform transform)
        {
            var random = new Random((uint) index + 1);
            for (var i = 0; i < NumAsteroids; i++)
            {
                var asteroidEntity = CommandBuffer.Instantiate(AsteroidPrefab);
                var randomPosition = new float3(random.NextFloat(Bounds.x, Bounds.y),
                    random.NextFloat(Bounds.z, Bounds.w), 0);
                CommandBuffer.SetComponent(asteroidEntity, new LocalTransform {Position = randomPosition, Rotation = random.NextQuaternionRotation(), Scale = 1f});
                CommandBuffer.AddComponent(asteroidEntity, new RotationSpeed {Value = random.NextFloat3(-5f, 5f)});
                CommandBuffer.AddComponent(asteroidEntity, new Asteroid());
            }
        }
    }
}
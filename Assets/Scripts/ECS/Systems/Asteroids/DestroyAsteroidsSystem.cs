using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    [UpdateBefore(typeof(AsteroidSpawnSystem))]
    public partial struct DestroyAsteroidsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<AsteroidSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var asteroidSpawner = SystemAPI.GetSingletonRW<AsteroidSpawner>();
            
            if (!asteroidSpawner.ValueRO.ResetAsteroids)
                return;
            
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            var destroyAsteroidsJob = new DestroyAsteroidsJob
            {
                CommandBuffer = commandBuffer
            };
            
            state.Dependency = destroyAsteroidsJob.Schedule(state.Dependency);
            state.Dependency.Complete();
            
            asteroidSpawner.ValueRW.ResetAsteroids = false;
            asteroidSpawner.ValueRW.ShouldSpawn = true;
        }
        
        [BurstCompile]
        [WithAll(typeof(Asteroid))]
        internal partial struct DestroyAsteroidsJob : IJobEntity
        {
            public EntityCommandBuffer CommandBuffer;

            private void Execute(Entity entity)
            {
                CommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
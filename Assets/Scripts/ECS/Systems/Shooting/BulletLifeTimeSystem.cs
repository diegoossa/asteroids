using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct BulletLifeTimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Bullet>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            var jobHandle = new BulletLifeTimeJob
            {
                CommandBuffer = commandBuffer.AsParallelWriter(),
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            
            state.Dependency = jobHandle.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        partial struct BulletLifeTimeJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            public float DeltaTime;

            private void Execute(Entity entity, [EntityIndexInQuery] int entityIndexInQuery, ref Bullet bullet)
            {
                bullet.CurrentLifeTime += DeltaTime;
                if (bullet.CurrentLifeTime > bullet.LifeTime)
                {
                    CommandBuffer.DestroyEntity(entityIndexInQuery, entity);
                }
            }
        }
    }
}
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    public partial struct EnemyDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<DamageEvent>();
            state.RequireForUpdate<Enemy>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var random = new Random((uint) SystemAPI.Time.ElapsedTime * 100 + 1);

            var jobHandle = new EnemyDamageJob
            {
                CommandBuffer = commandBuffer,
                Rnd = random
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(Enemy), typeof(DamageEvent))]
        partial struct EnemyDamageJob : IJobEntity
        {
            public EntityCommandBuffer CommandBuffer;
            public Random Rnd;

            private void Execute(Entity entity, ref Stage stage, ref Speed speed, ref Direction direction,
                ref PhysicsRadius physicsRadius, ref LocalTransform transform)
            {
                CommandBuffer.RemoveComponent<DamageEvent>(entity);

                // If the stage is in max stage, we destroy the enemy
                if (stage.Value >= stage.MaxStage)
                {
                    CommandBuffer.DestroyEntity(entity);
                    // TODO: Spawn explosion
                }
                else
                {
                    // Otherwise, we increment the stage and spawn next stage
                    stage.Value++;
                    speed.Value *= stage.SpeedMultiplier;
                    direction.Value = Rnd.NextFloat2Direction();
                    physicsRadius.Radius *= stage.ScaleMultiplier;
                    transform = LocalTransform.FromPositionRotationScale(transform.Position, transform.Rotation,
                        transform.Scale * stage.ScaleMultiplier);
                    
                    // Copy the enemy and change the direction
                    var enemyCopy = CommandBuffer.Instantiate(entity);
                    CommandBuffer.SetComponent(enemyCopy, new Direction {Value = Rnd.NextFloat2Direction()});
                }
            }
        }
    }
}
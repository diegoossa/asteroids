using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    [UpdateAfter(typeof(SpawnExplosionSystem))]
    public partial struct AsteroidDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<DamageEvent>();
            state.RequireForUpdate<Asteroid>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var random = new Random((uint) SystemAPI.Time.ElapsedTime * 100 + 1);
            var stageBuffer = SystemAPI.GetSingletonBuffer<Stage>();

            var jobHandle = new AsteroidDamageJob
            {
                CommandBuffer = commandBuffer,
                Rnd = random,
                StageBuffer = stageBuffer
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(DamageEvent))]
        partial struct AsteroidDamageJob : IJobEntity
        {
            public EntityCommandBuffer CommandBuffer;
            public Random Rnd;
            [ReadOnly] public DynamicBuffer<Stage> StageBuffer;

            private void Execute(Entity entity, ref Asteroid asteroid, ref Speed speed, ref Direction direction,
                ref PhysicsRadius physicsRadius, ref LocalTransform transform)
            {
                CommandBuffer.RemoveComponent<DamageEvent>(entity);

                var stage = StageBuffer[asteroid.CurrentStage];

                // Send Score of the current Stage
                var score = stage.Score;
                // Increase Stage
                asteroid.CurrentStage++;

                // Last Stage
                if (asteroid.CurrentStage >= StageBuffer.Length)
                {
                    CommandBuffer.DestroyEntity(entity);
                }
                else
                {
                    // Set the next stage
                    var nextStage = StageBuffer[asteroid.CurrentStage];
                    speed.Value = nextStage.Speed;
                    direction.Value = Rnd.NextFloat2Direction();
                    physicsRadius.Value = nextStage.Scale;
                    transform = LocalTransform.FromPositionRotationScale(transform.Position, transform.Rotation,
                        nextStage.Scale);

                    // Duplicate enemy and change the direction
                    var enemyCopy = CommandBuffer.Instantiate(entity);
                    CommandBuffer.SetComponent(enemyCopy, new Direction {Value = Rnd.NextFloat2Direction()});
                }
            }
        }
    }
}
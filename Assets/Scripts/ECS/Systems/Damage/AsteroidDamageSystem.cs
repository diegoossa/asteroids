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
            state.RequireForUpdate<Score>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var random = new Random((uint) SystemAPI.Time.ElapsedTime * 100 + 1);
            var stageBuffer = SystemAPI.GetSingletonBuffer<Stage>();
            var scoreEntity = SystemAPI.GetSingletonEntity<Score>();
            var score = SystemAPI.GetComponentRO<Score>(scoreEntity).ValueRO.Value;

            var jobHandle = new AsteroidDamageJob
            {
                CommandBuffer = commandBuffer,
                Rnd = random,
                StageBuffer = stageBuffer,
                ScoreEntity = scoreEntity,
                CurrentScore = score
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(DamageEvent))]
        partial struct AsteroidDamageJob : IJobEntity
        {
            [ReadOnly] public DynamicBuffer<Stage> StageBuffer;
            public EntityCommandBuffer CommandBuffer;
            public Random Rnd;
            public Entity ScoreEntity;
            public int CurrentScore;

            private void Execute(Entity entity, ref Asteroid asteroid, ref Speed speed, ref Direction direction,
                ref Radius radius, ref LocalTransform transform)
            {
                CommandBuffer.RemoveComponent<DamageEvent>(entity);
                var stage = StageBuffer[asteroid.CurrentStage];

                // Update score
                CurrentScore += stage.Score;
                CommandBuffer.SetComponent(ScoreEntity, new Score {Value = CurrentScore});
                
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
                    radius.Value = nextStage.Scale;
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
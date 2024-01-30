using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DO.Asteroids
{
    [BurstCompile]
    public partial struct ApplyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var jobHandle = new ApplyMovementJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };

            state.Dependency = jobHandle.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        partial struct ApplyMovementJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(ref LocalTransform transform, in Direction direction, in Speed speed)
            {
                transform.Position.xy += direction.Value * speed.Value * DeltaTime;
            }
        }
    }
}
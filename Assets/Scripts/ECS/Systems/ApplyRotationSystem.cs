using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    public partial struct ApplyRotationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RotationSpeed>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var job = new ApplyRotationJob
            {
                DeltaTime = deltaTime
            };
            state.Dependency = job.ScheduleParallel(state.Dependency);
        }
    }

    partial struct ApplyRotationJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(ref LocalTransform transform, in RotationSpeed rotationSpeed)
        {
            transform.Rotation = math.mul(transform.Rotation, quaternion.Euler(math.radians(rotationSpeed.Value) * DeltaTime));
        }
    }
}
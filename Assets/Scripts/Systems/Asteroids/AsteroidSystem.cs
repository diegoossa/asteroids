using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    public partial struct AsteroidSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Asteroid>();
            state.RequireForUpdate<RotationSpeed>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var job = new AsteroidMovementJob
            {
                DeltaTime = deltaTime
            };
            state.Dependency = job.ScheduleParallel(state.Dependency);
        }
    }

    partial struct AsteroidMovementJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(ref LocalTransform transform, in RotationSpeed rotationSpeed, in Velocity velocity)
        {
            transform.Rotation = math.mul(transform.Rotation, quaternion.Euler(math.radians(rotationSpeed.Value) * DeltaTime));
            transform.Position += new float3(velocity.Value, 0) * DeltaTime;
        }
    }
}
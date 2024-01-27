using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    [BurstCompile]
    public partial struct ShipMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Ship>();
            state.RequireForUpdate<PlayerInput>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var jobHandle = new ShipMovementJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            
            state.Dependency = jobHandle.Schedule(state.Dependency);
        }
    }

    [BurstCompile]
    [WithAll(typeof(Ship))]
    partial struct ShipMovementJob : IJobEntity
    {
        public float DeltaTime;

        private void Execute(ref LocalTransform transform, in PlayerInput input)
        {
            transform.Rotation = math.mul(transform.Rotation, quaternion.RotateZ(math.radians(input.Rotation * 60f) * DeltaTime));
        }
    }
}
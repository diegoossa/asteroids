using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    /// <summary>
    /// System that handles the movement of the ship.
    /// </summary>
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
            state.CompleteDependency();
        }
    }

    /// <summary>
    /// Job to handle ship movement 
    /// </summary>
    [BurstCompile]
    public partial struct ShipMovementJob : IJobEntity
    {
        public float DeltaTime;

        private void Execute(ref LocalTransform transform, ref Velocity velocity, in PlayerInput input, in Ship ship)
        {
            transform.Rotation = math.mul(transform.Rotation, quaternion.RotateZ(math.radians(-input.Rotation * ship.TurnTorque) * DeltaTime));

            if (input.Thrust)
            {
                var forward = new float3(0, ship.ThrustForce * DeltaTime, 0);
                velocity.Value += math.mul(transform.Rotation, forward).xy;
            }
            else
            {
                velocity.Value -= velocity.Value * ship.Drag * DeltaTime;
            }
            
            transform.Position.xy += velocity.Value * DeltaTime;
        }
    }
}
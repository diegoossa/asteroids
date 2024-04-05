using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    /// <summary>
    /// System that updates the scale of the ship exhaust based on the ship velocity.
    /// </summary>
    [BurstCompile]
    public partial struct ShipExhaustSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ShipExhaust>();
            state.RequireForUpdate<Ship>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var shipEntity = SystemAPI.GetSingletonEntity<Ship>();
            var exhaustEntity = SystemAPI.GetSingletonEntity<ShipExhaust>();
            var shipVelocity = SystemAPI.GetComponent<Velocity>(shipEntity);
            var exhaustTransform = SystemAPI.GetComponent<LocalTransform>(exhaustEntity);

            var variance = math.length(math.sin(shipVelocity.Value * shipVelocity.Value) * 0.25f);
            var targetScale = math.clamp(math.length(shipVelocity.Value), 0f, 1.25f) + variance;
            SystemAPI.SetComponent(exhaustEntity, LocalTransform.FromPositionRotationScale(exhaustTransform.Position, exhaustTransform.Rotation, targetScale));
        }
    }
}
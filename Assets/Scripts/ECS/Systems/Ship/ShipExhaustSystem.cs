using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
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
            
            var newScale = math.clamp(math.length(shipVelocity.Value), 0f, 1.6f);
            SystemAPI.SetComponent(exhaustEntity, LocalTransform.FromPositionRotationScale(exhaustTransform.Position, exhaustTransform.Rotation, newScale));
        }
    }
}
using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DO.Asteroids
{
    [UpdateAfter(typeof(CollisionSystem))]
    public partial struct SpawnExplosionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var localToWorld in SystemAPI.Query<LocalToWorld>().WithAll<DamageEvent>())
            {
                HybridSignalBus.Instance.OnSpawnExplosion?.Invoke(localToWorld.Position.xy);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}
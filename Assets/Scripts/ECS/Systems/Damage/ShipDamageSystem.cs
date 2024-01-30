using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DO.Asteroids
{
    [UpdateAfter(typeof(SpawnExplosionSystem))]
    public partial struct ShipDamageSystem : ISystem
    {
        private EntityQuery _damagedShipQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
             _damagedShipQuery = new EntityQueryBuilder(Allocator.Temp)
                 .WithAll<DamageEvent, Ship>()
                 .Build(state.EntityManager);

            state.RequireForUpdate(_damagedShipQuery);
            state.RequireForUpdate<Ship>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var shipEntity = SystemAPI.GetSingletonEntity<Ship>();
            state.EntityManager.DestroyEntity(shipEntity);

            var gameManager = SystemAPI.GetSingletonRW<GameManager>();
            gameManager.ValueRW.ShouldSpawn = true;
        }
    }
}
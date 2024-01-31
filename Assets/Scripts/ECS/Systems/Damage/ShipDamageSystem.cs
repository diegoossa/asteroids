using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

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
            state.RequireForUpdate<Lives>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var shipEntity = SystemAPI.GetSingletonEntity<Ship>();
            state.EntityManager.DestroyEntity(shipEntity);
            
            // Update lives
            var lives = SystemAPI.GetSingletonRW<Lives>();
            var newLives = math.max(0, lives.ValueRO.CurrentLives - 1);
            lives.ValueRW.CurrentLives = newLives;

            if (newLives <= 0) 
                return;
            
            var gameManager = SystemAPI.GetSingletonRW<GameManager>();
            gameManager.ValueRW.ShouldSpawn = true;
        }
    }
}
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    /// <summary>
    /// System that handles collision detection.
    /// </summary>
    [BurstCompile]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct CollisionSystem : ISystem
    {
        private EntityQuery _bulletQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Ship>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            
            _bulletQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Bullet>()
                .Build(ref state); 
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var bulletEntities = _bulletQuery.ToEntityArray(Allocator.TempJob);
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var shipEntity = SystemAPI.GetSingletonEntity<Ship>();
            
            var jobHandle = new CollisionJob
            {
                CommandBuffer = commandBuffer,
                Bullets = bulletEntities,
                PhysicsBoundsLookup = SystemAPI.GetComponentLookup<Radius>(),
                LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(),
                Ship = shipEntity
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
            state.Dependency.Complete();
            bulletEntities.Dispose();
        }

        [BurstCompile]
        [WithAll(typeof(Radius), typeof(LocalTransform), typeof(Enemy))]
        partial struct CollisionJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<Radius> PhysicsBoundsLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            [ReadOnly] public NativeArray<Entity> Bullets;
            [ReadOnly] public Entity Ship;
            public EntityCommandBuffer CommandBuffer;
                
            private void Execute(Entity entity)
            {
                var enemyBounds = PhysicsBoundsLookup[entity].Value;
                var enemyPosition = LocalTransformLookup[entity].Position.xy;
                
                // Check if bullet is colliding with enemy
                foreach (var bullet in Bullets)
                {
                    var bulletBounds = PhysicsBoundsLookup[bullet].Value;
                    var bulletPosition = LocalTransformLookup[bullet].Position.xy;
                    
                    if (Intersect(enemyBounds, bulletBounds, enemyPosition, bulletPosition))
                    {
                        CommandBuffer.DestroyEntity(bullet);
                        CommandBuffer.AddComponent<DamageEvent>(entity);
                    }
                }
                
                // Check if ship is colliding with enemy
                var shipBounds = PhysicsBoundsLookup[Ship].Value;
                var shipPosition = LocalTransformLookup[Ship].Position.xy;
                if (Intersect(enemyBounds, shipBounds, enemyPosition, shipPosition))
                {
                    CommandBuffer.AddComponent<DamageEvent>(Ship);
                    CommandBuffer.AddComponent<DamageEvent>(entity);
                }
            }
            
            /// <summary>
            /// Check if two circles intersect
            /// </summary>
            private static bool Intersect(float radius, float otherRadius, float2 position, float2 otherPosition)
            {
                var diff = position - otherPosition;
                var distSq = math.dot(diff, diff);
                return distSq <= (radius + otherRadius) * (radius + otherRadius);
            }
        }
    }
}
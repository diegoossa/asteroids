using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    [BurstCompile]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct CollisionSystem : ISystem
    {
        private EntityQuery _bulletQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
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
            
            var jobHandle = new CollisionJob
            {
                CommandBuffer = commandBuffer,
                Bullets = bulletEntities,
                PhysicsBoundsLookup = SystemAPI.GetComponentLookup<PhysicsRadius>(),
                LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>()
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
            state.Dependency.Complete();
            bulletEntities.Dispose();
        }

        [BurstCompile]
        [WithAll(typeof(PhysicsRadius), typeof(LocalTransform), typeof(Enemy))]
        partial struct CollisionJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<PhysicsRadius> PhysicsBoundsLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            [ReadOnly] public NativeArray<Entity> Bullets;
            public EntityCommandBuffer CommandBuffer;
                
            private void Execute(Entity entity)
            {
                var enemyBounds = PhysicsBoundsLookup[entity].Radius;
                var enemyPosition = LocalTransformLookup[entity].Position.xy;
                
                foreach (var bullet in Bullets)
                {
                    var bulletBounds = PhysicsBoundsLookup[bullet].Radius;
                    var bulletPosition = LocalTransformLookup[bullet].Position.xy;
                    
                    if (Intersect(enemyBounds, bulletBounds, enemyPosition, bulletPosition))
                    {
                        CommandBuffer.DestroyEntity(bullet);
                        CommandBuffer.AddComponent<DamageEvent>(entity);
                    }
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
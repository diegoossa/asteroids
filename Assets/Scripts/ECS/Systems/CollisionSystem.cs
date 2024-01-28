using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    // [BurstCompile]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct CollisionSystem : ISystem
    {
        private EntityQuery _bulletQuery;
        
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
                PhysicsBoundsLookup = SystemAPI.GetComponentLookup<PhysicsBounds>(),
                LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>()
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
            state.Dependency.Complete();
            bulletEntities.Dispose();
        }

        [BurstCompile]
        [WithAll(typeof(PhysicsBounds), typeof(LocalTransform), typeof(Asteroid))]
        partial struct CollisionJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<PhysicsBounds> PhysicsBoundsLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            [ReadOnly] public NativeArray<Entity> Bullets;
            public EntityCommandBuffer CommandBuffer;
                
            private void Execute(Entity entity)
            {
                var asteroidBounds = PhysicsBoundsLookup[entity].Radius;
                var asteroidPosition = LocalTransformLookup[entity].Position.xy;
                
                foreach (var bullet in Bullets)
                {
                    var bulletBounds = PhysicsBoundsLookup[bullet].Radius;
                    var bulletPosition = LocalTransformLookup[bullet].Position.xy;
                    
                    if (Intersect(asteroidBounds, bulletBounds, asteroidPosition, bulletPosition))
                    {
                        CommandBuffer.DestroyEntity(bullet);
                        CommandBuffer.DestroyEntity(entity);
                    }
                }
            }
            
            private static bool Intersect(float radius, float otherRadius, float2 position, float2 otherPosition)
            {
                var diff = position - otherPosition;
                var distSq = math.dot(diff, diff);
                return distSq <= (radius + otherRadius) * (radius + otherRadius);
            }
        }
    }
}
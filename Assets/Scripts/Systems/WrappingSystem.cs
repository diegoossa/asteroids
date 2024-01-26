using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    [BurstCompile]
    public partial struct WrappingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<LevelBounds>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var levelBounds = SystemAPI.GetSingleton<LevelBounds>();
            
            var jobHandle = new WrapJob
            {
                LevelBounds = levelBounds.Bounds
            };

            state.Dependency = jobHandle.ScheduleParallel(state.Dependency);
        }
    }

    /// <summary>
    /// Job to wrap entities around the level bounds.
    /// </summary>
    [BurstCompile]
    [WithAll(typeof(Wrap))]
    public partial struct WrapJob : IJobEntity
    {
        public float4 LevelBounds;

        private void Execute(ref LocalTransform transform, in PhysicsBounds physicsBounds)
        {
            var position = transform.Position;

            if (position.x < LevelBounds.x - physicsBounds.Radius)
            {
                position.x = LevelBounds.y + physicsBounds.Radius;
            }
            else if (position.x > LevelBounds.y + physicsBounds.Radius)
            {
                position.x = LevelBounds.x - physicsBounds.Radius;
            }
            else if (position.y < LevelBounds.z - physicsBounds.Radius)
            {
                position.y = LevelBounds.w + physicsBounds.Radius;
            } 
            else if (position.y > LevelBounds.w + physicsBounds.Radius)
            {
                position.y = LevelBounds.z - physicsBounds.Radius;
            }
            
            transform.Position = position;
        }
    }
}
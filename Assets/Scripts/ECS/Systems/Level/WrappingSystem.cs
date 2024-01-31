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

        private void Execute(ref LocalTransform transform, in PhysicsRadius physicsRadius)
        {
            var position = transform.Position;

            if (position.x < LevelBounds.x - physicsRadius.Value)
            {
                position.x = LevelBounds.y + physicsRadius.Value;
            }
            else if (position.x > LevelBounds.y + physicsRadius.Value)
            {
                position.x = LevelBounds.x - physicsRadius.Value;
            }
            else if (position.y < LevelBounds.z - physicsRadius.Value)
            {
                position.y = LevelBounds.w + physicsRadius.Value;
            } 
            else if (position.y > LevelBounds.w + physicsRadius.Value)
            {
                position.y = LevelBounds.z - physicsRadius.Value;
            }
            
            transform.Position = position;
        }
    }
}
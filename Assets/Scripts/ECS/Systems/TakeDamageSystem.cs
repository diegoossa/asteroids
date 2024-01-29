using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct TakeDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TakeDamage>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}
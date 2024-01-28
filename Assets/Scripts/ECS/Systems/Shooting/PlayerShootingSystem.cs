using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct PlayerShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Weapon>();
            state.RequireForUpdate<PlayerInput>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerInput = SystemAPI.GetSingleton<PlayerInput>();
            var weapon = SystemAPI.GetSingletonRW<Weapon>();
            weapon.ValueRW.IsFiring = playerInput.Shoot;
        }
    }
}
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids
{
    /// <summary>
    /// System that spawns bullets when the weapon is firing.
    /// </summary>
    public partial struct SpawnBulletSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Weapon>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            var random = new Random((uint) SystemAPI.Time.ElapsedTime * 100 + 1);

            var jobHandle = new SpawnBulletJob
            {
                CommandBuffer = commandBuffer,
                DeltaTime = SystemAPI.Time.DeltaTime,
                Rnd = random
            };

            state.Dependency = jobHandle.Schedule(state.Dependency);
        }

        [BurstCompile]
        partial struct SpawnBulletJob : IJobEntity
        {
            public EntityCommandBuffer CommandBuffer;
            public float DeltaTime;
            public Random Rnd;

            private void Execute(ref Weapon weapon, in LocalToWorld localToWorld)
            {
                if (weapon.IsFiring && weapon.CooldownTimer <= 0f)
                {
                    var bulletEntity = CommandBuffer.Instantiate(weapon.BulletPrefab);
                    var rotation = localToWorld.Rotation;
                    rotation = math.mul(rotation, quaternion.RotateZ(Rnd.NextFloat(-weapon.Spread, weapon.Spread)));
                    CommandBuffer.SetComponent(bulletEntity,
                        LocalTransform.FromPositionRotation(localToWorld.Position + localToWorld.Forward * 0.5f,
                            rotation));
                    CommandBuffer.SetComponent(bulletEntity, new Direction {Value = math.mul(rotation, math.up()).xy});
                    weapon.CooldownTimer = weapon.Cooldown;
                }
                else
                {
                    weapon.CooldownTimer -= DeltaTime;
                }
            }
        }
    }
}
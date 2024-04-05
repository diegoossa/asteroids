using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// Weapon component for the ship.
    /// </summary>
    public struct Weapon : IComponentData
    {
        public Entity BulletPrefab;
        public float Cooldown;
        public float CooldownTimer;
        public float Spread;
        public bool IsFiring;
    }
}
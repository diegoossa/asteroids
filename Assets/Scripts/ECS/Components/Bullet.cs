using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// Bullet component.
    /// </summary>
    public struct Bullet : IComponentData
    {
        public float CurrentLifeTime;
        public float LifeTime;
    }
}
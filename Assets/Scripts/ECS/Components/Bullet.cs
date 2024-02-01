using Unity.Entities;

namespace DO.Asteroids
{
    public struct Bullet : IComponentData
    {
        public float CurrentLifeTime;
        public float LifeTime;
    }
}
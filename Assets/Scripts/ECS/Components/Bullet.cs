using Unity.Entities;

namespace DO.Asteroids
{
    public struct BulletFactory : IComponentData
    {
        public Entity Prefab;
    }
    
    public struct Bullet : IComponentData
    {
        public float CurrentLifeTime;
        public float LifeTime;
    }
}
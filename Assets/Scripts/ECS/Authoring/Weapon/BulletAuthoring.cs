using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class BulletAuthoring : MonoBehaviour
    {
        public float Speed = 10f;
        public float Radius = 0.1f;
        public float LifeTime = 2f;

        public class BulletBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bullet {LifeTime = authoring.LifeTime});
                AddComponent(entity, new Speed {Value = authoring.Speed});
                AddComponent(entity, new Direction());
                AddComponent(entity, new PhysicsRadius {Value = authoring.Radius});
                AddComponent(entity, new Wrap());
            }
        }
    }
}
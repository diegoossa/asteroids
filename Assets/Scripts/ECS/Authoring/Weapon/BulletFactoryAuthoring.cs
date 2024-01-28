using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class BulletFactoryAuthoring : MonoBehaviour
    {
        public GameObject Prefab;

        public class BulletFactoryBaker : Baker<BulletFactoryAuthoring>
        {
            public override void Bake(BulletFactoryAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BulletFactory { Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic) });
            }
        }
    }
}
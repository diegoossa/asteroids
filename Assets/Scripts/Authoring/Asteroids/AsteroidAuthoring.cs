using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidAuthoring : MonoBehaviour
    {
        public float Radius = 0.5f;
        
        public class AsteroidBaker : Baker<AsteroidAuthoring>
        {
            public override void Bake(AsteroidAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Asteroid());
                AddComponent(entity, new PhysicsBounds {Radius = authoring.Radius});
                AddComponent(entity, new Velocity());
                AddComponent(entity, new Wrap());
            }
        }
    }
}
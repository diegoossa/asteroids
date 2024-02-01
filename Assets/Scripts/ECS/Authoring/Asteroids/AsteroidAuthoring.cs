using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    /// <summary>
    /// The AsteroidAuthoring class is used to bake an asteroid entity.
    /// </summary>
    public class AsteroidAuthoring : MonoBehaviour
    {
        public class AsteroidBaker : Baker<AsteroidAuthoring>
        {
            public override void Bake(AsteroidAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Asteroid());
                AddComponent(entity, new Speed());
                AddComponent(entity, new Direction());
                AddComponent(entity, new RotationSpeed());
                AddComponent(entity, new Radius());
                AddComponent(entity, new Enemy());
                AddComponent(entity, new Wrap());
            }
        }
    }
}
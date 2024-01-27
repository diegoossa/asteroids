using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class ShipAuthoring : MonoBehaviour
    {
        public float Radius = 0.5f;
        
        public class ShipBaker : Baker<ShipAuthoring>
        {
            public override void Bake(ShipAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Ship());
                AddComponent(entity, new PlayerInput());
                AddComponent(entity, new PhysicsBounds {Radius = authoring.Radius});
                AddComponent(entity, new Wrap());
            }
        }
    }
}
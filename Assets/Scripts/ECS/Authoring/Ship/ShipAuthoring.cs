using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    /// <summary>
    /// Authoring component for the ship.
    /// </summary>
    public class ShipAuthoring : MonoBehaviour
    {
        public float ThrustForce = 3f;
        public float TurnTorque = 45f;
        public float Drag = 1f;
        public float Radius = 0.5f;

        public class ShipBaker : Baker<ShipAuthoring>
        {
            public override void Bake(ShipAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Ship
                {
                    ThrustForce = authoring.ThrustForce, 
                    TurnTorque = authoring.TurnTorque, 
                    Drag = authoring.Drag
                });
                AddComponent(entity, new PlayerInput());
                AddComponent(entity, new Radius {Value = authoring.Radius});
                AddComponent(entity, new Wrap());
                AddComponent(entity, new Velocity());
            }
        }
    }
}
using Unity.Entities;

namespace DO.Asteroids
{
    public struct Ship : IComponentData
    {
        public float ThrustForce;
        public float TurnTorque;
        public float Drag;
        public Entity Exhaust;
    }
    
    public struct ShipExhaust : IComponentData { }
}
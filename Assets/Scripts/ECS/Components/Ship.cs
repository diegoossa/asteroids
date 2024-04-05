using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// Ship component.
    /// </summary>
    public struct Ship : IComponentData
    {
        public float ThrustForce;
        public float TurnTorque;
        public float Drag;
    }
    
    public struct ShipExhaust : IComponentData { }
}
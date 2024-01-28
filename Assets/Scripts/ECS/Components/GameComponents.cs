using Unity.Entities;
using Unity.Mathematics;

namespace DO.Asteroids
{
    public struct Direction : IComponentData
    {
        public float2 Value;
    }
    
    public struct Speed : IComponentData
    {
        public float Value;
    }

    public struct Velocity : IComponentData
    {
        public float2 Value;
    }

    public struct RotationSpeed : IComponentData
    {
        public float3 Value;
    }

    /// <summary>
    /// For simplicity, we are not using the Unity Physics package, but we can have a super simplified physics circle shape
    /// </summary>
    public struct PhysicsBounds : IComponentData
    {
        public float Radius;
    }

    public struct Wrap : IComponentData { }
    
    public struct Enemy : IComponentData { }
}
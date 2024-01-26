using Unity.Entities;
using Unity.Mathematics;

namespace DO.Asteroids
{
    public struct AsteroidsSpawner : IComponentData
    {
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public float Radius;
    }
    
    public struct AsteroidSettings : IComponentData
    {
        public float2 MinMaxSpeed;
        public float2 MinMaxRotationSpeed;
    }

    public struct Asteroid : IComponentData
    {
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
        public float3 Position;
    }
}

using Unity.Entities;
using Unity.Mathematics;

namespace DO.Asteroids
{
    public struct AsteroidsSpawner : IComponentData
    {
        public Entity AsteroidPrefab;
        public int NumAsteroids;
    }

    public struct Asteroid : IComponentData
    {
        public float Radius;
    }

    public struct Velocity : IComponentData
    {
        public float2 Value;
    }
    
    public struct RotationSpeed : IComponentData
    {
        public float3 Value;
    }
}

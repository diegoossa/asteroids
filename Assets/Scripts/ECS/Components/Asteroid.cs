using Unity.Entities;
using Unity.Mathematics;

namespace DO.Asteroids
{
    public struct AsteroidSpawner : IComponentData
    {
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public uint RandomSeed;
    }
    
    public struct AsteroidSettings : IComponentData
    {
        public float2 MinMaxSpeed;
        public float2 MinMaxRotationSpeed;
        public float Radius;
    }

    public struct Asteroid : IComponentData
    {
    }
}

using Unity.Entities;

namespace DO.Asteroids
{
    public struct AsteroidRandomSpawner : IComponentData
    {
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public uint RandomSeed;
        public bool ShouldSpawn;
    }
    
    public struct AsteroidSettings : IComponentData
    {
        public float Speed;
        public float RotationSpeed;
        public float Radius;
    }

    public struct Asteroid : IComponentData
    {
    }
    
    public struct Stage : IComponentData
    {
        public int Value;
        public int MaxStage;
        public float SpeedMultiplier;
        public float ScaleMultiplier;
    }
}

using Unity.Entities;

namespace DO.Asteroids
{
    public struct AsteroidRandomSpawner : IComponentData
    {
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public uint RandomSeed;
        public bool ShouldSpawn;
        public bool ResetAsteroids;
    }

    public struct Asteroid : IComponentData
    {
        public int CurrentStage;
    }
    
    [InternalBufferCapacity(3)]
    public struct Stage : IBufferElementData
    {
        public float Speed;
        public float Scale;
        public float RotationSpeed;
        public int Score;
    }
}

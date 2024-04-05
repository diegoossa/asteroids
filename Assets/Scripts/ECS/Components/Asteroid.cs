using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// The AsteroidSpawner component is used to spawn asteroids.
    /// </summary>
    public struct AsteroidSpawner : IComponentData
    {
        public Entity AsteroidPrefab;
        public int NumAsteroids;
        public uint RandomSeed;
        public bool ShouldSpawn;
        public bool ResetAsteroids;
        public bool IncreaseDifficulty;
    }

    /// <summary>
    /// The Asteroid component is used to identify an asteroid entity.
    /// </summary>
    public struct Asteroid : IComponentData
    {
        public int CurrentStage;
    }
    
    /// <summary>
    /// The Stage component is used to store asteroid settings for each stage.
    /// </summary>
    [InternalBufferCapacity(3)]
    public struct Stage : IBufferElementData
    {
        public float Speed;
        public float Scale;
        public float RotationSpeed;
        public int Score;
    }
}

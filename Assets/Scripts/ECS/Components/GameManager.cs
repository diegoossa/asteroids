using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// Game manager component that holds the game state.
    /// </summary>
    public struct GameManager : IComponentData
    {
        public Entity ShipPrefab;
        public float TimeToSpawn;
        public float CurrentTimer;
        public bool ShouldSpawn;
    }

    /// <summary>
    /// Lives component.
    /// </summary>
    public struct Lives : IComponentData
    {
        public int InitialLives;
        public int LastLives;
        public int CurrentLives;
    }

    /// <summary>
    /// Score component.
    /// </summary>
    public struct Score : IComponentData
    {
        public int Value;
        public int LastScore;
    }

    /// <summary>
    /// Difficulty level component.
    /// </summary>
    public struct DifficultyLevel : IComponentData
    {
        public int CurrentLevel;
        public int CountMultiplier;
        public float SpeedMultiplier;
    }
}
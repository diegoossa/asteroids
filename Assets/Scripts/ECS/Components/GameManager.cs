using Unity.Entities;

namespace DO.Asteroids
{
    public struct GameManager : IComponentData
    {
        public Entity ShipPrefab;
        public float TimeToSpawn;
        public float CurrentTimer;
        public bool ShouldSpawn;
        public GameState GameState;
    }

    public struct Lives : IComponentData
    {
        public int InitialLives;
        public int LastLives;
        public int CurrentLives;
    }
    
    public struct Score : IComponentData
    {
        public int Value;
        public int LastScore;
    }
    
    public enum GameState
    {
        None,
        Menu,
        Play,
        Pause,
        GameOver
    }
}
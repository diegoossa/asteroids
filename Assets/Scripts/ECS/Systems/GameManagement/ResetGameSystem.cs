using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct ResetGameSystem : ISystem, ISystemStartStop
    {
        public void OnStartRunning(ref SystemState state)
        {
            HybridSignalBus.Instance.OnGameStateChange -= OnResetGame;
        }

        [BurstCompile]
        private void OnResetGame(GameState gameState)
        {
            if (gameState == GameState.Menu)
            {
                // Reset game state
                var lives = SystemAPI.GetSingletonRW<Lives>();
                lives.ValueRW.CurrentLives = lives.ValueRO.InitialLives;

                var score = SystemAPI.GetSingletonRW<Score>();
                score.ValueRW.Value = 0;
                
                var asteroidSpawner = SystemAPI.GetSingletonRW<AsteroidSpawner>();
                asteroidSpawner.ValueRW.ResetAsteroids = true;
            }
        }
        
        public void OnStopRunning(ref SystemState state)
        {
            HybridSignalBus.Instance.OnGameStateChange -= OnResetGame;
        }
    }
}
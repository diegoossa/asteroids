using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct StartGameSystem : ISystem, ISystemStartStop
    {
        public void OnStartRunning(ref SystemState state)
        {
            HybridSignalBus.Instance.OnGameStateChange += OnStartGame;
        }

        [BurstCompile]
        private void OnStartGame(GameState gameState)
        {
            if (gameState == GameState.Play)
            {
                var gameManager = SystemAPI.GetSingletonRW<GameManager>();
                gameManager.ValueRW.ShouldSpawn = true;
            }
        }

        public void OnStopRunning(ref SystemState state)
        {
            HybridSignalBus.Instance.OnGameStateChange -= OnStartGame;
        }
    }
}
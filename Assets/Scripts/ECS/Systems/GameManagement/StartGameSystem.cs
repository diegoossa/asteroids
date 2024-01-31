using DO.Asteroids.Hybrid;
using Unity.Burst;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct StartGameSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameManager>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            if(HybridSignalBus.Instance != null)
                HybridSignalBus.Instance.OnGameStateChange += OnStartGame;
        }

        [BurstCompile]
        private void OnStartGame(GameState gameState)
        {
            if (gameState == GameState.Play)
            {
                var gameManager = SystemAPI.GetSingletonRW<GameManager>();
                gameManager.ValueRW.ShouldSpawn = true;
                gameManager.ValueRW.GameState = GameState.Play;
            }
        }

        public void OnStopRunning(ref SystemState state) { }
    }
}
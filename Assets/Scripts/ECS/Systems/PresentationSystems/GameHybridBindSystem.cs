// using DO.Asteroids.Hybrid;
// using Unity.Entities;
// using UnityEngine;
// using Zenject;
//
// namespace DO.Asteroids
// {
//     [DisableAutoCreation]
//     public partial class GameHybridBindSystem : SystemBase
//     {
//         SignalBus _signalBus;
//         
//         [Inject]
//         public void Construct(SignalBus signalBus)
//         {
//             _signalBus = signalBus;
//             //signalBus.Subscribe<StartGameSignal>(OnGameStarted);
//         }
//         
//         protected override void OnCreate()
//         {
//             _signalBus.Subscribe<StartGameSignal>(OnGameStarted);
//         }
//
//         private void OnGameStarted()
//         {
//             Debug.Log("GAME START FROM SYSTEM");
//         }
//
//         protected override void OnUpdate()
//         {
//             Debug.Log("UPDATE FROM SYSTEM");
//         }
//     }
//     
//     
// }
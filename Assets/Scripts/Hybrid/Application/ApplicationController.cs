using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DO.Asteroids.Hybrid
{
    public class ApplicationController : LifetimeScope
    {
        [SerializeField]
        private HybridMessageBus _hybridMessageBus;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponent(_hybridMessageBus);
                        
            RegisterMessagePipe(builder);
            RegisterSystems(builder);
        }
        
        private void RegisterMessagePipe(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            
            builder.RegisterMessageBroker<GameState, int>(options);
            builder.RegisterMessageBroker<StartGameMessage>(options);
        }
        
        private void RegisterSystems(IContainerBuilder builder)
        {
            // builder.RegisterSystemFromDefaultWorld<GameHybridBindSystem>();
            // builder.UseDefaultWorld(systems =>
            // {
            //     systems.Add<GameHybridBindSystem>();
            // });
        }
    }

    
}
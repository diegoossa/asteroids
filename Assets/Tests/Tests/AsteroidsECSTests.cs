using NUnit.Framework;
using Unity.Entities;
using Unity.Entities.Tests;
using Unity.Mathematics;
using Unity.Transforms;

namespace DO.Asteroids.Tests
{
    public class CustomEcsTestsFixture : ECSTestsFixture
    {
        private const int EntityCount = 1000;
        
        protected void UpdateSystem<T>() where T : unmanaged, ISystem
        {
            World.GetExistingSystem<T>().Update(World.Unmanaged);
        }

        protected SystemHandle CreateSystem<T>() where T : unmanaged, ISystem => World.CreateSystem<T>();

        protected Entity CreateEntity(params ComponentType[] types) => m_Manager.CreateEntity(types);

        protected void CreateEntities(ComponentType[] types, int entityCount = EntityCount)
        {
            for (var i = 0; i < entityCount; i++)
            {
                m_Manager.CreateEntity(types);
            }
        }

        protected void CreateEntityCommandBufferSystem()
        {
            World.CreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
    }

    [TestFixture]
    [Category("ECS Tests")]
    public class AsteroidsECSTests : CustomEcsTestsFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            CreateSystem<ApplyMovementSystem>();
        }
        
        [Test]
        public void Entity_Moves_When_ApplyMovementSystem_Is_Executed()
        {
            var entity = CreateEntity(typeof(LocalTransform), typeof(Direction), typeof(Speed));
            m_Manager.SetComponentData(entity, new LocalTransform {Position = new float3(0, 0, 0)});
            m_Manager.SetComponentData(entity, new Direction {Value = new float2(1f, 0)});
            m_Manager.SetComponentData(entity, new Speed {Value = 1f});
            
            UpdateSystem<ApplyMovementSystem>();
        
            var newPosition = m_Manager.GetComponentData<LocalTransform>(entity).Position;
            Assert.AreNotEqual(float3.zero, newPosition);
        }
    }
}
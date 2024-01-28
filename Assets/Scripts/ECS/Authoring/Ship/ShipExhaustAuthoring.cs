using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class ShipExhaustAuthoring : MonoBehaviour
    {
        public class ShipExhaustBaker : Baker<ShipExhaustAuthoring>
        {
            public override void Bake(ShipExhaustAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ShipExhaust());
            }
        }
    }
}
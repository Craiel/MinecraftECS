using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    /// <summary>
    /// This system creates a box collider for each block so the player can collide
    /// with them. This is a temporary workaround until ECS supports native physics.
    /// </summary>
    public sealed class BlockColliderSystem : JobComponentSystem
    {
        private readonly IDictionary<int, GameObject> colliders;

        public BlockColliderSystem()
        {
            this.colliders = new Dictionary<int, GameObject>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            this.CreateColliders();
            this.DestroyColliders();

            return inputDeps;
        }

        private void CreateColliders()
        {
            var queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {typeof(BlockCreateColliderTagData)},
                None = new ComponentType[] {typeof(BlockColliderData)}
            };

            var query = GetEntityQuery(queryDesc);
            NativeArray<Entity> data = query.ToEntityArray(Allocator.TempJob);

            if (data.Length == 0)
            {
                data.Dispose();
                return;
            }

            foreach (Entity addedBlock in data)
            {
                Translation translation = EntityManager.GetComponentData<Translation>(addedBlock);
                var obj = new GameObject("Block Collider") { layer = 9 };
                obj.transform.position = translation.Value;
                obj.AddComponent<BoxCollider>();
                int instanceId = obj.GetInstanceID();
                this.colliders.Add(instanceId, obj);

                EntityManager.AddComponent<BlockColliderData>(addedBlock);
                BlockColliderData colliderData = EntityManager.GetComponentData<BlockColliderData>(addedBlock);
                colliderData.ColliderId = obj.GetInstanceID();
                EntityManager.SetComponentData(addedBlock, colliderData);

                EntityManager.RemoveComponent<BlockCreateColliderTagData>(addedBlock);
            }

            data.Dispose();
        }

        private void DestroyColliders()
        {
            var queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] {typeof(BlockColliderData)},
                None = new ComponentType[] {typeof(BlockData)}
            };

            var query = GetEntityQuery(queryDesc);
            NativeArray<Entity> data = query.ToEntityArray(Allocator.TempJob);

            if (data.Length == 0)
            {
                data.Dispose();
                return;
            }

            foreach (Entity removedBlock in data)
            {
                var colliderData = World.Active.EntityManager.GetComponentData<BlockColliderData>(removedBlock);
                if (this.colliders.TryGetValue(colliderData.ColliderId, out GameObject collider))
                {
                    this.colliders.Remove(colliderData.ColliderId);
                    Object.Destroy(collider);
                }

                World.Active.EntityManager.RemoveComponent<BlockColliderData>(removedBlock);
            }

            data.Dispose();
        }
    }
}

using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    /// <summary>
    /// This system creates a box collider for each block so the player can collide
    /// with them. This is a temporary workaround until ECS supports native physics.
    /// </summary>
    class BlockColliderSystem : ComponentSystem
    {
        // System state components are persistent and not destroyed along with
        // entities so we can use them to track when a block is added or removed.
        struct BlockColliderData : ISystemStateComponentData
        {
            public int colliderIDs;
        }

        struct AddedBlockGroup
        {
            public EntityArray entities;
            public ComponentDataArray<BlockTag> tag;
            public ComponentDataArray<Position> positions;
            public SubtractiveComponent<BlockColliderData> missing;
        }
        [Inject]
        AddedBlockGroup addedBlocks;

        struct RemovedBlockGroup
        {
            public EntityArray entities;
            public ComponentDataArray<BlockColliderData> colliderData;
            public SubtractiveComponent<BlockTag> missing;
        }
        [Inject]
        RemovedBlockGroup removedBlocks;

        Dictionary<int, GameObject> colliders;

        protected override void OnStartRunning()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            colliders = new Dictionary<int, GameObject>();
        }

        protected override void OnUpdate()
        {
            // Process newly added blocks.
            for (int i = 0; i != addedBlocks.entities.Length; i++)
            {
                // Create new collider object for this block.
                var obj = new GameObject("Block Collider");
                obj.layer = 9;
                obj.transform.position = addedBlocks.positions[i].Value;
                obj.AddComponent<BoxCollider>();

                // Add collider data to block entity so we can remove it later.
                int id = obj.GetInstanceID();
                var blockData = new BlockColliderData();
                blockData.colliderIDs = id;
                colliders.Add(id, obj);

                PostUpdateCommands.AddComponent(addedBlocks.entities[i], blockData);
            }

            // Process removed blocks.
            for (int i = 0; i != removedBlocks.entities.Length; i++)
            {
                int id = removedBlocks.colliderData[i].colliderIDs;

                // Destroy collider object associated with this block.
                GameObject obj;

                if (colliders.TryGetValue(id, out obj))
                {
                    colliders.Remove(id);
                    GameObject.Destroy(obj);
                }

                PostUpdateCommands.RemoveComponent<BlockColliderData>(removedBlocks.entities[i]);
            }
        }
    }
}

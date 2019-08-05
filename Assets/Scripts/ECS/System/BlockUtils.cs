using System;
using DefaultNamespace;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.Rendering;

namespace Minecraft
{
    public static class BlockUtils
    {
        public static Unity.Mathematics.Random BlockRandom = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);

        public static BlockType GetBaseBlockTypeFromDepth(int depth, int groundHeight)
        {
            if (depth >= groundHeight)
            {
                return BlockType.Air;
            }

            if (depth == groundHeight - 1)
            {
                return BlockType.DirtGrass;
            }

            if (depth < 4)
            {
                return BlockType.Bedrock;
            }

            int rnd = BlockRandom.NextInt(0, 8);
            if (rnd == 5)
            {
                return BlockType.Air;
            }

            if (depth < Constants.SeaLevelHeight - 6)
            {
                return BlockType.Stone;
            }

            if (depth < groundHeight - 1)
            {
                return BlockType.Dirt;
            }

            return BlockType.Air;
        }

        public static Entity Create(EntityCommandBuffer.Concurrent commandBuffer, int index, BlockCreateData data)
        {
            Entity prefab = GameSettingsData.Instance.GetPrefab(data.BlockType);
            if (prefab != Entity.Null)
            {
                var translation = new Translation {Value = new float3(data.X, data.Y, data.Z)};
                Entity instance = commandBuffer.Instantiate(index, prefab);
                //Entity instance = commandBuffer.CreateEntity(index, GameSettingsData.Instance.BlockArchetype);
                WorldData.Set(data.X, data.Y, data.Z, instance);

                commandBuffer.AddComponent<BlockData>(index, instance);

                var blockData = new BlockData
                {
                    Type = data.BlockType
                };

                commandBuffer.SetComponent(index, instance, blockData);
                commandBuffer.SetComponent(index, instance, translation);

                switch (data.CreateType)
                {
                    case CreateType.SolidBlock:
                    {
                        if (GameSettingsData.Instance.CreateColliders)
                        {
                            commandBuffer.AddComponent<BlockCreateColliderTagData>(index, instance);
                        }

                        /*var mesh = new RenderMesh
                        {
                            mesh = GameSettingsData.Mesh,
                            material = GameSettingsData.Material,
                            castShadows = ShadowCastingMode.On,
                            receiveShadows = true,
                            layer = 1
                        };

                        commandBuffer.AddSharedComponent(index, instance, mesh);
                        commandBuffer.AddComponent(index, instance, new PerInstanceCullingTag());*/

                        break;
                    }
                }

                return instance;
            }

            return Entity.Null;
        }
    }
}
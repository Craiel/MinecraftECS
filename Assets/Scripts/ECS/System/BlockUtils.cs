using System;
using DefaultNamespace;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
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
            Material material = GameSettingsData.Instance.GetMaterial(data.BlockType);
            if (material == null)
            {
                return Entity.Null;
            }
            
            var translation = new Translation {Value = new float3(data.X, data.Y, data.Z)};
            Entity instance = commandBuffer.CreateEntity(index, GameSettingsData.Instance.BlockArchetype);
            WorldData.Set(data.X, data.Y, data.Z, instance);

            commandBuffer.AddComponent<BlockData>(index, instance);

            var blockData = new BlockData
            {
                Type = data.BlockType
            };

            commandBuffer.SetComponent(index, instance, blockData);
            commandBuffer.SetComponent(index, instance, translation);

            Mesh mesh = GameSettingsData.Instance.BlockMesh;
            switch (data.BlockType)
            {
                case BlockType.DirtGrass:
                {
                    mesh = GameSettingsData.Instance.BlockMesh2;
                    break;
                }

                case BlockType.TallGrass:
                case BlockType.Rose:
                {
                    mesh = GameSettingsData.Instance.FoliageMesh;
                    break;
                }
            }

            switch (data.CreateType)
            {
                case CreateType.SolidBlock:
                {
                    if (GameSettingsData.Instance.CreateColliders)
                    {
                        commandBuffer.AddComponent<BlockCreateColliderTagData>(index, instance);
                    }

                    var meshData = new RenderMesh
                    {
                        mesh = mesh,
                        material = material,
                        castShadows = ShadowCastingMode.On,
                        receiveShadows = true,
                        layer = 0
                    };

                    commandBuffer.AddComponent<LocalToWorld>(index, instance);
                    commandBuffer.AddSharedComponent(index, instance, meshData);

                    break;
                }
            }

            return instance;
        }
    }
}
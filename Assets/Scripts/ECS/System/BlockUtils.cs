using System;
using System.Collections.Generic;
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
	    private const int TextureBlockWidth = 4;
	    private const int TextureBlockHeight = 4;
	    private const float TextureUVOffsetX = 1f / TextureBlockWidth;
	    private const float TextureUVOffsetY = 1f / TextureBlockHeight;

        public static Unity.Mathematics.Random BlockRandom = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);

        private static Mesh[][] BlockMeshes;

        public static void InitializeMeshes()
        {
	        BlockMeshes = new Mesh[TextureBlockWidth][];
	        for (var x = 0; x < TextureBlockWidth; x++)
	        {
		        BlockMeshes[x] = new Mesh[TextureBlockHeight];
		        for (var y = 0; y < TextureBlockHeight; y++)
		        {
			        BlockMeshes[x][y] = CreateBlockMesh(x, y);
		        }
	        }

	        UnityEngine.Debug.LogFormat("Created {0} Block Meshes", TextureBlockWidth * TextureBlockHeight);
        }

        public static BlockType GetBaseBlockTypeFromDepth(int depth, int groundHeight)
        {
            if (depth >= groundHeight)
            {
                return BlockType.Air;
            }

            if (depth == groundHeight - 1)
            {
                return BlockType.Grass;
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

        private static readonly IDictionary<BlockType, Vector2Int> BlockMeshLookup =
	        new Dictionary<BlockType, Vector2Int>
	        {
		        {BlockType.Stone, new Vector2Int(0, 1)},
		        {BlockType.Bedrock, new Vector2Int(2, 1)},
		        {BlockType.CobbleStone, new Vector2Int(1, 1)},
		        {BlockType.Dirt, new Vector2Int(0, 0)},
		        {BlockType.Grass, new Vector2Int(1, 0)},
		        {BlockType.OakPlank, new Vector2Int(3, 3)},
		        {BlockType.OakWood, new Vector2Int(0, 3)},
		        {BlockType.OakLeaves, new Vector2Int(1, 3)},
		        {BlockType.Poppy, new Vector2Int(1, 2)},
		        {BlockType.TallGrass, new Vector2Int(0, 2)},
		        {BlockType.Glass, new Vector2Int(3, 2)},
		        {BlockType.Brick, new Vector2Int(3, 1)},
		        {BlockType.TNT, new Vector2Int(3, 0)},
		        {BlockType.Cloud, new Vector2Int(2, 0)},
	        };

        public static Entity Create(EntityCommandBuffer.Concurrent commandBuffer, int index, BlockCreateData data)
        {
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

            Mesh mesh = null;
            Material material = null;
            if (BlockMeshLookup.TryGetValue(data.BlockType, out Vector2Int blockMeshId))
            {
	            mesh = BlockMeshes[blockMeshId.x][blockMeshId.y];
            }

            switch (data.BlockType)
            {
                case BlockType.TallGrass:
                case BlockType.Poppy:
                {
                    mesh = GameSettingsData.Instance.FoliageMesh;
                    break;
                }

                case BlockType.Cloud:
                case BlockType.Glass:
                case BlockType.OakLeaves:
                {
                    material = GameSettingsData.Instance.PackedMaterialTransparent;
                    break;
                }

                default:
                {
                    material = GameSettingsData.Instance.PackedMaterial;
                    break;
                }
            }

            if (mesh == null || material == null)
            {
	            return Entity.Null;
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

        public static Vector2[] GetBlockUV(int x, int y)
        {
            float xOffset = x * TextureUVOffsetX;
            float yOffset = y * TextureUVOffsetY;

            Vector2 _00 = new Vector2(xOffset, yOffset);
            Vector2 _10 = new Vector2(xOffset + TextureUVOffsetX, yOffset);
            Vector2 _01 = new Vector2(xOffset, yOffset + TextureUVOffsetY);
            Vector2 _11 = new Vector2(xOffset + TextureUVOffsetX, yOffset + TextureUVOffsetY);

            return new[] {
	            // Bottom
	            _11, _01, _00, _10,

	            // Left
	            _11, _01, _00, _10,

	            // Front
	            _11, _01, _00, _10,

	            // Back
	            _11, _01, _00, _10,

	            // Right
	            _11, _01, _00, _10,

	            // Top
	            _11, _01, _00, _10,
            };
        }

        public static Mesh CreateBlockMesh(int x, int y)
        {
	        Mesh mesh = new Mesh();

	        float length = 1f;
	        float width = 1f;
	        float height = 1f;

	        Vector3 p0 = new Vector3(-length * .5f, -width * .5f, height * .5f);
	        Vector3 p1 = new Vector3(length * .5f, -width * .5f, height * .5f);
	        Vector3 p2 = new Vector3(length * .5f, -width * .5f, -height * .5f);
	        Vector3 p3 = new Vector3(-length * .5f, -width * .5f, -height * .5f);

	        Vector3 p4 = new Vector3(-length * .5f, width * .5f, height * .5f);
	        Vector3 p5 = new Vector3(length * .5f, width * .5f, height * .5f);
	        Vector3 p6 = new Vector3(length * .5f, width * .5f, -height * .5f);
	        Vector3 p7 = new Vector3(-length * .5f, width * .5f, -height * .5f);

	        Vector3[] vertices = {
		        // Bottom
		        p0, p1, p2, p3,

		        // Left
		        p7, p4, p0, p3,

		        // Front
		        p4, p5, p1, p0,

		        // Back
		        p6, p7, p3, p2,

		        // Right
		        p5, p6, p2, p1,

		        // Top
		        p7, p6, p5, p4
	        };

	        Vector3 up = Vector3.up;
	        Vector3 down = Vector3.down;
	        Vector3 front = Vector3.forward;
	        Vector3 back = Vector3.back;
	        Vector3 left = Vector3.left;
	        Vector3 right = Vector3.right;

	        Vector3[] normales = {
		        // Bottom
		        down, down, down, down,

		        // Left
		        left, left, left, left,

		        // Front
		        front, front, front, front,

		        // Back
		        back, back, back, back,

		        // Right
		        right, right, right, right,

		        // Top
		        up, up, up, up
	        };

	        int[] triangles = {
		        // Bottom
		        3, 1, 0,
		        3, 2, 1,

		        // Left
		        3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
		        3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

		        // Front
		        3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
		        3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

		        // Back
		        3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
		        3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

		        // Right
		        3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
		        3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

		        // Top
		        3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
		        3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

	        };

	        mesh.vertices = vertices;
	        mesh.normals = normales;
	        mesh.uv = GetBlockUV(x, y);
	        mesh.triangles = triangles;

	        mesh.RecalculateBounds();
	        mesh.Optimize();

	        return mesh;
        }
    }
}
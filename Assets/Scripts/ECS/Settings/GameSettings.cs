using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class GameSettings : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        [Header("World = ChunkBase x ChunkBase")]
        [SerializeField] public int ChunkBase = 1;

        [SerializeField] public GameObject[] Blocks;

        [SerializeField]
        public bool CreateColliders = true;

        [SerializeField]
        public Mesh BlockMesh;

        [SerializeField]
        public Material BlockMaterial;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            for (var i = 0; i < this.Blocks.Length; i++)
            {
                if (this.Blocks[i] != null)
                {
                    referencedPrefabs.Add(this.Blocks[i]);
                }
            }
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            GameSettingsData.Mesh = this.BlockMesh;
            GameSettingsData.Material = this.BlockMaterial;

            GameSettingsData.Instance = new GameSettingsData
            {
                WorldChunkSize = this.ChunkBase,
                CreateColliders = this.CreateColliders,
                BlockStone = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Stone]),
                BlockCobbleStone = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.CobbleStone]),
                BlockDirt = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Dirt]),
                BlockDirtGrass = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.DirtGrass]),
                BlockPlank = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Plank]),
                BlockGlass = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Glass]),
                BlockWood = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Wood]),
                BlockTNT = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.TNT]),
                BlockBrick = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Brick]),
                BlockTallGrass = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.TallGrass]),
                BlockRose = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Rose]),
                BlockCloud = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Cloud]),
                BlockLeaves = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Leaves]),
                BlockBedrock = conversionSystem.GetPrimaryEntity(this.Blocks[(int)BlockType.Bedrock]),
                BlockArchetype = World.Active.EntityManager.CreateArchetype(
                    typeof(Translation),
                    typeof(BlockData)
                ),
                BlockCreateArchetype = World.Active.EntityManager.CreateArchetype(
                    typeof(BlockCreateData)
                ),
                BlockDestroyArchetype = World.Active.EntityManager.CreateArchetype(
                    typeof(BlockDestroyData)),
                StructureCreateArchetype = World.Active.EntityManager.CreateArchetype(
                    typeof(StructureCreateData))
            };

            dstManager.AddComponentData(entity, new GameInitData());
        }
    }
}

using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class GameSettings : MonoBehaviour
    {
        [Header("World = ChunkBase x ChunkBase")]
        [SerializeField] public int ChunkBase = 1;

        [SerializeField]
        public bool CreateColliders = true;

        [SerializeField]
        public Mesh BlockMesh;

        [SerializeField]
        public Mesh BlockMesh2;

        [SerializeField]
        public Mesh FoliageMesh;

        [SerializeField]
        public Material[] BlockMaterials;

        public void Awake()
        {
            GameSettingsData.Instance = new GameSettingsData
            {
                WorldChunkSize = this.ChunkBase,
                CreateColliders = this.CreateColliders,
                Initialized = true,
                BlockMesh = this.BlockMesh,
                BlockMesh2 = this.BlockMesh2,
                FoliageMesh = this.FoliageMesh,
                BlockMaterials = this.BlockMaterials,
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

            World.Active.EntityManager.CreateEntity(typeof(GameInitData));
        }
    }
}

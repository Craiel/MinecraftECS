using Unity.Entities;
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
        public Mesh FoliageMesh;

        [SerializeField]
        public Material PackedMaterial;

        [SerializeField]
        public Material PackedMaterialTransparent;

        public void Awake()
        {
            GameSettingsData.Instance = new GameSettingsData
            {
                WorldChunkSize = this.ChunkBase,
                CreateColliders = this.CreateColliders,
                Initialized = true,
                FoliageMesh = this.FoliageMesh,
                PackedMaterial = this.PackedMaterial,
                PackedMaterialTransparent = this.PackedMaterialTransparent,
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

            BlockUtils.InitializeMeshes();

            World.Active.EntityManager.CreateEntity(typeof(GameInitData));
        }
    }
}

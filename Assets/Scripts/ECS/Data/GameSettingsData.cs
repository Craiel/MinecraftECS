using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Minecraft
{
    public struct GameSettingsData
    {
        public static GameSettingsData Instance;

        public int WorldChunkSize;

        public bool CreateColliders;

        public bool Initialized;

        public EntityArchetype BlockArchetype;
        public EntityArchetype BlockCreateArchetype;
        public EntityArchetype BlockDestroyArchetype;
        public EntityArchetype StructureCreateArchetype;

        public Mesh BlockMesh;
        public Mesh BlockMesh2;
        public Mesh FoliageMesh;
        public Material[] BlockMaterials;

        public Material GetMaterial(BlockType type)
        {
            return BlockMaterials[(int)type];
        }
    }
}
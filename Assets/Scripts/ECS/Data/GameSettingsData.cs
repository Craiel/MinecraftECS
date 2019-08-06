using Unity.Entities;
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

        public Mesh FoliageMesh;

        public Material PackedMaterial;
        public Material PackedMaterialTransparent;
    }
}
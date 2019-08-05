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

        public Entity BlockStone;
        public Entity BlockCobbleStone;
        public Entity BlockDirt;
        public Entity BlockDirtGrass;
        public Entity BlockPlank;
        public Entity BlockGlass;
        public Entity BlockWood;
        public Entity BlockTNT;
        public Entity BlockBrick;
        public Entity BlockTallGrass;
        public Entity BlockRose;
        public Entity BlockCloud;
        public Entity BlockLeaves;
        public Entity BlockBedrock;

        public EntityArchetype BlockArchetype;
        public EntityArchetype BlockCreateArchetype;
        public EntityArchetype BlockDestroyArchetype;
        public EntityArchetype StructureCreateArchetype;

        public static Mesh Mesh;
        public static Material Material;

        public Entity GetPrefab(BlockType type)
        {
            switch (type)
            {
                case BlockType.Stone:
                {
                    return this.BlockStone;
                }

                case BlockType.CobbleStone:
                {
                    return this.BlockCobbleStone;
                }

                case BlockType.Dirt:
                {
                    return this.BlockDirt;
                }

                case BlockType.DirtGrass:
                {
                    return this.BlockDirtGrass;
                }

                case BlockType.Glass:
                {
                    return this.BlockGlass;
                }

                case BlockType.Plank:
                {
                    return this.BlockPlank;
                }

                case BlockType.Wood:
                {
                    return this.BlockWood;
                }

                case BlockType.TNT:
                {
                    return this.BlockTNT;
                }

                case BlockType.Brick:
                {
                    return this.BlockBrick;
                }

                case BlockType.TallGrass:
                {
                    return this.BlockTallGrass;
                }

                case BlockType.Rose:
                {
                    return this.BlockRose;
                }

                case BlockType.Cloud:
                {
                    return this.BlockCloud;
                }

                case BlockType.Leaves:
                {
                    return this.BlockLeaves;
                }

                case BlockType.Bedrock:
                {
                    return this.BlockBedrock;
                }
            }

            return Entity.Null;
        }
    }
}
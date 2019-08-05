using DefaultNamespace;
using Unity.Entities;
using UnityEngine;

namespace Minecraft
{
    public static class WorldData
    {
        private static Entity[][][] Blocks;

        public static Bounds Bounds;

        public static void Initialize(int size)
        {
            int width = size * Constants.ChunkWidth;
            int length = size * Constants.ChunkLength;
            Blocks = new Entity[width][][];
            for (var x = 0; x < width; x++)
            {
                Blocks[x] = new Entity[length][];
                for (var z = 0; z < length; z++)
                {
                    Blocks[x][z] = new Entity[Constants.ChunkHeight];
                }
            }

            Bounds.SetMinMax(Vector3.zero, new Vector3(width, Constants.ChunkHeight, length));
        }

        public static void Set(int x, int y, int z, Entity entity)
        {
            Blocks[x][z][y] = entity;
        }

        public static Entity Get(int x, int y, int z)
        {
            return Blocks[x][z][y];
        }
    }
}
﻿using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class SpawnNumberBlocks: MonoBehaviour
    {
        //public static GameSettings GM;
        public static Texture2D Heightmap;

        public static EntityArchetype BlockArchetype;

        [Header("World = ChunkBase x ChunkBase")]
        public int ChunkBase = 1;

        [Header("Mesh Info")]
        public Mesh blockMesh;

        [Header("For Debug")]
        public Material no0Mat;
        public Material no1Mat;
        public Material no2Mat;
        public Material no3Mat;
        public Material no4Mat;
        public Material no5Mat;
        public Material no6Mat;
        public Material noQMat;


        Material maTemp;

        public Entity entities;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // Create an archetype for basic blocks.
            BlockArchetype = World.Active.EntityManager.CreateArchetype(
                typeof(Translation)
            );
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        void Start()
        {
            //Generate the world
            ChunkGenerator(ChunkBase);
        }

        void ChunkGenerator(int amount)
        {

            int totalamount = (amount * amount) * 1500;
            //int ordernumber = 0;
            int hightlevel;
            bool airChecker;


            //Block ordering from X*0,0,0 to 15,10,10( * Chunk x2)
            for (int yBlock = 0; yBlock < 15; yBlock++)
            {
                for (int xBlock = 0; xBlock < 10 * amount; xBlock++)
                {
                    for (int zBlock = 0; zBlock < 10 * amount; zBlock++)
                    {
                        hightlevel = (int)(Heightmap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                        airChecker = false;

                        switch (hightlevel)
                        {
                            case 0:
                                maTemp = no0Mat;
                                break;
                            case 1:
                                maTemp = no1Mat;
                                break;
                            case 2:
                                maTemp = no2Mat;
                                break;
                            case 3:
                                maTemp = no3Mat;
                                break;
                            case 4:
                                maTemp = no4Mat;
                                break;
                            case 5:
                                maTemp = no5Mat;
                                break;
                            case 6:
                                maTemp = no6Mat;
                                break;
                            default:
                                maTemp = noQMat;
                                airChecker = true;
                                break;
                        }

                        if (!airChecker)
                        {
                            Entity entities = World.Active.EntityManager.CreateEntity(BlockArchetype);
                            World.Active.EntityManager.SetComponentData(entities, new Translation { Value = new int3(xBlock, yBlock, zBlock) });
                            World.Active.EntityManager.AddComponentData(entities, new BlockTag { });

                            World.Active.EntityManager.AddSharedComponentData(entities, new RenderMesh
                            {
                                mesh = blockMesh,
                                material = maTemp
                            });
                        }
                    }
                }
            }
        }
    }
}

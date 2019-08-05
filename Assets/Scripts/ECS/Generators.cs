using Minecraft.ECS;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace ECS
{
/*public class Generators
{
    private static int ranDice;

    private static void ChunkGenerator(Entity prefab, int amount)
    {
        int totalamount = (amount * amount) * 1500;
        //int ordernumber = 0;
        int hightlevel;
        bool airChecker;

        Material maTemp;
        Mesh meshTemp;

        //Block ordering from X*0,0,0 to 15,10,10( * Chunk x2)
        for (int yBlock = 0; yBlock < 15; yBlock++)
        {

        }
    }
    private static void TreeGenerator(int xPos, int yPos, int zPos)
    {
        //xpos,ypos,zpos is the root position of the tree that we are going to plant.
        //woods
        for (int i = yPos; i < yPos + 7; i++)
        {
            //top leaves
            if (i == yPos + 6)
            {
                maTemp = leavesMaterial;
            }
            else
            {
                maTemp = woodMaterial;
            }

            if (!maTemp)
                maTemp = pinkMaterial;

            Entity entities = World.Active.EntityManager.CreateEntity(BlockArchetype);
            World.Active.EntityManager.SetComponentData(entities, new Translation { Value = new int3(xPos, i, zPos) });
            World.Active.EntityManager.AddComponentData(entities, new BlockTag { });
            World.Active.EntityManager.AddSharedComponentData(entities, new RenderMesh
            {
                mesh = blockMesh,
                material = maTemp
            });

            //leaves
            if(i >= yPos+3 && i <= yPos+6)
            {
                for (int j = xPos - 1; j <= xPos + 1; j++)
                {
                    for (int k = zPos - 1; k <= zPos + 1; k++)
                    {
                        if (k != zPos || j != xPos)
                        {
                            entities = World.Active.EntityManager.CreateEntity(BlockArchetype);
                            World.Active.EntityManager.SetComponentData(entities, new Translation { Value = new int3(j, i, k) });
                            World.Active.EntityManager.AddComponentData(entities, new BlockTag { });
                            World.Active.EntityManager.AddSharedComponentData(entities, new RenderMesh
                            {
                                mesh = blockMesh,
                                material = leavesMaterial
                            });
                        }
                    }
                }
            }
        }
    }

    private static void PlantGenerator(int xPos, int yPos, int zPos,int plantType)
    {

        //xpos,ypos,zpos is the root position of the plant that we are going to build.
        //rose
        if (plantType == 1)
        {
            maTemp = tallGrassMaterial;
        }
        else
        {
            maTemp = roseMaterial;
        }

        if (!maTemp)
            maTemp = pinkMaterial;

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Entity entities = World.Active.EntityManager.CreateEntity(BlockArchetype);
        World.Active.EntityManager.SetComponentData(entities, new Translation { Value = new int3(xPos, yPos, zPos) });
        World.Active.EntityManager.AddComponentData(entities, new Rotation { Value = rotation });
        World.Active.EntityManager.AddComponentData(entities, new SurfacePlantTag { });
        World.Active.EntityManager.AddSharedComponentData(entities, new RenderMesh
        {
            mesh = tallGrassMesh,
            material = maTemp
        });
    }

    private static void CloudGenerator(int xPos, int yPos, int zPos)
    {

        meshTemp = blockMesh;
        maTemp = CloudMaterial;

        if (!maTemp)
            maTemp = pinkMaterial;

        //ranDice = Unity.Mathematics.Random.(4, 7); this line doesn't work after preview12
        ranDice = UnityEngine.Random.Range(4, 7);

        for (int i = 0; i < ranDice; i++)
        {
            for (int j = 0; j < ranDice; j++)
            {
                Entity entities = World.Active.EntityManager.CreateEntity(BlockArchetype);
                World.Active.EntityManager.SetComponentData(entities, new Translation { Value = new int3(xPos+i, yPos + 15, zPos+j) });
                World.Active.EntityManager.AddSharedComponentData(entities, new RenderMesh
                {
                    mesh = meshTemp,
                    material = maTemp
                });
            }
        }
    }
}*/
}
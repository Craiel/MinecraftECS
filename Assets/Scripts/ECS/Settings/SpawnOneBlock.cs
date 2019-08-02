using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;

namespace Minecraft
{
    public class SpawnOneBlock : MonoBehaviour
    {

        public static EntityArchetype BlockArchetype;

        [Header("Mesh Info")]
        public Mesh blockMesh;

        [Header("Nature Block Type")]
        public Material blockMaterial;

        public Entity entities;
        public GameObject Prefab_ref;

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
            Entity entities = World.Active.EntityManager.CreateEntity(BlockArchetype);
            World.Active.EntityManager.SetComponentData(entities, new Translation { Value = new int3(2, 0, 0) });
            World.Active.EntityManager.AddComponentData(entities, new BlockTag { });

            World.Active.EntityManager.AddSharedComponentData(entities, new RenderMesh
            {
                mesh = blockMesh,
                material = blockMaterial
            });


            //use prefab to create a entity
            if (Prefab_ref)
            {
                NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);
                World.Active.EntityManager.Instantiate(Prefab_ref, entityArray);

                World.Active.EntityManager.SetComponentData(entityArray[0], new Translation { Value = new float3(4, 0f, 0f) });
                entityArray.Dispose();
            }
        }
    }
}

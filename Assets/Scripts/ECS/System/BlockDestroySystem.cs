using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Minecraft
{
    public class BlockDestroySystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnCreate()
        {
            this.entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        private struct CreateJob : IJobForEachWithEntity<BlockDestroyData>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref BlockDestroyData data)
            {
                Entity block = WorldData.Get(data.X, data.Y, data.Z);
                if (block != Entity.Null)
                {
                    this.CommandBuffer.DestroyEntity(index, block);
                    WorldData.Set(data.X, data.Y, data.Z, Entity.Null);
                }

                this.CommandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new CreateJob
            {
                CommandBuffer = this.entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);

            this.entityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }
}
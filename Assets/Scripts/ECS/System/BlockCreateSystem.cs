using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Minecraft
{
    public class BlockCreateSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnCreate()
        {
            this.entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        private struct CreateJob : IJobForEachWithEntity<BlockCreateData>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref BlockCreateData data)
            {
                BlockUtils.Create(this.CommandBuffer, index, data);
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
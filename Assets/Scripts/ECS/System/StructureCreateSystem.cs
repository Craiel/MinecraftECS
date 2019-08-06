
using System;
using DefaultNamespace;
using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Minecraft
{
    public class StructureCreateSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnCreate()
        {
            this.entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        private struct CreateJob : IJobForEachWithEntity<StructureCreateData>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref StructureCreateData data)
            {
                switch (data.Type)
                {
                    case StructureType.Cloud:
                    {
                        int roll = BlockUtils.BlockRandom.NextInt(4, 7);
                        for (int i = 0; i < roll; i++)
                        {
                            for (int j = 0; j < roll; j++)
                            {
                                if (data.X + i >= WorldData.Bounds.max.x
                                    || data.Z + j >= WorldData.Bounds.max.z)
                                {
                                    // Outside chunk border, ignore
                                    continue;
                                }

                                Entity part = this.CommandBuffer.CreateEntity(index, GameSettingsData.Instance.BlockCreateArchetype);
                                var createData = new BlockCreateData
                                {
                                    X = data.X + i,
                                    Y = Constants.CloudLevelHeight,
                                    Z = data.Z + j,
                                    BlockType = BlockType.Cloud
                                };

                                this.CommandBuffer.SetComponent(index, part, createData);
                            }
                        }

                        break;
                    }

                    case StructureType.Tree:
                    {
                        for (int i = data.Y; i < data.Y + 7; i++)
                        {
                            Entity stem = this.CommandBuffer.CreateEntity(index, GameSettingsData.Instance.BlockCreateArchetype);
                            var stemData = new BlockCreateData
                            {
                                X = data.X,
                                Y = i,
                                Z = data.Z,
                                BlockType = i == data.Y + 6 ? BlockType.Leaves : BlockType.Wood
                            };

                            this.CommandBuffer.SetComponent(index, stem, stemData);

                            // Canopy
                            if(i >= data.Y+3 && i <= data.Y+6)
                            {
                                for (int j = data.X - 1; j <= data.X + 1; j++)
                                {
                                    for (int k = data.Z - 1; k <= data.Z + 1; k++)
                                    {
                                        if (k != data.Z || j != data.X)
                                        {
                                            if (j < WorldData.Bounds.min.x || j >= WorldData.Bounds.max.x
                                                || k < WorldData.Bounds.min.z || k >= WorldData.Bounds.max.z)
                                            {
                                                // Outside world border, ignore
                                                continue;
                                            }

                                            Entity canopy = this.CommandBuffer.CreateEntity(index, GameSettingsData.Instance.BlockCreateArchetype);
                                            var canopyData = new BlockCreateData
                                            {
                                                X = j,
                                                Y = i,
                                                Z = k,
                                                BlockType = BlockType.Leaves
                                            };

                                            this.CommandBuffer.SetComponent(index, canopy, canopyData);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
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
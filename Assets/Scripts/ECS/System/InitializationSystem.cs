using System;
using DefaultNamespace;
using Enums;
using Minecraft.ECS;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class InitializationSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnCreate()
        {
            PerlinNoiseGenerator.Generate();

            this.entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        private struct GenerateJob : IJobForEachWithEntity<GameInitData>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref GameInitData init)
            {
                WorldData.Initialize(GameSettingsData.Instance.WorldChunkSize);
                int count = 0;
                for (int yBlock = 0; yBlock < Constants.ChunkHeight; yBlock++)
                {
                    for (int xBlock = 0; xBlock < Constants.ChunkWidth * GameSettingsData.Instance.WorldChunkSize; xBlock++)
                    {
                        for (int zBlock = 0; zBlock < Constants.ChunkLength * GameSettingsData.Instance.WorldChunkSize; zBlock++)
                        {
                            CreateType createType = CreateType.SolidBlock;
                            BlockType type = BlockType.Air;
                            int noiseHeight = (int) PerlinNoiseGenerator.GetHeight(xBlock, zBlock);
                            int groundHeight = Constants.SeaLevelHeight + noiseHeight;
                            int groundOffset = yBlock - (Constants.SeaLevelHeight + noiseHeight);

                            switch (groundOffset)
                            {
                                case Constants.CloudLevelHeight - Constants.SeaLevelHeight:
                                {
                                    if (BlockUtils.BlockRandom.NextInt(1, 200) >= 199)
                                    {
                                        var createEntity = this.CommandBuffer.CreateEntity(index,
                                            GameSettingsData.Instance.StructureCreateArchetype);
                                        var structureData = new StructureCreateData
                                        {
                                            X = xBlock,
                                            Y = yBlock,
                                            Z = zBlock,
                                            Type = StructureType.Cloud
                                        };

                                        this.CommandBuffer.SetComponent(index, createEntity, structureData);
                                    }

                                    continue;
                                }

                                case 0:
                                {
                                    //random surface block
                                    int surfaceRoll = BlockUtils.BlockRandom.NextInt(1, 201);
                                    if (surfaceRoll <= 20)
                                    {
                                        type = BlockType.TallGrass;
                                        createType = CreateType.FoliageBlock;
                                    }

                                    if (surfaceRoll == 200)
                                    {
                                        type = BlockType.Poppy;
                                        createType = CreateType.FoliageBlock;
                                    }
                                    else if (surfaceRoll == 199)
                                    {
                                        var createEntity = this.CommandBuffer.CreateEntity(index,
                                            GameSettingsData.Instance.StructureCreateArchetype);
                                        var structureData = new StructureCreateData
                                        {
                                            X = xBlock,
                                            Y = yBlock,
                                            Z = zBlock,
                                            Type = StructureType.Tree
                                        };

                                        this.CommandBuffer.SetComponent(index, createEntity, structureData);
                                        continue;
                                    }
                                    else if (surfaceRoll == 198)
                                    {
                                        var createEntity = this.CommandBuffer.CreateEntity(index,
                                            GameSettingsData.Instance.StructureCreateArchetype);
                                        var structureData = new StructureCreateData
                                        {
                                            X = xBlock,
                                            Y = yBlock,
                                            Z = zBlock,
                                            Type = StructureType.Cloud
                                        };

                                        this.CommandBuffer.SetComponent(index, createEntity, structureData);
                                        continue;
                                    }

                                    break;
                                }

                                default:
                                {
                                    type = BlockUtils.GetBaseBlockTypeFromDepth(yBlock, groundHeight);
                                    break;
                                }
                            }

                            switch (type)
                            {
                                // Special blocks that do not need to be created
                                case BlockType.Air:
                                {
                                    continue;
                                }
                            }

                            var data = new BlockCreateData
                            {
                                X = xBlock,
                                Y = yBlock,
                                Z = zBlock,
                                BlockType = type,
                                CreateType = createType
                            };

                            // We create the block immediately here, better for performance
                            BlockUtils.Create(this.CommandBuffer, index, data);
                            count++;
                        }
                    }
                }

                Debug.LogFormat("Initialized with {0} Blocks!", count);
                this.CommandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (!GameSettingsData.Instance.Initialized)
            {
                return inputDeps;
            }

            var job = new GenerateJob
            {
                CommandBuffer = this.entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);

            this.entityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }
}
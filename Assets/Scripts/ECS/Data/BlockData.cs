using Enums;
using Unity.Entities;

namespace Minecraft
{
    public struct BlockData : IComponentData
    {
        public BlockType Type;
    }
}
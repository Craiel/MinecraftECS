using Enums;
using Unity.Entities;

namespace Minecraft
{
    public struct BlockCreateData : IComponentData
    {
        public CreateType CreateType;
        public BlockType BlockType;

        public int X;
        public int Y;
        public int Z;
    }
}
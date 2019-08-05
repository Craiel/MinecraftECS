using Enums;
using Unity.Entities;

namespace Minecraft
{
    public struct StructureCreateData : IComponentData
    {
        public int X;
        public int Y;
        public int Z;

        public StructureType Type;
    }
}
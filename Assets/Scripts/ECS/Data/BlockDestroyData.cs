using Unity.Entities;

namespace Minecraft
{
    public struct BlockDestroyData : IComponentData
    {
        public int X;
        public int Y;
        public int Z;
    }
}
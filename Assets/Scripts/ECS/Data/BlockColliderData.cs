using Unity.Entities;

namespace Minecraft
{
    // System state components are persistent and not destroyed along with
    // entities so we can use them to track when a block is added or removed.
    public struct BlockColliderData : ISystemStateComponentData
    {
        public int ColliderId;
    }
}
using System;

namespace DefaultNamespace
{
    public static class Constants
    {
        public const byte ChunkWidth = 16;
        public const byte ChunkLength = ChunkWidth;
        public const byte ChunkHeight = Byte.MaxValue;
        public const byte SeaLevelHeight = 30;
        public const byte CloudLevelHeight = SeaLevelHeight + 30;
    }
}
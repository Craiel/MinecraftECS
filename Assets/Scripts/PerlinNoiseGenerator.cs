﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minecraft.ECS
{
    public static class PerlinNoiseGenerator
    {
        public static float[][] NoiseHeightMap;
        static int textureWidth = 200;
        static int textureHeight = 200;

        static float scale1 = 1f;
        static float scale2 = 10f;
        static float scale3 = 20f;

        static float offsetX;
        static float offsetY;

        public static void Generate()
        {
            offsetX = Random.Range(0, 99999);
            offsetY = Random.Range(0, 99999);

            NoiseHeightMap = new float[textureWidth][];
            for (int x = 0; x < textureWidth; x++)
            {
                NoiseHeightMap[x] = new float[textureHeight];
                for (int y = 0; y < textureHeight; y++)
                {
                    Color color = CalculateColor(x, y);
                    NoiseHeightMap[x][y] = color.r * 100f;
                }
            }
        }

        public static float GetHeight(int x, int y)
        {
            return NoiseHeightMap[x][y];
        }

        private static Color CalculateColor(int x, int y)
        {
            float xCoord1 = (float)x / textureWidth * scale1 + offsetX;
            float yCoord1 = (float)y / textureHeight * scale1 + offsetY;
            float xCoord2 = (float)x / textureWidth * scale2 + offsetX;
            float yCoord2 = (float)y / textureHeight * scale2 + offsetY;
            float xCoord3 = (float)x / textureWidth * scale3 + offsetX;
            float yCoord3 = (float)y / textureHeight * scale3 + offsetY;

            float sample1 = Mathf.PerlinNoise(xCoord1, yCoord1) / 15;
            float sample2 = Mathf.PerlinNoise(xCoord2, yCoord2) / 15;
            float sample3 = Mathf.PerlinNoise(xCoord3, yCoord3) / 15;

            return new Color(sample1 + sample2 + sample3, sample1 + sample2 + sample3, sample1 + sample2 + sample3);
        }
    }
}

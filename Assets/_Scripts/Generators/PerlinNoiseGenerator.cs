using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator {
      public int[,] GenerateBiomesMap(float chunkX, float chunkY)
    {
        int[,] map = new int[64, 64];
        float offset = Constants.BIOMES_OFFSET;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkX), offset + (y + chunkY), 500f, 8);

                map[x, y] = 0;
                if(perlinValue < .9f)
                {
                    map[x, y] = 1;
                }
            }
        }

        return map;
    }

    public int[,] GenerateWaterMap(float chunkX, float chunkY)
    {
        int[,] map = new int[66, 66];
        float offset = Constants.WATER_OFFSET;

        for (int x = 0; x < 66; x++)
        {
            for (int y = 0; y < 66; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkX - 1), offset + (y + chunkY - 1), 150f - 25f * WorldSettings.WATER_FREQUENCY, 6);

                map[x, y] = 0;
                if (perlinValue < .5f + .05f * WorldSettings.WATER_SIZE)
                {
                    map[x, y] = 3;
                } else if(perlinValue < .55f + .05f * WorldSettings.WATER_SIZE)
                {
                    map[x, y] = 2;
                } else if(perlinValue < .6f + .05f * WorldSettings.WATER_SIZE)
                {
                    map[x, y] = 1;
                }
            }
        }

        return map;        
    }

    public int[,] GenerateIronOreMap(float chunkX, float chunkY)
    {
        return null;
    }

    public int[,] GenerateCopperOreMap(float chunkX, float chunkY)
    {
        return null;
    }

    public int[,] GenerateCoalMap(float chunkX, float chunkY)
    {
        return null;
    }

    private float GetPerlinNoiseValueAt(float x, float y, float frequency, int octaves)
    {
        float returnValue = 0f;

        float gain = 1.0f;
        for (int i = 0; i < octaves; i++)
        { 
            returnValue += Mathf.PerlinNoise(x * gain / frequency, y * gain / frequency) / gain;
            gain *= 2.0f;
        }

        return returnValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoiseGenerator {

     public static TilesEnum[,] GenerateBiomesMap(Vector2 chunkCoordinates)
     {
        chunkCoordinates *= GlobalVariables.TILE_PER_CHUNK_AXIS;

        TilesEnum[,] map = new TilesEnum[64, 64];
        float offset = GlobalVariables.BIOMES_OFFSET;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkCoordinates.x), offset + (y + chunkCoordinates.y), 500f, 8);

                map[x, y] = TilesEnum.GRASS;
                if(perlinValue < .9f)
                {
                    map[x, y] = TilesEnum.SAVANNAH;
                }
            }
        }

        return map;
     }

    public static TilesEnum[,] GenerateWaterMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= GlobalVariables.TILE_PER_CHUNK_AXIS;
        TilesEnum[,] map = new TilesEnum[64, 64];
        float offset = GlobalVariables.WATER_OFFSET;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkCoordinates.x), offset + (y + chunkCoordinates.y), 150f - 25f * WorldSettings.WATER_FREQUENCY, 6);
                perlinValue -= .05f * WorldSettings.WATER_SIZE;
                if (perlinValue < .5f)
                {
                    map[x, y] = TilesEnum.DARK_WATER;
                } else if(perlinValue < .55f)
                {
                    map[x, y] = TilesEnum.NORMAL_WATER;
                } else if(perlinValue < .6f)
                {
                    map[x, y] = TilesEnum.LIGHT_WATER;
                }
            }
        }

        return map;        
    }

    public static bool[,] GenerateIronOreMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= GlobalVariables.TILE_PER_CHUNK_AXIS;
        bool[,] map = new bool[64, 64];
        float offset = GlobalVariables.RESOURCES_OFFSET;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkCoordinates.x), offset + (y + chunkCoordinates.y), 150f - 25f * WorldSettings.IRON_ORE_FREQUENCY, 6);
                perlinValue -= .05f * WorldSettings.IRON_ORE_SIZE;

                map[x, y] = false;
                if (perlinValue < .5f)
                {
                    map[x, y] = true;
                }
            }
        }

        return map;
    }

    public static bool[,] GenerateCoalMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= GlobalVariables.TILE_PER_CHUNK_AXIS;
        bool[,] map = new bool[64, 64];
        float offset = GlobalVariables.RESOURCES_OFFSET + 10000;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkCoordinates.x), offset + (y + chunkCoordinates.y), 150f - 25f * WorldSettings.COAL_FREQUENCY, 6);
                perlinValue -= .05f * WorldSettings.COAL_SIZE;

                map[x, y] = false;
                if (perlinValue < .5f)
                {
                    map[x, y] = true;
                }
            }
        }

        return map;
    }

    private static float GetPerlinNoiseValueAt(float x, float y, float frequency, int octaves)
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

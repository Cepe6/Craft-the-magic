using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoiseGenerator {
     public static int[,] GenerateBiomesMap(Vector2 chunkCoordinates)
     {
        chunkCoordinates *= Constants.TILE_PER_CHUNK_AXIS;

        int[,] map = new int[64, 64];
        float offset = Constants.BIOMES_OFFSET;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkCoordinates.x), offset + (y + chunkCoordinates.y), 500f, 8);

                map[x, y] = 0;
                if(perlinValue < .9f)
                {
                    map[x, y] = 1;
                }
            }
        }

        return map;
    }

    public static int[,] GenerateWaterMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= Constants.TILE_PER_CHUNK_AXIS;
        int[,] map = new int[66, 66];
        float offset = Constants.WATER_OFFSET;

        for (int x = 0; x < 66; x++)
        {
            for (int y = 0; y < 66; y++)
            {
                float perlinValue = GetPerlinNoiseValueAt(offset + (x + chunkCoordinates.x - 1), offset + (y + chunkCoordinates.y - 1), 150f - 25f * WorldSettings.WATER_FREQUENCY, 6);

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

    public static int[,] GenerateIronOreMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= Constants.TILE_PER_CHUNK_AXIS;
        return null;
    }

    public static int[,] GenerateCopperOreMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= Constants.TILE_PER_CHUNK_AXIS;
        return null;
    }

    public static int[,] GenerateCoalMap(Vector2 chunkCoordinates)
    {
        chunkCoordinates *= Constants.TILE_PER_CHUNK_AXIS;
        return null;
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

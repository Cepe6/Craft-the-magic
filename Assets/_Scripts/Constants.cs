using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants { 
    //Terrain perlin noise value offsets
    public static int BIOMES_OFFSET = 32768;
    public static int WATER_OFFSET = 65536;
    public static int RESOURCES_OFFSET = 131072;
    public static int TREES_OFFSET = 262144;

    public static int TILE_PER_CHUNK_AXIS = 64;
    public static int TILE_SIZE = 10;
    public static int CHUNK_SIZE_AXIS = 640;
}

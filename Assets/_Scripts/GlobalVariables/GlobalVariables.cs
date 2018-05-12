using UnityEngine;

public static class GlobalVariables { 
    //Terrain perlin
    public const int BIOMES_OFFSET = 32768;
    public const int WATER_OFFSET = 65536;
    public const int RESOURCES_OFFSET = 131072;
    public const int TREES_OFFSET = 262144;
    public const int TILE_PER_CHUNK_AXIS = 64;
    public const int TILE_SIZE = 10;
    
    //Inventory
    public const int MAX_STACK_AMMOUNT = 99;
    public static DragContainer ITEM_CONTAINER_BEING_DRAGGED = null;
    public static GameObject CURRENT_PLACABLE = null;
    public const int PLAYER_INVENTORY_SIZE = 100;
    public const int HOTBAR_SIZE = 10;
    public const int STORAGE_CONTAINER_SIZE = 100;
    
    //Drops
    public const int DROP_SIZE = 6;

    //Resource gathering
    public const float MINE_TIME = 2f;
    public const float EXTRACTOR_MINE_TIME = 1f;

    //Player interact distance
    public const float INTERACT_DISTANCE = 20f;

    //Fuel ammount
    public const float COAL_FUEL_AMMOUNT = 4f;

    //Smelter smelt time
    public const float SMELTER_SMELT_TIME = 2f;
}

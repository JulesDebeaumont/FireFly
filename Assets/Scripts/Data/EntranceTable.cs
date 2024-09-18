public static class EntranceTable
{
    private static int NextSceneId = 0;
    private static int NextSpawnId = 0;

    public static void SetupNextSpawn(int sceneId, int spawnId)
    {
        NextSceneId = sceneId;
        NextSpawnId = spawnId;
    }

    // TODO table
}

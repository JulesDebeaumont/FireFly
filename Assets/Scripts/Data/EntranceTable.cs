public static class EntranceTable
{
  // TODO Monobehvior ?
    private static int _nextSceneId = 0;
    private static int _nextSpawnId = 0;

    public static void SetupNextSpawn(int sceneId, int spawnId)
    {
        _nextSceneId = sceneId;
        _nextSpawnId = spawnId;
    }

    // TODO table
}

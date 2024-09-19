public static class EntranceTable
{
  // TODO Monobehvior ?
    private static int _sNextSceneId = 0;
    private static int _sNextSpawnId = 0;

    public static void SetupNextSpawn(int sceneId, int spawnId)
    {
        _sNextSceneId = sceneId;
        _sNextSpawnId = spawnId;
    }

    // TODO table
}

namespace Data.Tables
{
    public static class EntranceTable
    {
        // TODO Monobehvior ?
        private static int _nextSceneId;
        private static int _nextSpawnId;

        public static void SetupNextSpawn(int sceneId, int spawnId)
        {
            _nextSceneId = sceneId;
            _nextSpawnId = spawnId;
        }

        // TODO table
    }
}
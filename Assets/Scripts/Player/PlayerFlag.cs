using System.Collections.Generic;
using Manager;

namespace Player
{
    public abstract class PlayerFlag
    {
        public enum EWorldFlagType // Move into /tables
        {
            OPENING_DONE
        }

        private Dictionary<int, bool[]> _sceneFlags = new();
        private Dictionary<EWorldFlagType, bool> _worldFlags = new();
        public Player Player;
        public SceneCustomManager SceneCustomManager;

        public PlayerFlag()
        {
            LoadFlagFromSave();
        }

        private void LoadFlagFromSave()
        {
            SaveManager.CreateSaveFile(0); // TODO remove once file is created
            var saveFile = SaveManager.ReadCurrentSaveFile();
            _sceneFlags = saveFile.SceneFlags;
            _worldFlags = saveFile.WorldFlags;
        }

        public bool IsSceneFlagSet(int sceneId, int index)
        {
            return _sceneFlags[sceneId][index];
        }

        public void SetSceneFlag(int sceneId, int index)
        {
            _sceneFlags[sceneId][index] = true;
        }

        public void UnsetSceneFlag(int sceneId, int index)
        {
            _sceneFlags[sceneId][index] = false;
        }

        public bool IsCurrentSceneFlagSet(int index)
        {
            return _sceneFlags[SceneCustomManager.Instance.currentSceneId][index];
        }

        public void SetCurrentSceneFlag(int index)
        {
            _sceneFlags[SceneCustomManager.Instance.currentSceneId][index] = true;
        }

        public void UnsetCurrentSceneFlag(int index)
        {
            _sceneFlags[SceneCustomManager.Instance.currentSceneId][index] = false;
        }

        public void SetWorldFlag(EWorldFlagType type)
        {
            _worldFlags[type] = true;
        }

        public void UnsetWorldFlag(EWorldFlagType type)
        {
            _worldFlags[type] = false;
        }

        public bool IsWorldFlagSet(EWorldFlagType type)
        {
            return _worldFlags[type];
        }
    }
}
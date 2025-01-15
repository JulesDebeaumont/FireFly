using Manager;

namespace Actors.Handlers
{
    public class FlagHandler
    {
        private readonly int _flagId;
        
        public FlagHandler(int flagId)
        {
            _flagId = flagId;
        }

        public bool IsCurrentSceneFlagSet()
        {
            return PlayerManager.Instance.player.PlayerFlag.IsCurrentSceneFlagSet(_flagId);
        }

        public void SetCurrentSceneFlag()
        {
            PlayerManager.Instance.player.PlayerFlag.SetCurrentSceneFlag(_flagId);
        }

        public void UnsetCurrentSceneFlag()
        {
            PlayerManager.Instance.player.PlayerFlag.UnsetCurrentSceneFlag(_flagId);
        }
    }
}
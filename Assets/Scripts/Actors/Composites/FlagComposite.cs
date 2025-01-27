using Manager;

namespace Actors.Composites
{
    public class FlagComposite
    {
        public bool IsCurrentSceneFlagSet(int flagId)
        {
            return PlayerManager.Instance.player.PlayerFlag.IsCurrentSceneFlagSet(flagId);
        }

        public void SetCurrentSceneFlag(int flagId)
        {
            PlayerManager.Instance.player.PlayerFlag.SetCurrentSceneFlag(flagId);
        }

        public void UnsetCurrentSceneFlag(int flagId)
        {
            PlayerManager.Instance.player.PlayerFlag.UnsetCurrentSceneFlag(flagId);
        }
    }
}

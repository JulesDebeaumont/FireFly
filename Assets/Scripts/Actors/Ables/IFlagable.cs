using Manager;

namespace Actors.Ables
{
    public interface IFlagable
    {
        int FlagId { get; }
        
        public bool IsCurrentSceneFlagSet()
        {
            return PlayerManager.Instance.player.PlayerFlag.IsCurrentSceneFlagSet(FlagId);
        }

        public void SetCurrentSceneFlag()
        {
            PlayerManager.Instance.player.PlayerFlag.SetCurrentSceneFlag(FlagId);
        }

        public void UnsetCurrentSceneFlag()
        {
            PlayerManager.Instance.player.PlayerFlag.UnsetCurrentSceneFlag(FlagId);
        }
    }
}

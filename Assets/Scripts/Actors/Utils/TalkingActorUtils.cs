using UnityEngine;

public struct TalkingActorUtils
{
    private Transform _actorTransform;
    public TalkingActorUtils(Transform actorTransform)
    {
      _actorTransform = actorTransform;
    }

    public bool PlayerInFrontOfTransform()
    {
        return true;
        // check for ATap + in front of actor
    }

    public void StartDialog(int dialogId)
    {
        var dialogFound = DialogTable.GetDialogById(dialogId);
        UiManager.Instance.DialogManager.OpenDialog(dialogFound);
    }
}

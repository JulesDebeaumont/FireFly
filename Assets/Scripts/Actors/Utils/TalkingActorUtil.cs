using UnityEngine;

public struct TalkingActorUtils
{
    public bool PlayerInFrontOfTransform(Transform actorTransform)
    {
        return true;
        // check for ATap + in front of actor
    }

    public void StartDialog(int dialogId)
    {
        var dialogFound = DialogTable.GetDialogById(dialogId);
        DialogManager.Instance.OpenDialog(dialogFound);
    }
}

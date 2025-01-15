using Data.Tables;
using Manager;
using UnityEngine;

namespace Actors.Handlers
{
    public class NpcTalkHandler
    {
        private Transform _actorTransform;

        public NpcTalkHandler(Transform actorTransform)
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
            UiManager.Instance.dialogManager.OpenDialog(dialogFound);
        }
    }
}
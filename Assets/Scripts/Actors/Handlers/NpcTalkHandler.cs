using System.Collections.Generic;
using Data.Utils;
using Manager;
using UnityEngine;

namespace Actors.Handlers
{
    public class NpcTalkHandler
    {
        private Transform _actorTransform;
        private Dictionary<int, Dialog> _dialogs;

        public NpcTalkHandler(Transform actorTransform, string dialogFilename)
        {
            _actorTransform = actorTransform;
            _dialogs = DialogLoader.GetDialogsFromFile(dialogFilename);
        }

        public bool PlayerInFrontOfNpc()
        {
            return true;
            // in front of actor + no special state (no jump or whatever)
        }

        private Dialog FindDialogById(int dialogId)
        {
            return _dialogs.GetValueOrDefault(dialogId);
        }
        
        public void StartDialog(int dialogId)
        {
            var dialog = FindDialogById(dialogId);
            UiManager.Instance.dialogManager.OpenDialog(dialog);
        }
    }
}

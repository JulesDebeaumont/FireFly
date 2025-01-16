using Actors.Handlers;
using UnityEngine;

namespace Actors.Environments
{
    /*
     * Wooden breakable sign
     */
    public class WoodenSign : MonoBehaviour
    {
        public int dialogId = 1;
        private bool _isBroken;
        private NpcTalkHandler _npcTalkHandler;
        private const string DialogFilename = "wooden_sign";

        private void Awake()
        {
            _npcTalkHandler = new NpcTalkHandler(transform, DialogFilename);
        }

        private void Update()
        {
            if (_npcTalkHandler.PlayerInFrontOfNpc() && Input.GetKey(KeyCode.Y))
                _npcTalkHandler.StartDialog(dialogId);
        }

        protected void OnDisable()
        {
            _isBroken = false;
        }

        public void Break()
        {
        }
    }
}
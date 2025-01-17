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
        private TalkableHanlder _talkableHanlder;
        private const string DialogFilename = "wooden_sign";

        private void Awake()
        {
            _talkableHanlder = new TalkableHanlder(transform, DialogFilename);
        }

        private void Update()
        {
            if (_talkableHanlder.PlayerInFrontOfNpc() && Input.GetKey(KeyCode.Y))
                _talkableHanlder.StartDialog(dialogId);
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
using Actors.Ables;
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
        private ITalkable _talkable;
        private const string DialogFilename = "wooden_sign";

        private void Awake()
        {
            _talkable = new ITalkable(transform, DialogFilename);
        }

        private void Update()
        {
            if (_talkable.PlayerInFrontOfNpc() && Input.GetKey(KeyCode.Y))
                _talkable.StartDialog(dialogId);
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
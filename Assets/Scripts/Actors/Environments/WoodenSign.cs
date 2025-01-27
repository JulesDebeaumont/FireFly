using Actors.Composites;
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
        private TalkComposite _talkComposite;
        private const string DialogFilename = "wooden_sign";

        private void Awake()
        {
        }

        private void Update()
        {

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
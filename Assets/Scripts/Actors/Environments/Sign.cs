#nullable enable

using UnityEngine;

namespace Actors.Environments
{
    public class Sign : MonoBehaviour
    {
        public int dialogId = 1;
        private bool _isBroken;
        private TalkActorService _talkActorService;

        protected override void Awake()
        {
            base.Awake();
            _talkActorService = new TalkActorService();
        }

        private void Update()
        {
            if (_talkActorService.PlayerInFrontOfTransform() && Input.GetKey(KeyCode.Y))
                _talkActorService.StartDialog(dialogId);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _isBroken = false;
        }

        public void Break()
        {
        }
    }
}
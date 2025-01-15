using UnityEngine;

namespace Actors.Environments
{
    public class Torch : MonoBehaviour
    {
        public bool isLit;
        public int timer;
        private const int MilliSecondTimer = 5000;

        // TODO Model

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            // timer going down

            // if fire is near
            if (FireIsClose())
            {
                isLit = true;
                StartTimer();
            }

            if (isLit && timer == 0) Unlit();
        }

        private void StartTimer()
        {
            timer = MilliSecondTimer;
        }

        private void Unlit()
        {
            isLit = false;
            timer = 0;
        }

        private bool FireIsClose()
        {
            return false;
        }
    }
}
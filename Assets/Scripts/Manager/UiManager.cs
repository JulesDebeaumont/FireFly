using UnityEngine;

namespace Manager
{
    public class UiManager : MonoBehaviour
    {
        public DialogManager dialogManager;
        public PauseMenuManager pauseMenuManager;
        public static UiManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
    }
}
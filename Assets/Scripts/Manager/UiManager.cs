using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class UiManager : MonoBehaviour
    {
        [FormerlySerializedAs("DialogManager")] public DialogManager dialogManager;
        [FormerlySerializedAs("PauseMenuManager")] public PauseMenuManager pauseMenuManager;
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
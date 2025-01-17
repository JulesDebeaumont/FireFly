using UnityEngine;

namespace Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public Player.Player player;
        public static PlayerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
    }
}
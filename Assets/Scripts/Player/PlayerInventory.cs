using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Player player;
        private int _gemCount;
        private int _health = 12;
        private bool _bowObtained;
        private bool _bombObtained;
        private int _bowAmmo;

        private void Awake()
        {
            LoadInventory();
        }

        public void AddAmber(int amount)
        {
            // TODO logic with wallets
            _gemCount += amount;
        }

        public void RemoveGem(int amount)
        {
            _gemCount -= amount;
            if (_gemCount < 0) _gemCount = 0;
        }

        public void AddHealth(int amount)
        {
            _health += amount;
            // TODO max health calculate with heartpieces
        }

        public void RemoveHealth(int amount)
        {
            _health -= amount;
            // TODO sound effect
            // TODO invulnerable timeout
            if (_health < 0)
            {
                _health = 0;
            }
            if (_health == 0)
            {
                player.playerAction.Die();
            }
        }

        private void LoadInventory()
        {
            // TODO check in save file
        }

        public bool IsLowHealth()
        {
            return _health < 4;
        }
        
    }
}
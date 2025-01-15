using UnityEngine;

namespace Data.Architecture
{
    public class MainContext : MonoBehaviour
    {
        private static MainContext _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
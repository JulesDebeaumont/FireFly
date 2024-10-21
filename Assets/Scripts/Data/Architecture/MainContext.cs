using UnityEngine;

public class MainContext : MonoBehaviour
{
  private static MainContext Instance;
  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(this.gameObject);
    }
  }
}
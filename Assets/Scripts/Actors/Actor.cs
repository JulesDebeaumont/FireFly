using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Actor : MonoBehaviour
{
  protected ActorSpawnPosition _spawnPosition;

  protected virtual void Awake()
  {
    _spawnPosition = new ActorSpawnPosition(transform.position, transform.rotation);

    var className = this.GetType().Name;
    Debug.Log($"test Wake in Actor {className}");
  }

  protected virtual void OnDisable()
  {
    ResetToSpawnPosition();

    var className = this.GetType().Name;
    Debug.Log($"test On Disable in Actor {className}");
  }

  protected void ResetToSpawnPosition()
  {
    transform.position = _spawnPosition.Position;
    transform.rotation = _spawnPosition.Rotation;
  }

  protected struct ActorSpawnPosition
  {
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public ActorSpawnPosition(Vector3 position, Quaternion rotation)
    {
      Position = position;
      Rotation = rotation;
    }
  }
}

using UnityEngine;

namespace Actors.Ables
{
    public interface ISpawnResetable
    {
        Vector3 SpawnResetPosition { get; set; }
        Quaternion SpawnResetRotation { get; set; }
        Transform transform { get; }

        public void RegisterSpawnPosition()
        {
            SpawnResetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            SpawnResetRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
        
        public void ResetToSpawnPosition()
        {
            transform.position = SpawnResetPosition;
            transform.rotation = SpawnResetRotation;
        }

    }
}

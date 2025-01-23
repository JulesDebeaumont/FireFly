using UnityEngine;

namespace Actors.Ables
{
    public interface ISpawnResetable
    {
        Vector3 SpawnResetPosition { get; set; }
        Quaternion SpawnResetRotation { get; set; }
        Transform Transform { get; }

        public void RegisterSpawnPosition()
        {
            SpawnResetPosition = new Vector3(Transform.position.x, Transform.position.y, Transform.position.z);
            SpawnResetRotation = new Quaternion(Transform.rotation.x, Transform.rotation.y, Transform.rotation.z, Transform.rotation.w);
        }
        
        public void ResetToSpawnPosition()
        {
            Transform.position = SpawnResetPosition;
            Transform.rotation = SpawnResetRotation;
        }

    }
}
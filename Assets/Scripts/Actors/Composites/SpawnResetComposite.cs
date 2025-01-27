using UnityEngine;

namespace Actors.Composites
{
    public class SpawnResetComposite
    {
        private Vector3 _spawnResetPosition;
        private Quaternion _spawnResetRotation;

        public void RegisterSpawnPosition(Transform transform)
        {
            _spawnResetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            _spawnResetRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
        
        public void ResetToSpawnPosition(Transform transform)
        {
            transform.position = _spawnResetPosition;
            transform.rotation = _spawnResetRotation;
        }

    }
}

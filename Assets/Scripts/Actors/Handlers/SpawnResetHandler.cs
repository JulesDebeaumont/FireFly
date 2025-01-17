using UnityEngine;

namespace Actors.Handlers
{
    public class SpawnResetHandler
    {
        private readonly ActorSpawnPosition _spawnPosition;
        private readonly Transform _actorTransform;

        public SpawnResetHandler(Transform actorTransform)
        {
            _spawnPosition = new ActorSpawnPosition(actorTransform.position, actorTransform.rotation);
            _actorTransform = actorTransform;
        }
        
        public void ResetToSpawnPosition()
        {
            _actorTransform.position = _spawnPosition.Position;
            _actorTransform.rotation = _spawnPosition.Rotation;
        }

        private struct ActorSpawnPosition
        {
            public Vector3 Position { get; }
            public Quaternion Rotation { get; }

            public ActorSpawnPosition(Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }
    }
}
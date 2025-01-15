using UnityEngine;

namespace Actors.Handlers
{
    public class SpawnResetHandler
    {
        private readonly ActorSpawnPosition _spawnPosition;

        public SpawnResetHandler(Transform actorTransform)
        {
            _spawnPosition = new ActorSpawnPosition(actorTransform.position, actorTransform.rotation);
        }
        
        public void ResetToSpawnPosition(Transform actorTransform)
        {
            actorTransform.position = _spawnPosition.Position;
            actorTransform.rotation = _spawnPosition.Rotation;
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
using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Ables
{
    public interface IDropable
    {
        DropTable Droptable { get; }
        Transform Transform { get; }

        public void Drop(EDropSpawnAnimation animation)
        {
            DropableManager.Instance.AddToEntries(new DropableManager.DropableEntry(Transform.position, animation, Droptable));
        }
    }
}

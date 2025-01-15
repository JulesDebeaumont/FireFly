using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public new Rigidbody rigidbody;
        public new CapsuleCollider collider;
        public PlayerState playerState;
        public PlayerInventory playerInventory;
        public PlayerAction playerAction;
        public PlayerCamera playerCamera;
        public PlayerCameraEffect playerCameraEffect;
        public PlayerFlag PlayerFlag;
    }
}
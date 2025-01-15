using System.Collections.Generic;
using Data.Tables;

namespace Actors.Enemies
{
    public abstract class Enemy : Actor
    {
        protected static readonly DamageTable DamageTable = new()
        {
            Data = new Dictionary<DamageTable.EDamageType, int>
            {
                { DamageTable.EDamageType.ICE_DAMAGE, 10 },
                { DamageTable.EDamageType.FIRE_DAMAGE, 10 },
                { DamageTable.EDamageType.SWORD_VERTICAL_SLASH, 3 },
                { DamageTable.EDamageType.SWORD_HORIZONTAL_SLASH, 3 },
                { DamageTable.EDamageType.JUMPSLASH, 10 },
                { DamageTable.EDamageType.ARROW, 5 }
            }
        };

        protected bool HasStartedDying;
        protected int Health = 5;
        protected int InvincibilityTimer = 0;
        protected bool IsTargetable = true;

        public bool IsInvincibile => InvincibilityTimer > 0;

        public bool IsAlive => Health > 0;
        // public DropTable DropTable = new DropTable
        // {
        //     { }
        // }();

        // Start is called before the first frame update
        protected void Start()
        {
        }

        // Update is called once per frame
        protected void Update()
        {
            if (!IsAlive && HasStartedDying == false) Die();
            // detect any damage source and TakeDamage() if !IsInvicible
        }

        protected void Die()
        {
            HasStartedDying = true;
            // death anim
            DropItem();
            Destroy(gameObject);
        }

        protected void TakeDamage(DamageTable.EDamageType damageType, int multiplier)
        {
            if (!DamageTable.Data.ContainsKey(damageType)) return;
            var damage = DamageTable.Data[damageType];
            damage *= multiplier;
            Health -= damage;
            StartInvincibilityTimer();
        }

        protected void StartInvincibilityTimer()
        {
            // TODO
        }

        protected void DropItem()
        {
            // TODO
        }
    }
}
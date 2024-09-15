using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : Actor
{
    protected int INVINCIBILITY_TIMER = 800;
    protected int Health = 5;
    protected bool IsTargetable = true;
    protected bool HasStartedDying = false;
    protected int InvincibilityTimer = 0;
    public bool IsInvincibile
    {
        get
        {
            return this.InvincibilityTimer > 0;
        }
    }
    public bool IsAlive
    {
        get
        {
            return this.Health > 0;
        }
    }
    protected static readonly DamageTable DamageTable = new DamageTable
    {
        Data = new Dictionary<DamageTable.EDamageType, int>
        {
            { DamageTable.EDamageType.ICE_DAMAGE, 10 },
            { DamageTable.EDamageType.FIRE_DAMAGE, 10 },
            { DamageTable.EDamageType.SWORD_VERTICAL_SLASH, 3},
            { DamageTable.EDamageType.SWORD_HORIZONTAL_SLASH, 3},
            { DamageTable.EDamageType.JUMPSLASH, 10},
            { DamageTable.EDamageType.ARROW, 5}
        }
    };
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
        if (!this.IsAlive && this.HasStartedDying == false)
        {
            Die();
        }
        // detect any damage source and TakeDamage() if !IsInvicible
    }

    protected void Die()
    {
        this.HasStartedDying = true;
        // death anim
        DropItem();
        Destroy(this.gameObject);
    }

    protected void TakeDamage(DamageTable.EDamageType damageType, int multiplier)
    {
        if (!DamageTable.Data.ContainsKey(damageType))
        {
            return;
        }
        var damage = DamageTable.Data[damageType];
        damage *= multiplier;
        this.Health -= damage;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    public int money;
    /// <summary>角色动画组件</summary>
    public CharacterAnimation chAnim = null;
    public new void Start()
    {
        base.Start();
        chAnim = GetComponent<CharacterAnimation>();
    }
    public override int ApplyDamage(int damage, GameObject killer)
    {
        var damageVal = damage - defence;

        if (damageVal > 0)
        {
            HP -= damageVal;
            return damageVal;
        }
        return 0;
    }

    public override void Dead(GameObject killer)
    {
        chAnim.PlayAnimation("dead");
    }
}


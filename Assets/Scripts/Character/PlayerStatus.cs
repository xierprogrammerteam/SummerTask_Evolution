using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    /// <summary>
    /// 金币
    /// </summary>
    public int money;
    /// <summary>
    /// 进化点数
    /// </summary>
    public int EvolutionaryPoints;
    /// <summary>
    /// 解析度
    /// </summary>
    public int[] resolution= new int[4];
    /// <summary>
    /// 角色动画组件
    /// </summary>
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
    /// <summary>
    /// 增加解析度
    /// </summary>
    /// <param name="point">点数</param>
    /// <param name="id">霜狼，1；雷枭，2 </param>
    public void addResolution(int point,int id)
    {
        
    }



}


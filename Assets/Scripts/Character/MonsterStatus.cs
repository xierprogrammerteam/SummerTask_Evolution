using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小怪基础状态信息
/// </summary>
public class MonsterStatus : CharacterStatus
{
    /// <summary>
    /// 视野距离
    /// </summary>
    public int sightDistance;

    /// <summary>
    /// 小怪动画组件
    /// </summary>
    public CharacterAnimation chAnim = null;

    public new void Start()
    {
        base.Start();
        chAnim = GetComponent<CharacterAnimation>();
    }

    /// <summary>
    /// 重写父类的死亡方法 
    /// </summary>
    /// <param name="killer">杀手</param>
    public override void Dead(GameObject killer)
    {
        if (HP <= 0)
        {
            var status = killer.GetComponent<PlayerStatus>();
            //小怪死后要执行的方法
            if (status != null)
            {

            }
            chAnim.PlayAnimation("dead");
            //销毁
            GameObject.Destroy(gameObject, 3);
        }
    }

}

using UnityEngine;
using System.Collections;


/// <summary>
/// 角色状态 （表达所有角色共同的状态信息）
/// </summary>
public abstract class CharacterStatus : MonoBehaviour
{

    /// <summary>
    /// 生命
    /// </summary>
    public int HP = 100;
    /// <summary>
    /// 最大生命
    /// </summary>
    public int MaxHP = 100;
    /// <summary>
    /// 魔法值
    /// </summary>
    public int SP = 100;
    /// <summary>
    /// 最大魔法值
    /// </summary>
    public int MaxSP = 100;
    /// <summary>
    /// 伤害/攻击力
    /// </summary>
    public int damage = 100;
    /// <summary>
    /// 攻击速度
    /// </summary>
    public int attackSpeed = 5;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDistance = 2;
    /// <summary>
    /// 移动速度
    /// </summary>
    public int speed = 10;
    /// <summary>
    /// 防御力
    /// </summary>
    public int defence;
    /// <summary>
    /// 暴击率
    /// </summary>
    public int crit;
    /// <summary>
    /// 受击特效挂点 名字为“HitFxPos”
    /// </summary>
    [HideInInspector]
    public Transform HitFxPos;



    public void Start()
    {
        HitFxPos = TransformHelper.FindChild(transform, "HitFxPos");
    }

    #region 行为

    /// <summary>受击 模板方法</summary>
    public virtual void OnDamage(int damage, GameObject killer)
    {
        //应用伤害
        var damageVal = ApplyDamage(damage, killer);
        //应用死亡
        if (HP <= 0)
            Dead(killer);
    }

    /// <summary>应用伤害</summary>
    public virtual int ApplyDamage(int damage, GameObject killer)
    {
        HP -= damage;
        return damage;
    }


    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="killer">杀手</param>
    public virtual void Dead(GameObject killer)
    {

    }

    #endregion
}
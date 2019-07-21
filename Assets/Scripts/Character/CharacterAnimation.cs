﻿/*角色动画控制类
 *职责：负责动画切换，动画参数的调整，事画事件的处理
 */
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 动画控制类
/// </summary>
public class CharacterAnimation : MonoBehaviour
{
    /// <summary>引用动画组件</summary>
    private Animator anim;
    /// <summary>引用技能系统</summary>
    private CharacterSkillSystem chSystem;
    /// <summary>当前动画参数</summary>
    public string currentAnim = "idle";
    /// <summary>是否在播放攻击类动画</summary>
    public bool isAttack = false;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        chSystem = GetComponent<CharacterSkillSystem>();
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="paramName">动画状态机Bool型参数名</param>
    public void PlayAnimation(string paramName)
    {
        if (paramName.StartsWith("attack"))
            isAttack = true;
        anim.SetBool(currentAnim, false);
        anim.SetBool(paramName, true);
        currentAnim = paramName;
    }




}


using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 角色马达，控制角色的行动
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    //移动速度
    public float speed = 5;
    //转向速度
    public float rotationSpeed = 0.3f;
    //角色控制器
    private CharacterController ch = null;
    /// <summary>
    /// 跳跃曲线
    /// </summary>
    public AnimationCurve jumpCurve;
    /// <summary>
    /// 跳跃时间
    /// </summary>
    private float jumpTime = 30;
    private bool jumpDone = true;

    // private CharacterAnimation chAnimation = null;

    public void Start()
    {
        ch = GetComponent<CharacterController>();
       // chAnimation = GetComponent<CharacterAnimation>();
    }
    //移动
    public void Movement(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            //chAnimation.PlayAnimation("run");
            Rotating(horizontal, vertical);
            var direct = new Vector3(transform.forward.x, -1, transform.forward.z);
            ch.Move(direct * speed * Time.deltaTime);
        }
        else
        {
            //chAnimation.PlayAnimation("idle");
        }
    }
    /// <summary>
    /// 旋转
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void Rotating(float horizontal, float vertical)
    {
        var targetDirection = new Vector3(horizontal, 0, vertical);
        TransformHelper.LookAtTarget(targetDirection, transform, rotationSpeed);
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    public void JumpForSeconds()
    {

        if(jumpDone)
        {
            jumpDone = false;
            StartCoroutine(jumpForSeconds());
        }

    }

    IEnumerator jumpForSeconds()
    {
        int time = 0;
        float x = 0;
        float z = 0;
        float height = 0;
        while (time <= jumpTime)
        {
             x = transform.position.x;
             z = transform.position.z;
             height = jumpCurve.Evaluate(time);
            transform.position = new Vector3(x, height, z);
            yield return new WaitForEndOfFrame();
            time++;
        }
        jumpDone =true;

}
}





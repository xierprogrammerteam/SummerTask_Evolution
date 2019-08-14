using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;


[RequireComponent(typeof(CharacterMotor))]
public class CharacterInputController : MonoBehaviour
{
    //private Rigidbody playerRigidbody;
    private CharacterMotor motor;
    //private CharacterAnimation chAnimation;
    void Start()
    {
        motor = GetComponent<CharacterMotor>();
        //chAnimation = GetComponent<CharacterAnimation>();
    }
    public void onKeyDown(string keyName)
    {

    }







    // 判断是否落地
    private bool grounded = true;

    void Awake()
    {
        //playerRigidbody = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        motor.Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            motor.JumpForSeconds();
        }


    }

 


}
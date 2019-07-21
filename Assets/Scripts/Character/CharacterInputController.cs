using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;


namespace ARPGSimpleDemo.Character
{
    [RequireComponent(typeof(CharacterMotor))]
    public class CharacterInputController : MonoBehaviour
    {
        private CharacterMotor motor;
        private DateTime lastClickTime;

        private CharacterAnimation chAnimation;
        private CharacterSkillSystem chSystem ;

 

        void Start()
        {
            motor = GetComponent<CharacterMotor>();
            chSystem = GetComponent<CharacterSkillSystem>();
            chAnimation = GetComponent<CharacterAnimation>();
        }



        void Update()
        {
            motor.Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetKeyDown("fire0"))onKeyDown("BaseSkill");
            if (Input.GetKeyDown("fire1"))onKeyDown("Skill1");
            if (Input.GetKeyDown("fire2")) onKeyDown("Skill2");
            if (Input.GetKeyDown("fire3")) onKeyDown("Skill3");
            if (Input.GetKeyDown("fire4")) onKeyDown("Skill4");
        }
        public void onKeyDown(string keyName)
        {  
            switch (keyName)
            {
                case "Skill1":
                    chSystem.AttackUseSkill(1, false);
                    break;
                case "Skill2":
                    chSystem.AttackUseSkill(2, false);
                    break;
                case "BaseSkill":
                    chSystem.AttackUseSkill(3, false);
                    break;
            }  
           
           
        }

    }
}
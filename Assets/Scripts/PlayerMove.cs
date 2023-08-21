﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//목적1: W, A, S, D를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
//필요속성: 속도
//1-1: 사용자의 W, A, S, D입력을 받는다.
//1-2: 이동 방향을 설정한다.
//1-3: 이동속도에 따라 캐릭터를 이동시킨다.

//목적2: 스페이스를 누르면 수직으로 점프하고 싶다.
//필요속성: 캐릭터 컨트롤러 변수, 중력 변수, 수직 속력 변수, 점프파워, 점프 상태 변수
//2-1: 캐릭터 수직 속도에 중력을 적용하고 싶다.
//2-2: 캐릭터 컨트롤러로 나를 이동시키고 싶다.
//2-3: 스페이스 키를 누르면 수직속도에 점프파워를 적용하고 싶다.
//목적3: 점프 중인지 확인하고, 점프 중이면 점프 전 상태로 초기화 하고 싶다.
public class PlayerMove : MonoBehaviour
{
    //필요속성: 속도
    public float speed = 10f;

    //필요속성: 캐릭터 컨트롤러 변수, 중력 변수, 수직 속력 변수, 점프파워
    CharacterController characterController;
    float gravity = -20f;
    public float yVelocity = 0;
    public float jumpPower = 10f;
    public bool isJumping=false;
    private void Start()
    {
        characterController =GetComponent<CharacterController>();
    }
    // Update is called once per frame   
    void Update()
    {
        //목적1: W, A, S, D를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
        //1-1: 사용자의 W, A, S, D입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //만약 점프 중이였다면 점프 전 상태로 초기화 하고 싶다.
        if (isJumping == true && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }
        if (Input.GetButtonDown("Jump") && !isJumping)
        {            
            yVelocity = jumpPower;
            isJumping = true;

        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //}

        //1-2: 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0,v);
        dir = Camera.main.transform.TransformDirection(dir);

        //2-1: 캐릭터 수직 속도에 중력을 적용하고 싶다.
        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;
        //1-3: 이동속도에 따라 캐릭터를 이동시킨다.
        //transform.position += dir * speed * Time.deltaTime;
        //2-2: 캐릭터 컨트롤러로 나를 이동시키고 싶다.
        characterController.Move(dir * speed * Time.deltaTime);
    }
}

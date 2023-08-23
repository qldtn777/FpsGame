﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//목적1: W, A, S, D를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
//필요속성1: 속도
//1-1: 사용자의 W, A, S, D입력을 받는다.
//1-2: 이동 방향을 설정한다.
//1-3: 이동속도에 따라 캐릭터를 이동시킨다.

//목적2: 스페이스를 누르면 수직으로 점프하고 싶다.
//필요속성2: 캐릭터 컨트롤러 변수, 중력 변수, 수직 속력 변수, 점프파워, 점프 상태 변수
//2-1: 캐릭터 수직 속도에 중력을 적용하고 싶다.
//2-2: 캐릭터 컨트롤러로 나를 이동시키고 싶다.
//2-3: 스페이스 키를 누르면 수직속도에 점프파워를 적용하고 싶다


//목적3: 플레이어가 피격을 당하면 hp를 damage만큼 깍는다.
//필요속성3: hp

//목적4: 현재 플레이어의 Hp(%)를 슬라이더에 적용한다.
//필요속성4: hp(이미있음), maxHp, Slider


//목적5: 적의 공격을 받았을 때 HitImage를 켰다가 꺼준다.
//필요속성5: hitImage 게임오브젝트

//목적6: 플레이어가 죽으면 hitImage의 알파값을 현재 값에서 255로 만들어준다.
//필요속성6: 현재시간, hitImage 종료시간


//목적7: Ready상태일 때는 플레이어,적이 움직일 수 없도록 한다.
public class PlayerMove : MonoBehaviour
{
    //필요속성1: 속도
    public float speed = 10f;

    //필요속성2: 캐릭터 컨트롤러 변수, 중력 변수, 수직 속력 변수, 점프파워
    CharacterController characterController;
    float gravity = -20f;
    public float yVelocity = 0;
    public float jumpPower = 10f;
    public bool isJumping=false;


    //필요속성3: hp
    public int hp=10;


    //필요속성4: hp(이미있음), maxHp, Slider
    int maxHp = 10;
    public  Slider hpSlider ;


    //필요속성5: hitImage 게임오브젝트
    public GameObject hitImage;



    //필요속성6: 현재시간, hitImage 종료시간
    float currentTime;
    public float hitImageEndTime;

    private void Start()
    {
        characterController =GetComponent<CharacterController>();
       
        maxHp = hp;
    }
    // Update is called once per frame   
    void Update()
    {

        //목적4: 현재 플레이어의 Hp(%)를 슬라이더에 적용한다.
        hpSlider.value = (float)hp / maxHp;

        //목적7: Ready상태일 때는 플레이어,적이 움직일 수 없도록 한다.
        if (GameManager.Instance.state != GameManager.GameState.Start)
        {
            return;
        }

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
    //목적3: 플레이어가 피격을 당하면 hp를 damage만큼 깍는다.
    public void DamageAction(int damage)
    {
        hp -= damage;

        //목적5: 적의 공격을 받았을 때 HitImage를 켰다가 꺼준다.
        if (hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
        //목적6: 플레이어가 죽으면 hitImage의 알파값을 현재 값에서 255로 만들어준다.
        else
        {
            StartCoroutine(DeadEffect());
        }
    }

    

    //목적5: 적의 공격을 받았을 때 HitImage를 켰다가 꺼준다.
    IEnumerator PlayHitEffect()
    {
        hitImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        hitImage.gameObject.SetActive(false);
    }


    //목적6: 플레이어가 죽으면 hitImage의 알파값을 현재 값에서 255로 만들어준다.
    IEnumerator DeadEffect()
    {
        hitImage.gameObject.SetActive(true);
        Color hitImageColor = hitImage.GetComponent<Image>().color;


        while (true)
        {
            currentTime += Time.deltaTime;

            yield return null;

            hitImageColor.a = Mathf.Lerp(0.2f, 1, currentTime / hitImageEndTime);
            //그릇을 받고 적용을 다시 해줘야한다.
            hitImage.GetComponent<Image>().color = hitImageColor;
            
            //yield return null;

            if (currentTime > hitImageEndTime)
            {
                currentTime = 0;
                 break;
            }

        }
        

    }

}

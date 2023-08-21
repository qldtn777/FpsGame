using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//목적1: 적을 FSM 다이어그램에 따라 동작시킨다.
//필요속성1: 에너미 상태


//목적2: 플레이어와의 거리를 측정해서 특정 상태로 만들어 준다.
//필요속성2: 플레이어와의 거리, 플레이어 트랜스폼

//목적3: 적의 상태가 Move일 때, 플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
//필요속성3: 이동속도, character controller,공격범위
public class EnemyFSM : MonoBehaviour
{

    //필요속성1: 에너미 상태
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    //필요속성2: 플레이어와의 거리, 플레이어 트랜스폼
    public float findDistance =10.0f;
    Transform player;


    EnemyState enemyState;

    //필요속성3: 이동속도, character controller,공격범위
    public float moveSpeed = 5.0f;
    CharacterController characterController;
    public float attackDistance = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //목표1: 적을 FSM 다이어그램에 따라 동작시킨다.
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void Damaged()
    {
        throw new NotImplementedException();
    }

    private void Return()
    {
        throw new NotImplementedException();
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    
    private void Move()
    {
        //목적3: 적의 상태가 Move일 때, 플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;


            characterController.Move(dir*moveSpeed*Time.deltaTime);
        }
        else
        {
            enemyState = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
        }
    }

    //목적2: 플레이어와의 거리를 측정해서 특정 상태로 만들어 준다.
    private void Idle()
    {
        float distanceToPlyer = (player.position - transform.position).magnitude;
        //float tempDist = Vector3.Distance(transform.position, player.position);

        //더 가까우면 상태를 Move로 바꾸어 준다.
        if(distanceToPlyer <findDistance)
        {
            enemyState = EnemyState.Move;
            print("상태 전환: Idle -> Move");
        }
    }
}

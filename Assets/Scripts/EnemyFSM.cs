using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//목적1: 적을 FSM 다이어그램에 따라 동작시킨다.
//필요속성1: 에너미 상태


//목적2: 플레이어와의 거리를 측정해서 특정 상태로 만들어 준다.
//필요속성2: 플레이어와의 거리, 플레이어 트랜스폼


//목적3: 적의 상태가 Move일 때, 플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
//필요속성3: 이동속도, character controller,공격범위


//목표4: 플레이어가 공격범위 내에 들어오면 특정 시간에 한번씩 attackPower의 힘으로 공격한다.
//필요속성4: 현재시간, 공격딜레이,적의 공격 데미지


//목표5: 플레이어를 따라가다가 초기 위치에서 일정거리를 벗어나면 초기위치로 돌아온다.
//필요속성5: 일정거리(이동 가능 범위), 초기위치


//목표6: 초기 위치로 돌아간다. 특정 거리 이내면, Idle 상태로 돌아간다.
//필요속성6: 특정거리

//목표7: 플레이어의 공격을 받으면 플레이어의 hitDamage만큼 hp를 감소시킨다.
//필요속성7: hp

//목적8: 현재 에너미의 Hp(%)를 슬라이더에 적용한다.
//필요속성8: hp(이미있음), maxHp, Slider
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
    EnemyState enemyState;


    //필요속성2: 플레이어와의 거리, 플레이어 트랜스폼
    public float findDistance =10.0f;
    Transform player;
    

    //필요속성3: 이동속도, character controller,공격범위
    public float moveSpeed = 5.0f;
    CharacterController characterController;
    public float attackDistance = 3.0f;


    //필요속성4: 현재시간, 공격딜레이, 적의 공격 데미지
    public float currentTime = 0f;
    public float attackDelay = 2.0f;
    public int attackPower =3;


    //필요속성5: 일정거리(이동 가능 범위), 초기위치
    Vector3 originPos;
    public float moveDistance = 20.0f;


    //필요속성6: 특정거리
    float returnDistance = 0.1f;


    //필요속성7: hp
    public int hp = 3;

    //필요속성8: hp(이미있음), maxHp, Slider
    int maxHp= 3;
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        characterController = GetComponent<CharacterController>();

        originPos = transform.position;

        maxHp = hp;
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
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        hpSlider.value = (float)hp / maxHp;
    }


    private void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }


    //2초 후에 내 자신 파괴
    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2);

        print("사망");
        Destroy(gameObject);
    }


    //목표7: 플레이어의 공격을 받으면 플레이어의 hitDamage만큼 hp를 감소시킨다.
    //목표8: 에너미의 체력이 0보다 크면 피격 상태로 전환
    //목표9: 그렇지 않으면 죽음 상태로 전환
    public void DamageAction(int damage)
    {
        //만약, 이미 에너미가 피격됬거나, 사망 상태라면 데미지를 주지 않는다.
        if(enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
        {
            return;
        }

        //플레이어의 공격력 만큼 hp를 감소
        hp -= damage;

        //목표8: 에너미의 체력이 0보다 크면 피격 상태로 전환
        if(hp>0)
        {
            enemyState = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            Damaged();
        }

        //목표9: 그렇지 않으면 죽음 상태로 전환
        else
        {
            enemyState = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }
    }

    private void Damaged()
    {
        //피격 모션 0.5

        //피격 상태 처리를 위한 코루틴 실행
       StartCoroutine(DamageProcess());
    }
    //데미지 처리용
    IEnumerator DamageProcess()
    {
        //피격모션 시간만큼 기다린다.
        yield return new WaitForSeconds(0.5f);

        //이동 상태로 전환
        enemyState = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }
    private void Return()
    {
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        print(distanceToOriginPos);
        if(distanceToOriginPos > returnDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir * Time.deltaTime * moveSpeed);
            
        }
        else 
        {
            enemyState = EnemyState.Idle;   
            print("상태 변환:Return -> Idle");
        }
    }

    private void Attack()
    {
        //목표4: 플레이어가 공격범위 내에 들어오면 공격한다.

        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                if (player.GetComponent<PlayerMove>().hp < 0)
                {
                    enemyState = EnemyState.Idle;
                    print("상태 변환:Return -> Idle");
                    return;
                }

                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격!");
                currentTime = 0f;
            }
        }
        else
        {
            //그렇지 않으면 Move로 상태를 전환한다.
            enemyState = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0f;
        }
    }

    
    private void Move()
    {
        //목적3: 적의 상태가 Move일 때, 플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //목표5: 플레이어를 따라가다가 초기 위치에서 일정거리를 벗어나면 초기위치로 돌아온다.
        float distanceToOriginPos = (originPos - transform.position).magnitude;

        if (distanceToOriginPos>moveDistance)
        {
            enemyState = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        else if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;


            characterController.Move(dir*moveSpeed*Time.deltaTime);
        }
        else
        {
            enemyState = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
            currentTime = attackDelay;
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

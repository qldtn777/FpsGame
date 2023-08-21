using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//����1: ���� FSM ���̾�׷��� ���� ���۽�Ų��.
//�ʿ�Ӽ�1: ���ʹ� ����


//����2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ����� �ش�.
//�ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������

//����3: ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
//�ʿ�Ӽ�3: �̵��ӵ�, character controller,���ݹ���
public class EnemyFSM : MonoBehaviour
{

    //�ʿ�Ӽ�1: ���ʹ� ����
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    //�ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������
    public float findDistance =10.0f;
    Transform player;


    EnemyState enemyState;

    //�ʿ�Ӽ�3: �̵��ӵ�, character controller,���ݹ���
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
        //��ǥ1: ���� FSM ���̾�׷��� ���� ���۽�Ų��.
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
        //����3: ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;


            characterController.Move(dir*moveSpeed*Time.deltaTime);
        }
        else
        {
            enemyState = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");
        }
    }

    //����2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ����� �ش�.
    private void Idle()
    {
        float distanceToPlyer = (player.position - transform.position).magnitude;
        //float tempDist = Vector3.Distance(transform.position, player.position);

        //�� ������ ���¸� Move�� �ٲپ� �ش�.
        if(distanceToPlyer <findDistance)
        {
            enemyState = EnemyState.Move;
            print("���� ��ȯ: Idle -> Move");
        }
    }
}

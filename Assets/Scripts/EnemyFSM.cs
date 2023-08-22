using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//����1: ���� FSM ���̾�׷��� ���� ���۽�Ų��.
//�ʿ�Ӽ�1: ���ʹ� ����


//����2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ����� �ش�.
//�ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������


//����3: ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
//�ʿ�Ӽ�3: �̵��ӵ�, character controller,���ݹ���


//��ǥ4: �÷��̾ ���ݹ��� ���� ������ Ư�� �ð��� �ѹ��� attackPower�� ������ �����Ѵ�.
//�ʿ�Ӽ�4: ����ð�, ���ݵ�����,���� ���� ������


//��ǥ5: �÷��̾ ���󰡴ٰ� �ʱ� ��ġ���� �����Ÿ��� ����� �ʱ���ġ�� ���ƿ´�.
//�ʿ�Ӽ�5: �����Ÿ�(�̵� ���� ����), �ʱ���ġ


//��ǥ6: �ʱ� ��ġ�� ���ư���. Ư�� �Ÿ� �̳���, Idle ���·� ���ư���.
//�ʿ�Ӽ�6: Ư���Ÿ�

//��ǥ7: �÷��̾��� ������ ������ �÷��̾��� hitDamage��ŭ hp�� ���ҽ�Ų��.
//�ʿ�Ӽ�7: hp

//����8: ���� ���ʹ��� Hp(%)�� �����̴��� �����Ѵ�.
//�ʿ�Ӽ�8: hp(�̹�����), maxHp, Slider
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
    EnemyState enemyState;


    //�ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������
    public float findDistance =10.0f;
    Transform player;
    

    //�ʿ�Ӽ�3: �̵��ӵ�, character controller,���ݹ���
    public float moveSpeed = 5.0f;
    CharacterController characterController;
    public float attackDistance = 3.0f;


    //�ʿ�Ӽ�4: ����ð�, ���ݵ�����, ���� ���� ������
    public float currentTime = 0f;
    public float attackDelay = 2.0f;
    public int attackPower =3;


    //�ʿ�Ӽ�5: �����Ÿ�(�̵� ���� ����), �ʱ���ġ
    Vector3 originPos;
    public float moveDistance = 20.0f;


    //�ʿ�Ӽ�6: Ư���Ÿ�
    float returnDistance = 0.1f;


    //�ʿ�Ӽ�7: hp
    public int hp = 3;

    //�ʿ�Ӽ�8: hp(�̹�����), maxHp, Slider
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


    //2�� �Ŀ� �� �ڽ� �ı�
    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2);

        print("���");
        Destroy(gameObject);
    }


    //��ǥ7: �÷��̾��� ������ ������ �÷��̾��� hitDamage��ŭ hp�� ���ҽ�Ų��.
    //��ǥ8: ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
    //��ǥ9: �׷��� ������ ���� ���·� ��ȯ
    public void DamageAction(int damage)
    {
        //����, �̹� ���ʹ̰� �ǰ݉�ų�, ��� ���¶�� �������� ���� �ʴ´�.
        if(enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
        {
            return;
        }

        //�÷��̾��� ���ݷ� ��ŭ hp�� ����
        hp -= damage;

        //��ǥ8: ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
        if(hp>0)
        {
            enemyState = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");
            Damaged();
        }

        //��ǥ9: �׷��� ������ ���� ���·� ��ȯ
        else
        {
            enemyState = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");
            Die();
        }
    }

    private void Damaged()
    {
        //�ǰ� ��� 0.5

        //�ǰ� ���� ó���� ���� �ڷ�ƾ ����
       StartCoroutine(DamageProcess());
    }
    //������ ó����
    IEnumerator DamageProcess()
    {
        //�ǰݸ�� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        //�̵� ���·� ��ȯ
        enemyState = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
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
            print("���� ��ȯ:Return -> Idle");
        }
    }

    private void Attack()
    {
        //��ǥ4: �÷��̾ ���ݹ��� ���� ������ �����Ѵ�.

        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                if (player.GetComponent<PlayerMove>().hp < 0)
                {
                    enemyState = EnemyState.Idle;
                    print("���� ��ȯ:Return -> Idle");
                    return;
                }

                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����!");
                currentTime = 0f;
            }
        }
        else
        {
            //�׷��� ������ Move�� ���¸� ��ȯ�Ѵ�.
            enemyState = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
            currentTime = 0f;
        }
    }

    
    private void Move()
    {
        //����3: ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //��ǥ5: �÷��̾ ���󰡴ٰ� �ʱ� ��ġ���� �����Ÿ��� ����� �ʱ���ġ�� ���ƿ´�.
        float distanceToOriginPos = (originPos - transform.position).magnitude;

        if (distanceToOriginPos>moveDistance)
        {
            enemyState = EnemyState.Return;
            print("���� ��ȯ: Move -> Return");
        }
        else if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;


            characterController.Move(dir*moveSpeed*Time.deltaTime);
        }
        else
        {
            enemyState = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");
            currentTime = attackDelay;
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

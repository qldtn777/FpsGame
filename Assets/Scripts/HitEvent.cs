using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����1: �÷��̾�� �������� ������.


public class HitEvent : MonoBehaviour
{
    public EnemyFSM eFsm;

    public void HitPlayer()
    {
        eFsm.AttackAction();
    }
}

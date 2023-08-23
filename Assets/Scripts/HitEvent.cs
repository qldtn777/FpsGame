using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목적1: 플레이어에게 데미지를 입힌다.


public class HitEvent : MonoBehaviour
{
    public EnemyFSM eFsm;

    public void HitPlayer()
    {
        eFsm.AttackAction();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목적: 폭탄이 물체에 부딪히면 파되된다.
//필요속성: 폭발 이펙트
public class BombAction : MonoBehaviour
{
    //필요속성: 폭발 이펙트
    public GameObject bombEffect;
    private void OnCollisionEnter(Collision collision)
    {
        //이펙트를 만든다.
        GameObject bombEffGO = Instantiate(bombEffect);
        //이펙트의 위치 설정한다.
        bombEffGO.transform.position = transform.position;
        Destroy(gameObject);
    }
}

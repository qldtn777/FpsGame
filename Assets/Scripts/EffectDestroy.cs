using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목적: 내(이펙트)가 특정 시간이 지나면 제거된다.
//필요속성: 현재시간, 삭제 임계 시간
public class EffectDestroy : MonoBehaviour
{
    //필요속성: 현재시간, 삭제 임계 시간
    public float destroyTime = 1;
    float currentTime = 0;

    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//����1: HP bar�� �չ����� Ÿ���� �� �������� ���Ѵ�.
//�ʿ�Ӽ�1: Ÿ��

public class HpBarTarget : MonoBehaviour
{
    //�ʿ�Ӽ�1: Ÿ��
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = target.forward;
    }
}

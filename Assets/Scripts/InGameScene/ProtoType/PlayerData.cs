using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public int MAXHP = 100;//Player�̍ő�HP
    [HideInInspector] public int ATK_Near = 30;//�ߋ����U��
    [HideInInspector] public int ATK_Far = 10;//�������U��
    [HideInInspector] public float Moov_Speed = 5.0f;       // �ړ����x
    [HideInInspector] public float Cool_Time = 0.5f;  // ���̍U���܂ł̎���

    [SerializeField] public int _maxhp = 100;
    [SerializeField] public int _atk_near = 30;
    [SerializeField] public int _atk_far = 10;
    [SerializeField] public float _moov_speed = 5.0f;
    [SerializeField] public float _cool_time = 0.5f;


    public void Initialize()
    {
        MAXHP = _maxhp;
        ATK_Near = _atk_near;
        ATK_Far = _atk_far;
        Moov_Speed = _moov_speed;
        Cool_Time = _cool_time;
    }
}

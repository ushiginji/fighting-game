using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeyState : MonoBehaviour
{
    private int _currentHp;
    [SerializeField]
    private PlayerData _playerData;
    [SerializeField]
    private bool Down = false;
    // Start is called before the first frame update
    void Start()
    {
        if (_playerData != null)
        {
            _currentHp = _playerData.MAXHP;
        }
        else
        {
            _currentHp = 0;
            Debug.LogError("PlayerData is null !");
        }
    }

    public void Damage(Collider collider)
    {
        int damage = 10;

        _currentHp -= damage;
        if (_currentHp < 0)
        {
            _currentHp = 0;
            PlayerDown();
            Debug.Log("ƒ_ƒEƒ“’†");
        }
    }

    public void PlayerDown()
    {
        Invoke("Heal", 5.0f);
        Debug.Log("•œ‹A");
    }

    public void Heal()
    {
        _currentHp = _playerData.MAXHP;
    }
}

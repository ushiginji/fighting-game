// yu-ki-rohi
// https://coacoa.net/unity_explain/unity_objectpool2/

using Photon.Pun;
using UnityEngine;

public class ObjectManager : MonoBehaviourPunCallbacks
{
    const int MAX_PLAYER_NUM = 4;
    const int MAX_BULLET_NUM = 50;

    [SerializeField] private AvatarController[] _players = new AvatarController[MAX_PLAYER_NUM];

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject[] _bullets = new GameObject[MAX_BULLET_NUM];

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < MAX_PLAYER_NUM; i++)
        {
            _players[i] = null;
        }

        for (int i = 0; i < MAX_BULLET_NUM; i++)
        {
            if (_bullets[i] == null)
            {
                _bullets[i] = Instantiate(_bullet);
                _bullets[i].GetComponent<AvatarAttack>().SetObjectManager(this);
                _bullets[i].SetActive(false);
            }
        }
    }

    // PlayerŠÖŒW
    public void AddPlayer(AvatarController player)
    {
        for (int i = 0; i < MAX_PLAYER_NUM; i++)
        {
            if(_players[i] == null)
            {
                _players[i] = player;
                return;
            }
        }
    }

    public AvatarController GetPlayer(int id_)
    {
        if(id_ < 0 ||  id_ >= MAX_PLAYER_NUM)
        {
            return null;
        }
        return _players[id_];
    }

    public int GetPlayerNum()
    {
        int num = 0;
        foreach(var player in _players)
        {
            if(player != null && player.IsActive)
            {
                num++;
            }
        }
        return num;
    }

    // BulletŠÖŒW
    public void Fire(Vector3 pos, Vector3 dir, int id, int ownerId)
    {
        for (int i = 0; i < MAX_BULLET_NUM; i++)
        {
            if (!_bullets[i].activeSelf)
            {
                _bullets[i].SetActive(true);
                _bullets[i].transform.position = pos;
                _bullets[i].transform.forward = dir;
                _bullets[i].GetComponent<AvatarAttack>().Init(id, ownerId);
                break;
            }
        }
        
    }

    public void Hit(int id, int ownerId)
    {
        for (int i = 0; i < MAX_BULLET_NUM; i++)
        {
            if (_bullets[i].activeSelf)
            {
                AvatarAttack avatarAttack = _bullets[i].GetComponent<AvatarAttack>();
                if(avatarAttack != null && avatarAttack.Equals(id,ownerId))
                {
                    _bullets[i].SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

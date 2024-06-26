using Cinemachine;
using Photon.Pun;
using System.Runtime.Serialization;
using UnityEngine;


// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarController : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum Type
    {
        InFight,
        OutRange
    }

    [SerializeField] private Type type = Type.InFight;
    [SerializeField] private Material[] materials;
    Renderer renderer;

    [SerializeField] private GameObject weapon;

    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float rotateSpeed = 3.0f;

    private bool isActive = true;
    public bool IsActive { get { return isActive; } }

   
   

    [SerializeField]  private float respawnTime = 6.0f;
    private float respawnTimer;

    private Rigidbody rigidbody;

    [SerializeField] private GameObject _bullet;
    private int nextBulletId = 0;

    [SerializeField] private ObjectManager _objectManager = null;

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material = materials[(int)type];
        rigidbody = gameObject.GetComponent<Rigidbody>();
        weapon.SetActive(false);
        respawnTimer = respawnTime;
        if (photonView.IsMine)
        {
            virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = gameObject.transform;
            virtualCamera.LookAt = gameObject.transform;
        }
        _objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();   
    }
    private void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            if(transform.position.y < -10.0f)
            {
                isActive = false;
                transform.position = new Vector3(0, 5, 0);
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
            }
            if (isActive)
            {
                var input = Vector3.zero;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    input += Vector3.forward;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    input += Vector3.back;
                }

                transform.Translate(6f * Time.deltaTime * input);

                if (Input.GetKeyDown(KeyCode.X))
                {
                    type = (Type)(((int)type + 1) % 2);
                    renderer.material = materials[(int)type];
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (type == Type.InFight)
                    {
#if false
                        if (!weapon.activeSelf)
                        {
                            weapon.SetActive(true);
                        }
#else
                        photonView.RPC(nameof(Attack), RpcTarget.All);
#endif
                    }
                    else
                    {
#if false
                        var bullet = PhotonNetwork.Instantiate("ProtoType/Bullet", transform.position, Quaternion.identity);
                        bullet.transform.rotation = transform.rotation;
                        bullet.GetComponent<AvatarAttack>().OwnerId = id;
#else
                        photonView.RPC(nameof(Fire), RpcTarget.All,nextBulletId++);
#endif
                    }
                }
            }
            else
            {
                respawnTimer -= Time.deltaTime;
                if (respawnTimer < 0)
                {
                    respawnTimer = respawnTime;
                    isActive = true;
                    rigidbody.useGravity = true;
                }
            }
            
            float add_rotate = 0.0f;
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                add_rotate += 1.0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                add_rotate -= 1.0f;
            }
            
            transform.Rotate(0, add_rotate * rotateSpeed * Time.deltaTime, 0);
            
        }
        else
        {
            renderer.material = materials[(int)type];
            rigidbody.useGravity = isActive;
            if (!isActive)
            {               
                rigidbody.velocity = Vector3.zero;
            }
        }
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(type);
            stream.SendNext(isActive);
            stream.SendNext(weapon.activeSelf);
        }
        else
        {
            type = (Type)stream.ReceiveNext();
            isActive = (bool)stream.ReceiveNext();
            weapon.SetActive((bool)stream.ReceiveNext());
        }
    }
   
    public void Init(ObjectManager objectManager_)
    {
        _objectManager = objectManager_;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {   
            if (other.TryGetComponent<AvatarAttack>(out var attack))
            {
                if (attack.OwnerId() == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    Vector3 knockBackVec = (transform.position - other.transform.position).normalized;
                    knockBackVec *= attack.Power();
                    rigidbody.AddForce(knockBackVec, ForceMode.Impulse);
                }
            }
        }
        if (other.tag == "Bullet")
        {
            if (!photonView.IsMine)
            {
                if (other.TryGetComponent<AvatarAttack>(out var bullet))
                {
                    if (bullet.OwnerId() == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        photonView.RPC(nameof(Damaged), RpcTarget.All, other.transform.position, bullet.Power());
                        photonView.RPC(nameof(HitBullet), RpcTarget.All, bullet.Id());
                    }
                }
            }
        }
    }

    [PunRPC]
    private void Attack()
    {
        if (!weapon.activeSelf)
        {
            weapon.SetActive(true);
        }
    }

    [PunRPC]
    private void Fire(int id_)
    {

#if false
        var bullet = Instantiate(_bullet, transform.position + transform.forward * 1.5f, Quaternion.identity);
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<AvatarAttack>().Init(id_, photonView.OwnerActorNr);
#else
        _objectManager.Fire(transform.position, transform.forward, id_, photonView.OwnerActorNr);
#endif
    }
    [PunRPC]
    private void Damaged(Vector3 pos,float power)
    {
        Vector3 knockBackVec = (transform.position - pos).normalized;
        knockBackVec *= power;
        rigidbody.AddForce(knockBackVec, ForceMode.Impulse);
    }
    [PunRPC]
    private void HitBullet(int id, PhotonMessageInfo info)
    {
        _objectManager.Hit(id, info.Sender.ActorNumber);
    }
}

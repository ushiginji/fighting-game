using Cinemachine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    private int id = -1;
    public int Id { get { return id; } }

    [SerializeField]  private float respawnTime = 6.0f;
    private float respawnTimer;

    private Rigidbody rigidbody;

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material = materials[(int)type];
        rigidbody = gameObject.GetComponent<Rigidbody>();
        weapon.SetActive(false);
        respawnTimer = respawnTime;
        id = photonView.OwnerActorNr;
        if (photonView.IsMine)
        {
            virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = gameObject.transform;
            virtualCamera.LookAt = gameObject.transform;
        }
            
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
                        if (!weapon.activeSelf)
                        {
                            weapon.SetActive(true);
                        }
                        //photonView.RPC(nameof(Attack), RpcTarget.All);
                    }
                    else
                    {
                        var bullet = PhotonNetwork.Instantiate("ProtoType/Bullet", transform.position, Quaternion.identity);
                        bullet.transform.rotation = transform.rotation;
                        bullet.GetComponent<AvatarAttack>().OwnerId = id;
                        //photonView.RPC(nameof(Fire), RpcTarget.All);
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
   

    [PunRPC]
    private void Attack()
    {
        if (!weapon.activeSelf)
        {
            weapon.SetActive(true);
        }
    }

    [PunRPC]
    private void Fire()
    {
        var bullet = PhotonNetwork.Instantiate("ProtoType/Bullet", transform.position + transform.forward * 1.5f, Quaternion.identity);
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<AvatarAttack>().OwnerId = id;
    }
}
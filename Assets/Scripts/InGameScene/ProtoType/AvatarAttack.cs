// yu-ki-rohi
// https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/2e3520

using Photon.Pun;
using UnityEngine;

public class AvatarAttack : MonoBehaviourPunCallbacks
{
    public enum Type
    {
        InFight,
        OutRange
    }

    [SerializeField] private Type type = Type.InFight;
    [SerializeField] private float rotateSpeed = 10.0f;
    private float distance = 0.8f;
    private float theta = 0.0f;

    [SerializeField] private float speed = 12.0f;
    [SerializeField] private const float time = 2.0f;
    private float timer = 0.0f;

    private int id = -1;
    private int ownerId = -1;


    [SerializeField] private ObjectManager objectManager = null;

    [SerializeField] private float power = 5.0f;

    public int Id() { return id; }
    public int OwnerId() { return ownerId; }
    public float Power() { return power; }

    public void SetObjectManager(ObjectManager objectManager_)
    {
        this.objectManager = objectManager_;
    }

    public void Init(int id_, int ownerId_)
    {
        id = id_;
        ownerId = ownerId_;
        timer = 0.0f;
    }

    public bool Equals(int id_, int ownerId_)
    {
        return id == id_ && ownerId == ownerId_;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(type == Type.InFight)
        {
            ownerId = PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            
        }

        if (type == Type.InFight)
        {
            if (gameObject.activeInHierarchy)
            {
                Vector3 new_pos = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
                transform.localPosition = distance * new_pos;

                theta += (rotateSpeed * Mathf.PI / 180.0f * Time.deltaTime);
                if (theta > Mathf.PI)
                {
                    theta = 0.0f;
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
            timer += Time.deltaTime;
            if (timer > time)
            {
                objectManager.Hit(id, ownerId);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
#if false
        if (other.tag == "Player")
        {
            if (other.gameObject.GetPhotonView().OwnerActorNr != photonView.OwnerActorNr)
            {
                Debug.Log("Hit!");
                Vector3 knockBackVec = (other.transform.position - transform.position).normalized;
                if (type == Type.InFight)
                {
                    knockBackVec *= 10.0f;
                }
                else
                {
                    knockBackVec *= 5.0f;
                }

                Rigidbody rigidbody = other.GetComponent<Rigidbody>();
                rigidbody.AddForce(knockBackVec, ForceMode.Impulse);

                if (photonView.IsMine)
                {
                    if (type == Type.OutRange)
                    {
                        Destroy(gameObject);
                    }
                }

            }


        }


        if (other.tag == "Stage")
        {
            if (type == Type.OutRange)
            {
                Destroy(gameObject);
            }
            if (photonView.IsMine)
            {

            }
        }
#else
        if (other.tag == "Stage")
        {
            objectManager.Hit(id, ownerId);
        }
       
#endif
    }

    [PunRPC]

    private void HitAttack(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetPhotonView().OwnerActorNr != ownerId)
            {
                Vector3 knockBackVec = (other.transform.position - transform.position).normalized;
                if (type == Type.InFight)
                {
                    knockBackVec *= 10.0f;
                }
                else
                {
                    knockBackVec *= 5.0f;
                }

                Rigidbody rigidbody = other.GetComponent<Rigidbody>();
                rigidbody.AddForce(knockBackVec, ForceMode.Impulse);

                if (type == Type.OutRange)
                {
                    objectManager.Hit(id, ownerId);
                }
                if (photonView.IsMine)
                {
                    
                }

            }


        }

        if (type == Type.OutRange)
        {
            Debug.Log("Hit2");
            objectManager.Hit(id, ownerId);
        }
        if (photonView.IsMine)
        {

        }
        if (other.tag == "Stage")
        {
           
        }
    }
}

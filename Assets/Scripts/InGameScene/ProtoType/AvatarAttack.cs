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
    [SerializeField] private float timer = 2.0f;

    private int ownerId = -1;
    public int OwnerId { set { ownerId = value; } }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            if (type == Type.InFight)
            {
                if(gameObject.activeInHierarchy)
                {
                    Vector3 new_pos = new Vector3(Mathf.Cos(theta),0,Mathf.Sin(theta));
                    transform.localPosition = distance * new_pos;
                    
                    theta += (rotateSpeed * Mathf.PI / 180.0f * Time.deltaTime);
                    if(theta > Mathf.PI)
                    {
                        theta = 0.0f;
                        gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.tag == "Player")
        {
            AvatarController avatarController = other.GetComponent<AvatarController>();
            if (avatarController != null && avatarController.Id != ownerId)
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

                if (type == Type.OutRange)
                {
                    Destroy(gameObject);
                }
            }

            
        }
           

        if (other.tag == "Stage" && type == Type.OutRange)
        {
            Destroy(gameObject);
        }
    }
}

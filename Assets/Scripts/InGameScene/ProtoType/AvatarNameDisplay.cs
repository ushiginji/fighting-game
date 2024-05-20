using Photon.Pun;
using TMPro;
using UnityEngine;

// https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/c04ad5
// MonoBehaviourPunCallbacks���p�����āAphotonView�v���p�e�B���g����悤�ɂ���
public class AvatarNameDisplay : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI textMeshPro; 
    private RectTransform rectTransform;
    private AvatarController avatarController;
    private void Start()
    {
        avatarController = gameObject.GetComponentInParent<AvatarController>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        float pos_y = 40 * (photonView.OwnerActorNr - 1);

        // �v���C���[���ƃv���C���[ID��\������
        rectTransform.position = new Vector3(100, 1000 - pos_y, 0);
        textMeshPro.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
    }
    private void Update()
    {
        if(avatarController != null)
        {
            if (avatarController.IsActive)
            {
                textMeshPro.color = Color.black;
            }
            else
            {
                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0.5f);
            }
        }
    }
}

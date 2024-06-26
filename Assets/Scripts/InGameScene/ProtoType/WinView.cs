// yu-ki-rohi

using Photon.Pun;
using TMPro;
using UnityEngine;

public class WinView : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI _textMeshPro;
    private AvatarController _avatarController;
    // Start is called before the first frame update
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _avatarController = GetComponentInParent<AvatarController>();
        _textMeshPro.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr}) WIN !!";
        _textMeshPro.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_avatarController != null)
        {
            if(_avatarController.YouWin)
            {
                _textMeshPro.enabled = true;
            }
            else
            {
                _textMeshPro.enabled = false;
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectedPlayerUiSetter : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI lbl_name;
    [SerializeField] TMPro.TextMeshProUGUI lbl_id;
    [SerializeField] TMPro.TextMeshProUGUI lbl_state;

    public string PlayerName { set => lbl_name.text = value; }
    public string PlayerId { set => lbl_id.text = value; }
    public string PlayerState { set => lbl_state.text = value; }
}

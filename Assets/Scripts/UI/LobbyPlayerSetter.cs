using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerSetter : MonoBehaviour
{

    [SerializeField] TMPro.TMP_Text lbl_name;
    [SerializeField] Image role;
    [SerializeField] Image img_ready;
    [SerializeField] Sprite driver;
    [SerializeField] Sprite shooter;

    private void Awake()
    {
        img_ready.color = Color.red;
    }

    public string Name { get => lbl_name.text; set => lbl_name.text = value; }
    public bool Ready { get => img_ready.color == Color.green; set => img_ready.color = value ? Color.green : Color.red; }
    public bool Role { get => role.sprite == driver; set => role.sprite = value ? driver : shooter; }
}

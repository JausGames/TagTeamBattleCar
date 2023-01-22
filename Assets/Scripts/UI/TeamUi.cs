using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;

public class TeamUi : MonoBehaviour
{
    [SerializeField] List<TMPro.TextMeshProUGUI> lbl_list_teammate;
    [SerializeField] TMPro.TMP_InputField input_mate_name;
    [SerializeField] Button btn_add_mate;
    [NonSerialized] public UnityEvent addMateEvent = new UnityEvent();

    public string MateName { get => input_mate_name.text; }

    private void Awake()
    {
        btn_add_mate.onClick.AddListener(TryAddMate);
    }

    public bool AddTeammateUi(string name)
    {
        for(int i = 0; i < lbl_list_teammate.Count; i++)
        {
            if(lbl_list_teammate[i].text == "")
            {
                lbl_list_teammate[i].text = name;
                return true;
            }
        }
        return false;
    }
    public void TryAddMate()
    {
        if (input_mate_name.text == "") return;
        addMateEvent.Invoke();
        input_mate_name.text = "";
    }
}

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
    [SerializeField] Button btn_leave_team;
    [NonSerialized] public UnityEvent addMateEvent = new UnityEvent();
    [NonSerialized] public UnityEvent leaveTeam = new UnityEvent();

    public string MateName { get => input_mate_name.text; }

    private void Awake()
    {
        btn_add_mate.onClick.AddListener(TryAddMate);
        btn_leave_team.onClick.AddListener(leaveTeam.Invoke);
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

    internal void RemoveTeammateUi(string mateName)
    {
        Debug.Log("TeamUi, RemoveTeammateUi : mateName = " + mateName);
        List<string> nameList = new List<string>();
        var corres = 0;
        for (int i = 0; i < lbl_list_teammate.Count; i++)
        {
            nameList.Add(mateName);
            if (lbl_list_teammate[i].text == mateName)
                corres = i;
        }
        lbl_list_teammate[corres].text = "";
        for (int i = corres + 1; i < lbl_list_teammate.Count; i++)
        {
            lbl_list_teammate[i - 1].text = lbl_list_teammate[i].text;
            lbl_list_teammate[i].text = "";
        }


    }
    internal void RemoveAllTeammateUi()
    {
        lbl_list_teammate[1].text = "";
        lbl_list_teammate[2].text = "";
    }
}

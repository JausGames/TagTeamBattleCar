using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingUi : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI lbl_queue_status;
    [SerializeField] Button btn_join_quit;
    [SerializeField] TMPro.TextMeshProUGUI btn_label;

    public string QueueStatus { get => lbl_queue_status.text; set => lbl_queue_status.text = value; }
    public string ButtonLabel { get => btn_label.text; set => btn_label.text = value; }
    public Button JoinQuitButton { get => btn_join_quit; set => btn_join_quit = value; }
}

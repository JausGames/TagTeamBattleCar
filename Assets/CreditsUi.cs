using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUi : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI lbl_amount;
    public int Amount { get => int.Parse(lbl_amount.text); set => lbl_amount.text = value.ToString(); }
}

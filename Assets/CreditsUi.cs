using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUi : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI lbl_amount;
    [SerializeField] TMPro.TextMeshProUGUI lbl_earns;
    [SerializeField] Animation earnAnimation;
    public int Amount 
    { 
        get => int.Parse(lbl_amount.text);
        set 
        {
            var previous = int.Parse(lbl_amount.text);
            var currentDelta = lbl_earns.text.Length > 0 ? int.Parse(lbl_earns.text.Substring(1)) : 0;
            lbl_amount.text = value.ToString();
            var diff = currentDelta + (value - previous);
            if ((value - previous) != 0)
            {
                Debug.Log("CreditsUi, Amount : diff = " + diff);
                lbl_earns.text = (diff > 0 ? "+" : "-") + diff.ToString();
                lbl_earns.color = diff > 0 ? Color.green : Color.red;
                if(!earnAnimation.isPlaying) earnAnimation.Play();
                else earnAnimation.Rewind();
            }
        }
    }
}

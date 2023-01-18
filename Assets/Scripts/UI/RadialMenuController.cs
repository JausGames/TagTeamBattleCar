using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
{
    [SerializeField] GameObject theMenu;

    public List<ItemHolder> items;
    List<ItemData> itemData = new List<ItemData>();

    [SerializeField] private ItemData pickedItem;
    [SerializeField] GameObject divider;

    [SerializeField] RectTransform SelectedSegments;
    [SerializeField] RectTransform Mask2;

    float angle;

    public ItemData PickedItem { get => pickedItem; set => pickedItem = value; }

    // Start is called before the first frame update
    void Start()
    {
        //var angleIncrement = 180f / items.Count;
        var angleIncrement = (2f * Mathf.PI) / items.Count;
        var angleIncrementDegree = 360f / items.Count;
        var diameter = Screen.height / 3.5f;

        Mask2.rotation = Quaternion.Euler(0f,0f, -90f + angleIncrementDegree);

        for (int i = 0; i < items.Count; i++)
        {
            var div = Instantiate(divider, theMenu.transform.position, Quaternion.Euler(0f, 0f, (i * angleIncrementDegree + 90f) - angleIncrementDegree * .5f), theMenu.transform);
            div.layer = theMenu.layer;

            var containerObj = new GameObject(items[i].name + "_container");
            containerObj.transform.parent = theMenu.transform;
            containerObj.transform.localPosition = Vector3.zero;

            var goName = new GameObject(items[i].name + "_name", typeof(TMPro.TextMeshProUGUI));
            goName.transform.parent = containerObj.transform;
            goName.layer = theMenu.layer;

            var lblName = goName.GetComponent<TMPro.TextMeshProUGUI>();
            lblName.text = items[i].Name;
            lblName.alignment = TMPro.TextAlignmentOptions.Center;
            lblName.color = new Color(lblName.color.r, lblName.color.g, lblName.color.b, .8f);

            var rectName = lblName.GetComponent<RectTransform>();
            rectName.localPosition = (Mathf.Cos(angleIncrement * i) * Vector3.right + Mathf.Sin(angleIncrement * i) * Vector3.up) * diameter;


            var goCost = new GameObject(items[i].name + "_cost", typeof(TMPro.TextMeshProUGUI));
            goCost.transform.parent = containerObj.transform;
            goCost.layer = theMenu.layer;

            var lblCost = goCost.GetComponent<TMPro.TextMeshProUGUI>();
            lblCost.text = items[i].Cost.ToString() + "$";
            lblCost.fontSize = lblName.fontSize * .75f;
            lblCost.color = new Color(214f / 255f, 191f / 255f, 136f / 255f, .8f);
            lblCost.alignment = TMPro.TextAlignmentOptions.Center;

            var rectCost = lblCost.GetComponent<RectTransform>();
            rectCost.localPosition = (Mathf.Cos(angleIncrement * i) * Vector3.right + Mathf.Sin(angleIncrement * i) * Vector3.up) * diameter - Vector3.up * 35f;

            itemData.Add(new ItemData(items[i], i * angleIncrementDegree));
        }
    }

    // Update is called once per frame
    void Update()
    {
        var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var angleIncrementDegree = 360f / items.Count;
        angle = Vector2.SignedAngle(Vector2.right, Mouse.current.position.ReadValue() - screenCenter);
        angle = angle < 0 ? angle + 360f : angle;

        var id = 0;
        while(itemData[id].Angle + angleIncrementDegree * .5f < angle)
        {
            id++;
            if (angle > (itemData.Count - 1) * angleIncrementDegree + angleIncrementDegree * .5f) { id = 0; break; }
            else if (id == itemData.Count - 1) break;
        }
        pickedItem = itemData[id];

        SelectedSegments.rotation = Quaternion.Euler(0f, 0f, itemData[id].Angle - angleIncrementDegree * .5f);
    }

}

[Serializable]
public class ItemData
{
    [SerializeField] string name;
    int cost;
    float angle;
    GameObject prefab;

    public float Angle { get => angle; set => angle = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Cost { get => cost; set => cost = value; }

    public ItemData(string name, int cost, float angle, GameObject prefab)
    {
        this.name = name;
        this.cost = cost;
        this.angle = angle;
        this.prefab = prefab;
    }
    public ItemData(ItemHolder holder, float angle)
    {
        this.name = holder.Name;
        this.cost = holder.Cost;
        this.angle = angle;
        this.prefab = holder.Prefab;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField]public Transform seatTransform;
    [SerializeField]public NetworkAnimator animator;
    [SerializeField]public Transform target;
    [SerializeField] private Transform bulletStart;
    [SerializeField] public Transform rightHand;
    [SerializeField]public Rigidbody body;
    [SerializeField]public ShooterAnimatorController controller;

    private GameObject item;

    public GameObject Item { 
        get => item;
        set
        {
            if (item != null) Destroy(item);
            item = Instantiate(value, rightHand);
            if (item.GetComponent<Weapon>()) bulletStart = item.GetComponent<Weapon>().canonEnd;
            else bulletStart = null;
            item.GetComponent<Item>().Id = value.GetComponent<Item>().Id;
            item.GetComponent<Item>().Prefab = value.GetComponent<Item>().Prefab;
        }
    }

    public Transform BulletStart { get => bulletStart; set => bulletStart = value; }
}

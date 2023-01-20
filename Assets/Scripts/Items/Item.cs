using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : MonoBehaviour
{
    GameObject prefab;
    int id;
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Id { get => id; set => id = value; }

    public abstract bool Use(ShooterController owner);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemHolder : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] new string name;
    [SerializeField] int cost;
    [SerializeField] int id;

    public string Name { get => name; set => name = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Cost { get => cost; set => cost = value; }
    public int Id { get => id; set => id = value; }
}

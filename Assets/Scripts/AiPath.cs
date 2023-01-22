using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPath : MonoBehaviour
{
    [SerializeField]
    List<Transform> checkpoints = new List<Transform>();

    public List<Transform> Checkpoints { get => checkpoints; set => checkpoints = value; }
    public List<Vector3[]> Segments 
    {
        get
        {
            var value = new List<Vector3[]>();
            value.Add(new Vector3[] { checkpoints[checkpoints.Count - 1].position, checkpoints[0].position });
            for (int i = 0; i < checkpoints.Count - 1; i++)
            {
                value.Add(new Vector3[] { checkpoints[i].position, checkpoints[i + 1].position });
            }
            return value;
        }
    }
}

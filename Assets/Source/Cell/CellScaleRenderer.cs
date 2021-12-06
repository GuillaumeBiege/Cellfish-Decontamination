using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScaleRenderer : MonoBehaviour
{
    public GameObject Sphere = null;

    public float period = 1f;
    public float amplitude = 5f;
    public float zorigine = 1f;

    float headStart = 0f;

    // Start is called before the first frame update
    void Start()
    {
        headStart = (float)Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        Sphere.transform.localScale = ((Mathf.Sin(Time.time * period + headStart) / amplitude) + zorigine) * Vector3.one;
        
    }
}

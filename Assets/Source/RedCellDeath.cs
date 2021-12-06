using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCellDeath : MonoBehaviour
{
    ParticleSystem[] part;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        int count = 0;
        foreach (ParticleSystem item in part)
        {
            count += item.particleCount;
        }
        if (count <= 0 && timer >= 0.2f)
        {
            Destroy(gameObject);
        }
    }
}

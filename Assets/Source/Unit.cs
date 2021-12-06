using UnityEngine;

public class Unit : MonoBehaviour
{
    public Box CurrentBox = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        gameObject.transform.position = CurrentBox.GetWorldPos();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterAnimEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeTransform()
    {
        transform.parent.gameObject.transform.Rotate(0, 180, 0);
    }
}

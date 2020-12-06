using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float d = _MGR_Spawner.Instance.multiply * _MGR_Spawner.Instance.nTrees /4;
        transform.localScale = new Vector3(d, d, d);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

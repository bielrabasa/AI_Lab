using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWander : MonoBehaviour
{
    Movement mov;
    
    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        mov.Wander();
    }
}

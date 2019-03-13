using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionNode : MonoBehaviour {

    public GameObject next;
    
    public DirectionNode()
    {
        
    }

    public GameObject getNext()
    {
        return next;
    }
}

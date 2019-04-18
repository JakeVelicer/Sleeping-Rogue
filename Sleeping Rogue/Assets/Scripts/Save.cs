using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<Vector3> objectStates = new List<Vector3>();
    public List<Quaternion> objectRot = new List<Quaternion>();
    public List<bool> buttonStates = new List<bool>();
    public List<bool> interactableStates = new List<bool>();
    public List<GameObject> interactables = new List<GameObject>();
    public List<GameObject> buttons = new List<GameObject>();
    public List<GameObject> objects = new List<GameObject>();
}


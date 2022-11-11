using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireHandController : MonoBehaviour
{
    [SerializeField]
    private WireController[] wireControllers;
    // Start is called before the first frame update
    [SerializeField]
    private Transform target;


    public void ToggleFire()
    {
        foreach (WireController wireController in wireControllers)
        {
            wireController.ToggleFire();
            wireController.SetTarget(target);
        }
    }
}

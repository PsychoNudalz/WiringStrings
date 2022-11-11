using System;
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


    [Header("Tether point finder")]
    [SerializeField]
    private Transform head;

    [SerializeField]
    private float range = 30f;

    [SerializeField]
    private LayerMask layerMask;


    [SerializeField]
    private bool activeWires = false;


    private void Start()
    {
        foreach (WireController wireController in wireControllers)
        {
            wireController.SetFire(activeWires,target);
        }
    }

    public void ToggleFire()
    {
        if (!activeWires)
        {
            target = GetTarget();
            if (target)
            {
                foreach (WireController wireController in wireControllers)
                {
                    wireController.ToggleFire(target);
                }

                activeWires = true;
            }
        }
        else
        {
            activeWires = false;
            foreach (WireController wireController in wireControllers)
            {
                wireController.ToggleFire(null);
            }
        }
        
    }

    public Transform GetTarget()
    {
        RaycastHit hit;
        Debug.DrawLine(head.position,head.position+head.forward*range,Color.red,1f);
        if (Physics.Raycast(head.position, head.forward, out hit, range, layerMask))
        {
            print("Hit");
            if (hit.collider.TryGetComponent(out TetherPoint t))
            {
                return t.transform;
            }
        }
        return null;
    }
}
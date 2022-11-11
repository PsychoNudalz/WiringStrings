using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private WireHandController wireHandController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnShoot(InputValue inputValue)
    {
        wireHandController.ToggleFire();
    }
}

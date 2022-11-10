using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private WireController wireController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnShoot(InputValue inputValue)
    {
        wireController.ToggleFire();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour {
    public event EventHandler onIntercatAction;
    private PlayerInputActions playerInputActions;
    private void Awake(){
    
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (onIntercatAction != null)
        {
            onIntercatAction(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovmentVectorNormalized(){
    
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>(); ;

        inputVector = inputVector.normalized;

        return inputVector;
    }
}



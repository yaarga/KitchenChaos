using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour {

    public static GameInput Instance{get; private set;}

    public event EventHandler OnIntercatAction;
    public event EventHandler OnIntercatAlternateAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputActions;
    private void Awake(){
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

    }
    private void OnDestroy(){
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Dispose();
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnIntercatAlternateAction(this, EventArgs.Empty);

    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        OnPauseAction?.Invoke(this, EventArgs.Empty);
  
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnIntercatAction != null)
        {
            OnIntercatAction(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovmentVectorNormalized(){
    
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>(); ;

        inputVector = inputVector.normalized;

        return inputVector;
    }
}



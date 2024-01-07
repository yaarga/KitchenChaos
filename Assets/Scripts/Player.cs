using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements.Experimental;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public ClearCounter selectedCounter;
    
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;


    private bool isWalking;
    private Vector3 lastIntercatDir;
    private ClearCounter selectedCounter;

    private void Awake(){
        if (Instance != null){

            Debug.Log("there is more than one player instance");
        }
        Instance = this;
    }
    private void Start(){
        gameInput.onIntercatAction += GameInput_onIntercatAction;
    }

    private void GameInput_onIntercatAction(object sender, EventArgs e){
        if (selectedCounter != null) {
            selectedCounter.Interact();
        }
       
    }

    private void Update(){
        HandleMovmvent();
        HandleIntercations();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleIntercations()
    {
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir!=Vector3.zero)
        {
            lastIntercatDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastIntercatDir, out RaycastHit raycastHit, interactDistance,countersLayerMask)){
           if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                //Has ClearCounter
                if (clearCounter != selectedCounter){
                    SetSelectedCounter(clearCounter);



                }
            } else{
                SetSelectedCounter(null);
            }
        }else{
            SetSelectedCounter(null);
        }
        Debug.Log(selectedCounter);
    }
    private void HandleMovmvent()
    {
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Cannot move towards moveDir

            //Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move only on the X

                //Attemot only Z movment
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }
    private void SetSelectedCounter (ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}

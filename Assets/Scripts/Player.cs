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

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;


    private bool isWalking;
    private Vector3 lastIntercatDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake(){
        if (Instance != null){

            Debug.Log("there is more than one player instance");
        }
        Instance = this;
    }
    private void Start(){
        gameInput.OnInteractAction += GameInput_OnIntercatAction;
        gameInput.OnIntercatAlternateAction += GameInput_OnIntercatAlternateAction;

    }

    private void GameInput_OnIntercatAlternateAction(object sender, System.EventArgs e){
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null){
            selectedCounter.InteractAlternate(this);
        }

    }
    private void GameInput_OnIntercatAction(object sender, System.EventArgs e){
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
       
    }

    private void Update(){
        HandleMovmvent();
        HandleIntercations();
    }
    public bool IsWalking(){
    
        return isWalking;
    }
    private void HandleIntercations(){
    
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir!=Vector3.zero){
        
            lastIntercatDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastIntercatDir, out RaycastHit raycastHit, interactDistance,countersLayerMask)){
           if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                //Has ClearCounter
                if (baseCounter != selectedCounter){
                    SetSelectedCounter(baseCounter);



                }
            } else{
                SetSelectedCounter(null);
            }
        }else{
            SetSelectedCounter(null);
        }
        
    }
    private void HandleMovmvent(){
    
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
            canMove = (moveDir.x<-5f || moveDir.x>.5f) &&  !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

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
                canMove = (moveDir.z < -5f || moveDir.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
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
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null){
            //if the player did pick up something - fire off this event
            OnPickedSomething?.Invoke(this, EventArgs.Empty);

        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}

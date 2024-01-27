using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecpieSO[] fryingRecpieSOArray;
    [SerializeField] private BurningRecpieSO[] burningRecpieSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecpieSO fryingRecpieSO;
    private BurningRecpieSO burningRecpieSO;

    private float burningTimer;
    private void Start(){
        state = State.Idle;
    }
    private void Update(){
        if (HasKitchenObject()){
            switch (state){
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecpieSO.fryingTimerMax
                    });
                    if (fryingTimer > fryingRecpieSO.fryingTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecpieSO.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecpieSO = GetBurningRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state

                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecpieSO.burningTimerMax
                    });
                    if (burningTimer > burningRecpieSO.burningTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecpieSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                    }
                    break;
                case State Burned:
                    break;

            }

        }

    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //there is no kitchen obj
            if (player.HasKitchenObject())
            {
                //player is carring something that can be fried
                if (HasRecpieWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Drop it
                    player.GetKitchenObject().SetKitchenObjectParent(this);


                    fryingRecpieSO = GetFryingRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = fryingTimer / fryingRecpieSO.fryingTimerMax
                    }) ;
                }
            }
            else
            {
                //player not carrying anything
            }
        }
        else
        {
            //There is KitchenObject here
            if (player.HasKitchenObject())
            {
                //player is carring something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf(); 
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state

                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                }
            }
            else
            {
                //player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state

                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecpieWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecpieSO fryingRecpieSO = GetFryingRecpieSOWithInput(inputKitchenObjectSO);
        return fryingRecpieSO != null;

    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        FryingRecpieSO fryingRecpieSO = GetFryingRecpieSOWithInput(inputKitchenObjectSO);
        if (fryingRecpieSO != null){       
            return fryingRecpieSO.output;
        }
        else{       
            return null;
        }

    }
    private FryingRecpieSO GetFryingRecpieSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach (FryingRecpieSO fryingRecpieSO in fryingRecpieSOArray){
            if (fryingRecpieSO.input == inputKitchenObjectSO){
                return fryingRecpieSO;
            }
        }
        return null;
    }
     private BurningRecpieSO GetBurningRecpieSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach (BurningRecpieSO burningRecpieSO in burningRecpieSOArray){
            if (burningRecpieSO.input == inputKitchenObjectSO){
                return burningRecpieSO;
            }
        }
        return null;
    }
}

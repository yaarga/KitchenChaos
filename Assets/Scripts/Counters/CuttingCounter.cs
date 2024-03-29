using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress {

    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecpieSOArray;
    private int cuttingProgress;
    public override void Interact(Player player){
        if (!HasKitchenObject()){
            //there is no kitchen obj
            if (player.HasKitchenObject()){
                //player is carring something
                if (HasRecpieWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    //Drop it
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecpieSO = GetCuttingRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecpieSO.cuttingProgressMax
                    });
                }
            }
            else{
                //player not carrying anything
            }
        }
        else{
            //There is KitchenObject here
            if (player.HasKitchenObject()){
                //player is carring somtehing 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
            }
            else{
                //player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecpieWithInput(GetKitchenObject().GetKitchenObjectSO())){
        //There is a kitchenObject here and it can be cut
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecpieSO = GetCuttingRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                progressNormalized = (float)cuttingProgress / cuttingRecpieSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecpieSO.cuttingProgressMax){
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }
    private bool HasRecpieWithInput(KitchenObjectSO inputKitchenObjectSO){
        CuttingRecipeSO cuttingRecpieSO = GetCuttingRecpieSOWithInput(inputKitchenObjectSO);
        return cuttingRecpieSO != null;

    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        CuttingRecipeSO cuttingRecpieSO = GetCuttingRecpieSOWithInput(inputKitchenObjectSO);
        if (cuttingRecpieSO != null){
           return cuttingRecpieSO.output;
        }
        else{
            return null;
        }
        
    }
    private CuttingRecipeSO GetCuttingRecpieSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach (CuttingRecipeSO cuttingRecpieSO in cuttingRecpieSOArray){
            if (cuttingRecpieSO.input == inputKitchenObjectSO){
                return cuttingRecpieSO;
            }
        }
        return null;
    }
}


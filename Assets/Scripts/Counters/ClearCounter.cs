using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player){
        if (!HasKitchenObject()){
            //there is no kitchen obj
            if (player.HasKitchenObject()){
                //player is carring smthing
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else{
                //player not carrying anything
            }
        } else{
            //There is KitchenObject here
            if (player.HasKitchenObject()){
                //player is carring somthing 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else{
                    //player is not holding a plate but something else
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject)){
                        //Counter is holding a plate
                        if (plateKitchenObject.TryAddIngrediant(player.GetKitchenObject().GetKitchenObjectSO())){
                            player.GetKitchenObject().DestroySelf();
                        }
                    
                    }
                }
            } else{
                //player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }


}

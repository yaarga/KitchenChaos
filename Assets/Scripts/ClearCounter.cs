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
            } else{
                //player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }


}

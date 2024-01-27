using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlateCompleteVisual : MonoBehaviour{
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;
    private void Start(){
        plateKitchenObject.OnIngrediantAdded += PlateKitchenObject_OnIngrediantAdded;

        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList){
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngrediantAdded(object sender, PlateKitchenObject.OnIngrediantAddedEventArgs e){
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList){
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO){
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}

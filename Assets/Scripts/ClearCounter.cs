using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour{
    [SerializeField] private Transform tomatoPrefab;
    [SerializeField] private Transform counterTopPoint;

    public void Interact(){

        Debug.Log("Interact!");
        Transform tomatoTransform = Instantiate(tomatoPrefab, counterTopPoint);
        tomatoTransform.localPosition = Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    [SerializeField] PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    private List<GameObject> plateVisualGameObjectList;

    private void Awake()//This method is called when the script instance is being loaded.It initializes the plateVisualGameObjectList list.
    {
        plateVisualGameObjectList= new List<GameObject>();
    }
    private void Start(){ //This method is called before the first frame update.
        //It subscribes to the OnPlatesSpawned event of the platesCounter.
        //This suggests that platesCounter raises an event whenever plates are spawned.
        platesCounter.OnPlatesSpawned += PlatesCounter_OnPlatesSpawned;
        platesCounter.OnPlatesRemoved += PlatesCounter_OnPlatesRemoved;

    }
    private void PlatesCounter_OnPlatesRemoved(object sender, System.EventArgs e) {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count -1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
    
        private void PlatesCounter_OnPlatesSpawned(object sender, System.EventArgs e){
        //When plates are spawned, it instantiates a new plate visual object based on the plateVisualPrefab.
        //It calculates the vertical position for the new plate visual object based on the count of plates already present,
        //adding an offset (platesOffsetY) to each subsequent plate.
        //It adds the instantiated plate visual object to the plateVisualGameObjectList.
        Transform platesVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float platesOffsetY = .1f;

        platesVisualTransform.localPosition = new Vector3(0, platesOffsetY* plateVisualGameObjectList.Count,0);

        plateVisualGameObjectList.Add(platesVisualTransform.gameObject);
    }
    //Overall, this script dynamically creates plate visual representations in the game world as plates are spawned,
    //stacking them vertically on top of each other at the counterTopPoint.
}

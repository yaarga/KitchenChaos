using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI CountdownText;
    private void Start(){    
        KitchenGameManager.Instance.OnStateChanged += KitchenGameMagnager_OnStateChanged;
    }

    private void KitchenGameMagnager_OnStateChanged(object sender, System.EventArgs e){    
        if (KitchenGameManager.Instance.IsCountdownToStartActive()){        
            Show();
        }
        else{        
            Hide();
        }
    }

    private void Update(){    
        CountdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
     }
    private void Show(){    
        gameObject.SetActive(true);
    }
    private void Hide(){    
        gameObject.SetActive(false);
    }
}

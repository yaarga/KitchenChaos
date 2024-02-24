using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour{

    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI CountdownText;
    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator=GetComponent<Animator>();
    }

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
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        CountdownText.text =countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber=countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }
    private void Show(){    
        gameObject.SetActive(true);
    }
    private void Hide(){    
        gameObject.SetActive(false);
    }
}

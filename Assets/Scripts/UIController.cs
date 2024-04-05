using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private int acorns = 0;

    [SerializeField] private TextMeshProUGUI acornValue;

    [SerializeField] private TextMeshProUGUI foodInfo;

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PICKUP_ACORN, OnAcornValueChanged);
        //Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        //Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_ACORN, OnAcornValueChanged);
        //Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        //Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAcornValueChanged(int acorn)
    {
        acorns++;
        acornValue.text = acorns.ToString();
    }
}

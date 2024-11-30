using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EOG_SwapsiesButton : MonoBehaviour
{
    bool IsShowMap = true;
    public TMPro.TextMeshProUGUI _wiredText;
    public UI_EOGOverlayManager _wiredOverlayManager;
    
    void Start()
    {
        _wiredText.text = "View Map";
    }

    public void DoAndSwap() {
        IsShowMap = !IsShowMap;
        if (IsShowMap) {
            _wiredText.text = "View Map";
            _wiredOverlayManager.CloseOverlay();
        } else {
            _wiredText.text = "View Results";
            _wiredOverlayManager.Present(null);
        }
    }
}

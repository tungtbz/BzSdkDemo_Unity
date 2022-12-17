using System;
using System.Collections.Generic;
using RofiSdk;
using RofiSdk.UI.PopupManager;
using UnityEngine;

public class PopupManager : PersistentSingleton<PopupManager>
{
    private List<BasePopupUi> _popups;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _popups = new List<BasePopupUi>();
    }

    /// <summary>
    /// Open and add popup to stack
    /// </summary>
    /// <param name="popup"></param>
    public void OpenPopup(OpenPopupSetting openPopupSetting)
    {
        var popupInstance = SimpleObjectPool.Spawn(openPopupSetting.popupPrefabPath, transform);
        if (popupInstance == null)
        {
            return;
        }

        var popup = popupInstance.GetComponent<BasePopupUi>();
        _popups.Add(popup);
        popup.Open();
    }
    
    /// <summary>
    /// Close top popup
    /// </summary>
    public void CloseTopPopup()
    {
        if(_popups.Count == 0) return;
        var popup = _popups[_popups.Count - 1];
        popup.Close(PopupCloseFlag.MANAGER);
        
    }
}

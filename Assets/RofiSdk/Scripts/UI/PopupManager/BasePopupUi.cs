using System;
using UnityEngine;

namespace RofiSdk.UI.PopupManager
{
    public class BasePopupUi : MonoBehaviour
    {
        public virtual void Open()
        {
            
        }

        public virtual void Close(PopupCloseFlag closeFlag)
        {
            SimpleObjectPool.Recycle(gameObject);
        }
    }
}
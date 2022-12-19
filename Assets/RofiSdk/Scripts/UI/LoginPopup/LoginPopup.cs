using System.Collections;
using System.Collections.Generic;
using RofiSdk.UI.PopupManager;
using TMPro;
using UnityEngine;

public class LoginPopup : BasePopupUi
{
    [SerializeField] private TMP_InputField usernameTextField;
    [SerializeField] private TMP_InputField passwordTextField;
    
    // Start is called before the first frame update
    void Start()
    {
        InitUi();
    }
    
    private void InitUi()
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
 
        /*Left*/ rectTransform.offsetMin = Vector2.zero;
        /*Right*/ rectTransform.offsetMax = Vector2.zero;

        usernameTextField.contentType = TMP_InputField.ContentType.EmailAddress;
        TMP_Text placeholderTextComponent = usernameTextField.placeholder as TMP_Text;
        if (placeholderTextComponent != null) placeholderTextComponent.text = "Enter username or email";
        
        passwordTextField.contentType = TMP_InputField.ContentType.Password;
        placeholderTextComponent = passwordTextField.placeholder as TMP_Text;
        if (placeholderTextComponent != null) placeholderTextComponent.text = "Enter Password";
    }

    public override void Open()
    {
        // gameObject.SetActive(true);
    }

    public override void Close(PopupCloseFlag closeFlag)
    {
        // gameObject.SetActive(false);
        base.Close(closeFlag);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginPopup : MonoBehaviour
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
        usernameTextField.contentType = TMP_InputField.ContentType.EmailAddress;
        TMP_Text placeholderTextComponent = usernameTextField.placeholder as TMP_Text;
        if (placeholderTextComponent != null) placeholderTextComponent.text = "Enter username or email";
        
        passwordTextField.contentType = TMP_InputField.ContentType.Password;
        placeholderTextComponent = passwordTextField.placeholder as TMP_Text;
        if (placeholderTextComponent != null) placeholderTextComponent.text = "Enter Password";
    }
}

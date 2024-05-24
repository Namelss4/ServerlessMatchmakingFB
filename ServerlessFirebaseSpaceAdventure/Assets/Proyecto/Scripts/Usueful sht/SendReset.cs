using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendReset : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_Text messageText;

    public void RecoverPassword()
    {
        string email = emailInput.text;

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                messageText.text = "Se ha enviado un correo electr�nico para restablecer tu contrase�a.";
            }
            else
            {
                messageText.text = "Ha ocurrido un error al intentar enviar el correo electr�nico. Por favor, intenta de nuevo.";
            }
        });
    }
}

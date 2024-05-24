using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{
    private string uid;
    public string matchInfo;

    private void Start()
    {
        uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").ValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot.Exists)
        {
            matchInfo = e.Snapshot.Value.ToString();
            Debug.Log("Información del match actualizada: " + matchInfo);
            // Aquí puedes realizar acciones según la información del match actualizada
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }
    }

    void Update()
    {

        if (matchInfo == "")
        {
            gameObject.SetActive(false);
        }
    }
}
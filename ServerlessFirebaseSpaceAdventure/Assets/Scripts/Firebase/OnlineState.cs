using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class OnlineState : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentUserId;
    void Start()
    {
        Application.focusChanged += OnApplicationFocusChanged;
    }
    private void OnApplicationFocusChanged(bool hasFocus)
    {
        SetOnlineStatus(hasFocus);
    }
    private void OnDestroy()
    {
        Application.focusChanged -= OnApplicationFocusChanged;
    }
    private void SetOnlineStatus(bool isOnline)
    {
        if (!string.IsNullOrEmpty(currentUserId))
        {
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUserId).Child("online");
            if (isOnline)
            {
                userRef.OnDisconnect().SetValue(false);
                userRef.SetValueAsync(true);
            }
            else
            {
                userRef.SetValueAsync(false);
            }
        }
        else
        {
            Debug.LogError("currentUserId is null or empty.");
        }
    }
}

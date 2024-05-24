using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ChangeSearch : MonoBehaviour
{
    public bool isSearch;

    private void Start()
    {
        isSearch = false;

    }

    public void ChangeState()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        if (isSearch)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
            isSearch = false; 
        }
        else if (!isSearch) 
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(true);
            isSearch = true;
        }
    }
}

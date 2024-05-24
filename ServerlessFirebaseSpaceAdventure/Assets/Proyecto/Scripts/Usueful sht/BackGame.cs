using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGame : MonoBehaviour
{
    private string uid;
    private string scene;

    private void Start()
    {
        uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.GetReference($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").ValueChanged += HandleMatch;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("accepted").ValueChanged += HandleUser;
    }

    private void HandleMatch(object sender, ValueChangedEventArgs e)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").Child("searching").SetValueAsync(false);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").Child("match").SetValueAsync("");

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").Child("accepted").SetValueAsync(false);

        SceneManager.LoadScene(scene);
    }

    private void HandleUser(object sender, ValueChangedEventArgs e)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").Child("searching").SetValueAsync(false);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").Child("match").SetValueAsync("");

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").Child("accepted").SetValueAsync(false);

        SceneManager.LoadScene(scene);
    }

    public void Back()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
    }

}

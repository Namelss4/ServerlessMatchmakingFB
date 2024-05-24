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
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").ValueChanged += HandleMatch;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").ValueChanged += HandleUser;
    }

    private void HandleMatch(object sender, ValueChangedEventArgs e)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").SetValueAsync(false);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").SetValueAsync("");

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").SetValueAsync(false);

        SceneManager.LoadScene(scene);
    }

    private void HandleUser(object sender, ValueChangedEventArgs e)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").SetValueAsync(false);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").SetValueAsync("");

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child($"{FirebaseAuth.DefaultInstance.CurrentUser.UserId}").Child("match").SetValueAsync(false);

        SceneManager.LoadScene(scene);
    }

    public void Back()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
    }

}
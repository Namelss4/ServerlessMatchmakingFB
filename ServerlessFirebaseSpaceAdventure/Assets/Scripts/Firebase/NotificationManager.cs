using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Messaging;
using Firebase.Database;
using Firebase.Auth;
using System;

public class NotificationManager : MonoBehaviour
{
  /*  private DatabaseReference databaseReference;
    private FirebaseMessaging firebaseMessaging;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        firebaseMessaging = FirebaseMessaging.DefaultInstance;
        SubscribeToNotifications();
        GetToken();
    }
    private void SubscribeToNotifications()
    {
        firebaseMessaging.SubscribeAsync("/topics/yourTopicName").ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SubscribeAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SubscribeAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Subscribed to topic: yourTopicName");
        });
    }
    private void GetToken()
    {
        FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("GetTokenAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("GetTokenAsync encountered an error: " + task.Exception);
                return;
            }

            string token = task.Result;
            Debug.Log("Received registration token: " + token);

            // Almacena el token en la base de datos o realiza otras operaciones necesarias
            // ...
        });
    }
    private void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received registration token: " + token.Token);
        string currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        databaseReference.Child("users").Child(currentUserId).Child("notificationToken").SetValueAsync(token.Token);
    }

    public void SendNotificationToFriends(string userId)
    {
        databaseReference.Child("users").Child(userId).Child("friends").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener la lista de amigos: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            List<string> friendTokens = new List<string>();

            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                string friendId = childSnapshot.Key;
                databaseReference.Child("users").Child(friendId).Child("notificationToken").GetValueAsync().ContinueWithOnMainThread(tokenTask =>
                {
                    if (tokenTask.IsFaulted)
                    {
                        Debug.LogError("Error al obtener el token de notificación: " + tokenTask.Exception);
                        return;
                    }

                    string notificationToken = tokenTask.Result.Value.ToString();
                    if (!string.IsNullOrEmpty(notificationToken))
                    {
                        friendTokens.Add(notificationToken);
                    }
                });
            }

            if (friendTokens.Count > 0)
            {
                SendNotification(friendTokens, $"{FirebaseAuth.DefaultInstance.CurrentUser.DisplayName} se ha conectado.");
            }
        });
    }

    private void SendNotification(List<string> tokens, string message)
    {
        var data = new Dictionary<string, string>
        {
            { "message", message }
        };

        firebaseMessaging.SendAsync("/topics/yourTopicName", data);
    }*/
}

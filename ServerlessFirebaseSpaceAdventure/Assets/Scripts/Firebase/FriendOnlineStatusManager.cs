using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System;

public class FriendOnlineStatusManager : MonoBehaviour
{
   private DatabaseReference databaseReference;
   private string currentUserId;
   public NotificationManager notificationManager;

   private void Start()
   {
      databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
      currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

      LoadFriendList();
   }
   private void LoadFriendList()
   {
      databaseReference.Child("users").Child(currentUserId).Child("friends").GetValueAsync().ContinueWithOnMainThread(task =>
      {
         if (task.IsFaulted)
         {
            Debug.LogError("Error al obtener la lista de amigos: " + task.Exception);
            return;
         }

         DataSnapshot snapshot = task.Result;

         foreach (DataSnapshot childSnapshot in snapshot.Children)
         {
            string friendId = childSnapshot.Key;
            MonitorFriendOnlineStatus(friendId);
         }
      });
   }
   private void MonitorFriendOnlineStatus(string friendId)
   {
      DatabaseReference friendOnlineRef = databaseReference.Child("users").Child(friendId).Child("online");
      friendOnlineRef.ValueChanged += OnFriendOnlineStatusChanged;
   }
   private void OnFriendOnlineStatusChanged(object sender, ValueChangedEventArgs args)
   {
      string friendId = args.Snapshot.Key;
      bool isOnline = (bool)args.Snapshot.Value;

      // Obtener el nombre de usuario del amigo
      databaseReference.Child("users").Child(friendId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
      {
         if (task.IsFaulted)
         {
            Debug.LogError("Error al obtener el nombre de usuario: " + task.Exception);
            return;
         }

         string friendUsername = task.Result.Value.ToString();

         // Mostrar la notificación correspondiente
         if (notificationManager != null)
         {
            if (isOnline)
            {
               notificationManager.SetNotificationConnected(friendUsername);
            }
            else
            {
               notificationManager.SetNotificationDisconnected(friendUsername);
            }
         }
      });
   }
}
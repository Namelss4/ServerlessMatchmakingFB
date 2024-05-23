using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using System;

public class FriendListManager : MonoBehaviour
{
    public ScrollRect scrollView;
    public GameObject friendItemPrefab;

    private DatabaseReference databaseReference;
    private string currentUserId;
    private List<FriendItem> friendItems = new List<FriendItem>();
    private OnlineState onlineState;

    public void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        GameObject onlineStateObject = new GameObject("OnlineState");
        OnlineState onlineState = onlineStateObject.AddComponent<OnlineState>();
        onlineState.currentUserId = currentUserId;
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
                CreateFriendItem(friendId);
            }

            SortFriendsByOnlineStatus();
        });
    }

    private void CreateFriendItem(string friendId)
    {
        GameObject friendItemGO = Instantiate(friendItemPrefab, scrollView.content);
        FriendItem friendItem = friendItemGO.GetComponent<FriendItem>();
        friendItem.SetFriendId(friendId);
        friendItems.Add(friendItem);
        
        // Suscribirse a los cambios en el estado en línea del amigo
        DatabaseReference friendOnlineRef = databaseReference.Child("users").Child(friendId).Child("online");
        friendOnlineRef.ValueChanged += OnFriendOnlineStatusChanged;
    }
    private void OnFriendOnlineStatusChanged(object sender, ValueChangedEventArgs args)
    {
        SortFriendsByOnlineStatus();
    }

    public void SortFriendsByOnlineStatus()
    {
        friendItems.Sort((a, b) => b.IsOnline.CompareTo(a.IsOnline));

        for (int i = 0; i < friendItems.Count; i++)
        {
            friendItems[i].transform.SetSiblingIndex(i);
        }
    }
}

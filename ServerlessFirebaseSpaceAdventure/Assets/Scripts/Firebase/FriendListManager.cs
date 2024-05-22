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

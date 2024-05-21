using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System;

public class FriendRequestManager : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private string currentUserId;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child("friendRequests");
        currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    public void SendFriendRequest(string receiverId)
    {
        string requestKey = databaseReference.Child(currentUserId).Child(receiverId).Push().Key;
        FriendRequest request = new FriendRequest(currentUserId, receiverId, "pending");
        string jsonRequest = JsonUtility.ToJson(request);
        databaseReference.Child(currentUserId).Child(receiverId).Child(requestKey).SetRawJsonValueAsync(jsonRequest);
    }

    public void AcceptFriendRequest(string senderId, string requestKey)
    {
        databaseReference.Child(senderId).Child(currentUserId).Child(requestKey).Child("status").SetValueAsync("accepted");
        AddFriend(senderId);
        AddFriend(currentUserId);
    }

    public void RejectFriendRequest(string senderId, string requestKey)
    {
        databaseReference.Child(senderId).Child(currentUserId).Child(requestKey).Child("status").SetValueAsync("rejected");
    }

    private void AddFriend(string userId)
    {
        DatabaseReference userFriendsRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("friends");
        userFriendsRef.Child(currentUserId).SetValueAsync(true);
    }

    public void GetPendingFriendRequests(System.Action<List<FriendRequest>> callback)
    {
        databaseReference.Child(currentUserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener solicitudes de amistad: " + task.Exception);
                callback(null);
                return;
            }

            List<FriendRequest> requests = new List<FriendRequest>();
            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                foreach (DataSnapshot requestSnapshot in childSnapshot.Children)
                {
                    FriendRequest request = JsonUtility.FromJson<FriendRequest>(requestSnapshot.GetRawJsonValue());
                    if (request.status == "pending")
                    {
                        requests.Add(request);
                    }
                }
            }

            callback(requests);
        });
    }
}

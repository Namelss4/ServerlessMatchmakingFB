using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using TMPro;

public class FriendRequestManager : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private OnlineState onlineState;
    private string currentUserId;
    public ScrollRect scrollView;
    public GameObject requestItemPrefab;
    [SerializeField] private TMP_InputField _UserInputField;
    public TextMeshProUGUI _usernameText;
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users");
        currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        GameObject onlineStateObject = new GameObject("OnlineState");
        OnlineState onlineState = onlineStateObject.AddComponent<OnlineState>();
        onlineState.currentUserId = currentUserId;
    }

    public void SendFriendRequest()
    {
        if (_UserInputField != null && !string.IsNullOrEmpty(_UserInputField.text))
        {
            string receiverId = _UserInputField.text;
            if (receiverId != currentUserId) // Existing check for self-friend request
            {
                SendFriendRequest(receiverId);
            }
        }
        else
        {
            Debug.LogError("Friend request input field is null or empty!");
        }
    }
    public void SendFriendRequest(string receiverId)
    {
        string requestKey = databaseReference.Child(currentUserId).Child("friendRequests").Child(receiverId).Push().Key;
        Debug.Log("Request key: " + requestKey);
        Debug.Log("Current user ID: " + currentUserId);
        Debug.Log("Receiver ID: " + receiverId);
        Debug.Log("CurrentUser " + FirebaseAuth.DefaultInstance.CurrentUser);
        Debug.Log("Database Reference: " + databaseReference.ToString());

        //FriendRequest request = new FriendRequest(currentUserId, receiverId, "pending");
        /*FriendRequest request = gameObject.AddComponent<FriendRequest>();
        request.senderId = currentUserId;
        request.receiverId = receiverId;
        request.status = "pending";*/
        Dictionary<string, object> requestData = new Dictionary<string, object>
        {
            { "senderId", currentUserId },
            {"senderUsername", _usernameText.text},
            { "receiverId", receiverId },
            { "status", "pending" }
        };

        //string jsonRequest = JsonUtility.ToJson(request);

        databaseReference.Child(receiverId).Child("friendRequests").Child(requestKey).SetValueAsync(requestData);
    }

    public void AcceptFriendRequest(string senderId, string requestKey)
    {
        databaseReference.Child(currentUserId).Child("friendRequests").Child(requestKey).Child("status").SetValueAsync("accepted");
        AddFriend(senderId, currentUserId);
        AddFriend(currentUserId, senderId);
        // Eliminar la solicitud del nodo del remitente
        databaseReference.Child(senderId).Child("friendRequests").Child(currentUserId).Child(requestKey).RemoveValueAsync();
    }

    public void RejectFriendRequest(string senderId, string requestKey)
    {
        databaseReference.Child(senderId).Child("friendRequests").Child(currentUserId).Child(requestKey).Child("status").SetValueAsync("rejected");
    }

    private void AddFriend(string userId, string friendId)
    {
        databaseReference.Child(userId).Child("friends").Child(friendId).Child("id").SetValueAsync(friendId);
        databaseReference.Child(friendId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted)
            {
                string friendUsername = task.Result.Value.ToString();
                Debug.Log("Friend username: " + friendUsername);
                databaseReference.Child(userId).Child("friends").Child(friendId).Child("username").SetValueAsync(friendUsername);
            }
        });
    }

    public void GetPendingFriendRequests(System.Action<List<FriendRequest>> callback)
    {
        databaseReference.Child(currentUserId).Child("friendRequests").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener solicitudes de amistad: " + task.Exception);
                callback(null);
                return;
            }

            List<FriendRequest> requests = new List<FriendRequest>();
            DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot requestSnapshot in snapshot.Children)
                {
                    IDictionary requestData = (IDictionary)requestSnapshot.Value;
                    string senderId = requestData["senderId"].ToString();
                    string senderUsername = requestData["senderUsername"].ToString();
                    string receiverId = requestData["receiverId"].ToString();
                    string status = requestData["status"].ToString();
                    
                    if (receiverId == currentUserId && status == "pending")
                    {
                        FriendRequest request = new FriendRequest(senderId, senderUsername, receiverId, status);
                        requests.Add(request);
                    }
                }
            
            callback(requests);
        });
    }
    public void CreateFriendRequestItems()
    {
        GetPendingFriendRequests(requests =>
        {
            if (requests != null)
            {
                foreach (FriendRequest request in requests)
                {
                    CreateRequestItem(request.senderId, request.senderUsername,request.receiverId);
                }
            }
        });
    }

    private void CreateRequestItem(string senderId,string senderUsername, string receiverId)
    {
        GameObject requestItemGO = Instantiate(requestItemPrefab, scrollView.content);
        FriendRequestItem requestItem = requestItemGO.GetComponent<FriendRequestItem>();
        requestItem.SetRequestData(senderId,senderUsername, receiverId, this);
    }
}

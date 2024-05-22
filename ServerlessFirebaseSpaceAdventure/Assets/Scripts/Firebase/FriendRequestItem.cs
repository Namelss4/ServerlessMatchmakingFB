using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class FriendRequestItem : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public Button acceptButton;
    public Button rejectButton;

    private string senderId;
    private string receiverId;
    private DatabaseReference databaseReference;
    private FriendRequestManager friendRequestManager;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        acceptButton.onClick.AddListener(AcceptRequest);
        rejectButton.onClick.AddListener(RejectRequest);
    }

    public void SetRequestData(string senderId, string receiverId, FriendRequestManager friendRequestManager)
    {
        this.senderId = senderId;
        this.receiverId = receiverId;
        this.friendRequestManager = friendRequestManager;
        LoadUserData();
    }

    private void LoadUserData()
    {
        databaseReference.Child("users").Child(senderId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener el nombre de usuario: " + task.Exception);
                return;
            }

            string username = task.Result.Value.ToString();
            usernameText.text = username;
        });
    }

    private void AcceptRequest()
    {
        friendRequestManager.AcceptFriendRequest(senderId, receiverId);
        Destroy(gameObject);
    }

    private void RejectRequest()
    {
        friendRequestManager.RejectFriendRequest(senderId, receiverId);
        Destroy(gameObject);
    }
}

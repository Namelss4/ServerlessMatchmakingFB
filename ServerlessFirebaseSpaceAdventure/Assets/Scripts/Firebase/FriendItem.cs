using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using System.Linq;
using TMPro;

public class FriendItem : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public Image onlineStatusImage;

    private string friendId;
    private DatabaseReference databaseReference;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SetFriendId(string friendId)
    {
        this.friendId = friendId;
        LoadFriendData();
        MonitorOnlineStatus();
    }

    private void LoadFriendData()
    {
        databaseReference.Child("users").Child(friendId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
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

    private void MonitorOnlineStatus()
    {
        databaseReference.Child("users").Child(friendId).Child("online").ValueChanged += OnlineStatusChanged;
    }

    private void OnlineStatusChanged(object sender, ValueChangedEventArgs args)
    {
        bool isOnline = (bool)args.Snapshot.Value;
        onlineStatusImage.color = isOnline ? Color.green : Color.red;
        GetComponentInParent<FriendListManager>().SortFriendsByOnlineStatus();
    }

    public bool IsOnline
    {
        get
        {
            return onlineStatusImage.color == Color.green;
        }
    }
}

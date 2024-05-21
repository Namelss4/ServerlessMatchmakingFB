using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FriendRequest : MonoBehaviour
{
    public string senderId { get; set; }
    public string receiverId { get; set; }
    public string status { get; set; } // "pending", "accepted", "rejected"

    public FriendRequest()
    {
    }
    public FriendRequest(string senderId, string receiverId, string status)
    {
        this.senderId = senderId;
        this.receiverId = receiverId;
        this.status = status;
    }
}

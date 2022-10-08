using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortNpc : MonoBehaviour
{
    public void FollowPlayer(Transform playerTransform)
    {
        this.transform.SetParent(playerTransform);
    }
}

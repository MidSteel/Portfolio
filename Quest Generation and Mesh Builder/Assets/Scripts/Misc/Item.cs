using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Player player;
        
        if (other.TryGetComponent(out player))
        {
            player.PlayerInventory.AddItemToInventory(this.GetComponent<Entity>().Id);
            Destroy(this.gameObject);
        }
    }
}

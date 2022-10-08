using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour
{
    public delegate void OnKill(int id);
    public OnKill onKill;

    public void OnTriggerEnter(Collider other)
    {
        Health h;
        if (other.TryGetComponent(out h))
        {
            int id = h.GetComponent<Entity>().Id;
            onKill?.Invoke(id);
            h.Kill();
        }
    }
}

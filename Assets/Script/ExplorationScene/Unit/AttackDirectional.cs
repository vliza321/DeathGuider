using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirectional : MonoBehaviour
{
    private GameObject player;
    private Vector2 target;

    private void Awake()
    {
        player = this.transform.parent.GetComponent<PlayerSwap>().guider;
    }
}

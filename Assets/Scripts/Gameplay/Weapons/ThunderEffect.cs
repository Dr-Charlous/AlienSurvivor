using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffect : MonoBehaviour
{
    [SerializeField] float _timerDeath = 60;

    void Start()
    {
        Destroy(gameObject, _timerDeath);
    }
}

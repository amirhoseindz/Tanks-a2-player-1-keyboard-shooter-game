using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKitManager : MonoBehaviour
{
    public float m_RedAidKitHealing = 15f;
    public int m_RedFirstAidKitRespawnTime = 100;
    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
    }
}

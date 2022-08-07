using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarFirstAidKit : MonoBehaviour
{
    public float m_FarFirstAidKitHealing = 50f;
    public int m_FarFirstAidKitRespawnTime = 12;
    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFirstAidKit : MonoBehaviour
{
    public float m_CloseAidKitHealing = 20f;
    public int m_CloseFirstAidKitRespawnTime = 100;
    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
    }
}

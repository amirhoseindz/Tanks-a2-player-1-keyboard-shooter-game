using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarFirstAidKit : MonoBehaviour
{
    public float m_FarFirstAidKitHealingForTankNum1 = 45f;
    public int m_FarFirstAidKitRespawnTime = 12;
    public float m_FarFirstAidKitHealingForTankNum2 = 35f;
    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
    }
}

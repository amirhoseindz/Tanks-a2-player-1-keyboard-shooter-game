using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;             
    public Rigidbody m_Shell;                  
    public Transform m_FireTransform;          
    public Slider m_AimSlider;                 
    public AudioSource m_ShootingAudio;         
    public AudioClip m_ChargingClip;            
    public AudioClip m_FireClip;               
    public float m_MinLaunchForce = 15f;        
    public float m_MaxLaunchForce = 30f;        
    public float m_MaxChargeTime = 0.75f;
    public float m_RotateSpeed = 200f;



    private string m_FireButton;               
    private float m_CurrentLaunchForce;        
    private float m_ChargeSpeed;                
    private bool m_Fired;
    private Rigidbody shellInstance;
    private GameManager m_Target;
    private Vector3 missileDirection;
    private Vector3 m_HomingMissileTarget;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start ()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        
        m_Target = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update ()
    {
        m_AimSlider.value = m_MinLaunchForce;
        
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            if (m_PlayerNumber == 1)
            {
                m_HomingMissileTarget = m_Target.m_Tanks[1].m_Instance.transform.position;
            }
            else
            {
                m_HomingMissileTarget = m_Target.m_Tanks[0].m_Instance.transform.position;
            }
            Debug.Log(m_HomingMissileTarget);
            m_FireTransform.position = Vector3.MoveTowards(m_FireTransform.position, m_HomingMissileTarget, 
                m_MaxLaunchForce * Time.deltaTime);
            missileDirection = m_HomingMissileTarget - m_FireTransform.position;
            missileDirection.Normalize();
            var rotateAmount = Quaternion.LookRotation(missileDirection);
            m_FireTransform.rotation = Quaternion.Slerp(m_FireTransform.rotation,
                rotateAmount, m_RotateSpeed * Time.deltaTime);
            Fire();
        }
        else if (Input.GetButtonDown (m_FireButton))
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play ();
        }
        else if (Input.GetButton (m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp (m_FireButton) && !m_Fired)
        {
            Fire ();
        }
    }


    private void Fire ()
    {
        m_Fired = true;

        shellInstance =
            Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play ();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}
using System;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;                       
    public ParticleSystem m_ExplosionParticles;        
    public AudioSource m_ExplosionAudio;               
    public float m_MaxDamage = 100f;                 
    public float m_ExplosionForce = 1000f;             
    public float m_MaxLifeTime = 10f;                   
    public float m_ExplosionRadius = 5f;
    public float m_RotateSpeed = 200f;

    private GameManager m_Target;
    private TankShooting m_TankShooting;
    private Vector3 missileDirection;
    private Vector3 m_HomingMissileTarget;
    

    private void Start ()
    {
        Destroy (gameObject, m_MaxLifeTime);
        
        m_Target = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        m_TankShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<TankShooting>();
    }

    private void Update()
    {
        if (m_TankShooting.m_Flag)
        {
            if (m_TankShooting.m_PlayerNumber == 1)
            {
                m_HomingMissileTarget = m_Target.m_Tanks[1].m_Instance.transform.position;
            }
            else
            {
                m_HomingMissileTarget = m_Target.m_Tanks[1].m_Instance.transform.position;
            }
            missileDirection = m_HomingMissileTarget - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, m_HomingMissileTarget, 
                m_TankShooting.m_MaxLaunchForce * Time.deltaTime);
            missileDirection.Normalize();
            var rotateAmount = Quaternion.LookRotation(missileDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateAmount, m_RotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage (targetRigidbody.position);

            targetHealth.TakeDamage (damage);
        }

        m_ExplosionParticles.transform.parent = null;

        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

        Destroy (gameObject);
    }


    private float CalculateDamage (Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;

        damage = Mathf.Max (0f, damage);

        return damage;
    }
}
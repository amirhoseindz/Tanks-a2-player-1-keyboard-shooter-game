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
    public Transform m_HoomingShellTarget;
    
    private Vector3 missileDirection;
    private TankShooting m_TankShooting;



    private void Start ()
    {
        Destroy (gameObject, m_MaxLifeTime);

        m_TankShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<TankShooting>();

    }

    public void SetHomingShellTarget(Transform homingShellTarget)
    {
        m_HoomingShellTarget.position = homingShellTarget.position;
    }

    private void Update()
    {
        if (m_HoomingShellTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_HoomingShellTarget.position, 
                m_TankShooting.m_MaxLaunchForce * Time.deltaTime);
            missileDirection = m_HoomingShellTarget.position - transform.position;
            missileDirection.Normalize();
            var rotateAmount = Quaternion.LookRotation(missileDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                rotateAmount, m_RotateSpeed * Time.deltaTime);
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
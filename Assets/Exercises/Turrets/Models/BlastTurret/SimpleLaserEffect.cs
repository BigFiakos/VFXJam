using UnityEngine;
using UnityEngine.InputSystem;

namespace Antoine
{
    public class SimpleLaserEffect : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private ParticleSystem m_laserParticles;
        [SerializeField] private Transform m_firePoint;

        [Header("Settings")]
        [SerializeField] private float m_growthDuration = 0.1f;
        [SerializeField] private float m_lifeDuration = 0.5f;
        [SerializeField] private float m_maxLineWidth = 0.2f;
        [SerializeField] private float m_laserDistance = 50f;

        private Coroutine m_laserRoutine;
        private WaitForSeconds m_lifeWait;

        private void Awake()
        {
            if (m_lineRenderer != null)
            {
                m_lineRenderer.enabled = false;
                m_lineRenderer.useWorldSpace = true;
            }

            if (m_laserParticles != null)
            {
                m_laserParticles.Stop();
            }

            m_lifeWait = new WaitForSeconds(m_lifeDuration);
        }

        private void Update()
        {
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (m_lineRenderer == null || m_firePoint == null) return;

                if (m_laserRoutine != null)
                {
                    StopCoroutine(m_laserRoutine);
                }
                m_laserRoutine = StartCoroutine(HandleLaserSequence());
            }
        }

        private System.Collections.IEnumerator HandleLaserSequence()
        {
            m_lineRenderer.enabled = true;

            if (m_laserParticles != null)
            {
                m_laserParticles.transform.position = m_firePoint.position;
                m_laserParticles.transform.rotation = m_firePoint.rotation;
                m_laserParticles.Play();
            }

            float elapsedTime = 0f;
            while (elapsedTime < m_growthDuration)
            {
                elapsedTime += Time.deltaTime;
                float currentWidth = Mathf.Lerp(0f, m_maxLineWidth, elapsedTime / m_growthDuration);

                Vector3 targetPosition = m_firePoint.position + (m_firePoint.forward * m_laserDistance);
                m_lineRenderer.SetPosition(0, m_firePoint.position);
                m_lineRenderer.SetPosition(1, targetPosition);

                if (m_laserParticles != null)
                {
                    m_laserParticles.transform.position = m_firePoint.position;
                    m_laserParticles.transform.rotation = m_firePoint.rotation;
                }

                m_lineRenderer.startWidth = currentWidth;
                m_lineRenderer.endWidth = currentWidth;
                yield return null;
            }

            m_lineRenderer.startWidth = m_maxLineWidth;
            m_lineRenderer.endWidth = m_maxLineWidth;

            yield return m_lifeWait;

            m_lineRenderer.enabled = false;

            if (m_laserParticles != null)
            {
                m_laserParticles.Stop();
            }

            m_laserRoutine = null;
        }
    }
}
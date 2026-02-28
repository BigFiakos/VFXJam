using UnityEngine;
using UnityEngine.InputSystem;

namespace Antoine
{
    public sealed class DoubleTurret : MonoBehaviour
    {
        [SerializeField] private GameObject mprojectilePrefab;
        [SerializeField] private GameObject mflamePrefab;
        [SerializeField] private Transform mfirePointLeft;
        [SerializeField] private Transform mfirePointRight;

        [Header("Settings")]
        [SerializeField] private float mfireRate = 0.5f;
        [SerializeField] private float mprojectileForce = 5f;
        [SerializeField] private float mflameDuration = 0.1f;

        private float mnextFireTime;
        private bool misLeftTurn = true;

        private void Update()
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Time.time >= mnextFireTime)
            {
                Shoot();
                mnextFireTime = Time.time + (mfireRate * 0.5f);
            }
        }

        private void Shoot()
        {
            Transform activePoint = misLeftTurn ? mfirePointLeft : mfirePointRight;

            if (activePoint == null) return;

            if (mflamePrefab != null)
            {
                GameObject flame = Instantiate(mflamePrefab, activePoint.position, activePoint.rotation, activePoint);

                flame.transform.localPosition = Vector3.zero;
                flame.transform.localRotation = Quaternion.identity;
                flame.transform.localScale = Vector3.one;

                Destroy(flame, mflameDuration);
            }

            if (mprojectilePrefab != null)
            {
                GameObject projectile = Instantiate(mprojectilePrefab, activePoint.position, activePoint.rotation);

                if (projectile.TryGetComponent(out Rigidbody rb))
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.AddForce(activePoint.forward * mprojectileForce, ForceMode.Impulse);
                }
            }

            misLeftTurn = !misLeftTurn;
        }
    }
}

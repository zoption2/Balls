using UnityEngine;

namespace TheGame
{
    public class TargetingRay : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        private bool isEnabled;

        private void Start()
        {
            line.positionCount = 3;
        }

        public void Targeting()
        {
            if (isEnabled)
            {
                line.SetPosition(0, transform.position);

                var hit = Physics2D.Raycast(transform.position, transform.up);
                line.SetPosition(1, hit.point);
                var dirVector = hit.point - (Vector2)transform.position;
                var reflected = Vector2.Reflect(dirVector, hit.normal);
                var secondHit = Physics2D.Raycast((hit.point - dirVector * 0.01f), reflected);
                line.SetPosition(2, secondHit.point);
            }
        }

        public void EnableRay()
        {
            line.enabled = true;
            isEnabled = true;
            Targeting();
        }

        public void DisableRay()
        {
            line.enabled = false;
            isEnabled = false;
        }
    }
}


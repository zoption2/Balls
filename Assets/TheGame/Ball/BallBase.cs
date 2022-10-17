using UnityEngine;
using Zenject;

namespace TheGame
{
    public abstract class BallBase : MonoBehaviour, IPoolableBalls, IGlobalFixedUpdateObserver
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float speed = 2f;
        [Inject] private IGlobalFixedUpdateProvider fixedUpdateProvider;

        public abstract BallType BallType { get;}
        private Vector2 velocity;
        private Transform originParent;
        protected int hitsCount;
        protected IIdentifiers identifiers;
        protected bool isLaunched;

        protected abstract InteractionArgs InteractionArgs { get; }


        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(InteractionArgs);
                //rb.velocity = Vector2.zero;
                //fixedUpdateProvider.Remove(this);
                //return;
            }
            rb.velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        }

        public void FixedUpdateGlobal()
        {
            rb.velocity = rb.velocity.normalized * speed;
            velocity = rb.velocity;
        }

        public virtual void OnCreate()
        {
            _collider.enabled = false;
            originParent = transform.parent;
        }

        public virtual void OnRestore()
        {
            _collider.enabled = false;
            originParent = transform.parent;
        }

        public virtual void OnStore()
        {
            if (isLaunched)
            {
                fixedUpdateProvider.Remove(this);
            }

            transform.parent = originParent;
        }

        public void Launch(IIdentifiers identifiers, Vector2 direction)
        {
            this.identifiers = identifiers;
            fixedUpdateProvider.Add(this);
            isLaunched = true;
            rb.velocity = direction * speed;
            _collider.enabled = true;
        }
    }

    public enum BallType
    {
        simple
    }
}


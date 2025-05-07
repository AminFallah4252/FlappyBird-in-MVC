using System;
using DG.Tweening;
using Interfaces;
using Models;
using Signals;
using UnityEngine;
using Zenject;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class BirdController : MonoBehaviour, IBirdController
    {
        [Inject] private SignalBus signalBus;
        [Inject] private BirdModel model;

        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            var tempGravity = rb.gravityScale;
            DOVirtual.Float(0, tempGravity, 0.5f, (g) => { rb.gravityScale = g; });
            signalBus.Subscribe<PlayerFlapSignal>(OnPlayerFlap);
        }

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, model.GetRotation(rb.linearVelocityY));
        }

        private void OnDestroy()
        {
            signalBus.TryUnsubscribe<PlayerFlapSignal>(OnPlayerFlap);
        }

        public GameObject GetGameObject() => gameObject;

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
            transform.position = Vector3.zero;
            transform.SetAsFirstSibling();
        }

        public void OnPlayerFlap()
        {
            rb.linearVelocity = model.GetJumpVelocity();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            signalBus.Fire<GameOverSignal>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            signalBus.Fire<AddScoreSignal>();
        }

        public class Factory : PlaceholderFactory<BirdController>
        {
        }
    }
}
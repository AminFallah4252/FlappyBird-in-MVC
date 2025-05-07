using System.Collections;
using Controllers;
using Interfaces;
using Models;
using NUnit.Framework;
using Signals;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class BirdControllerTests : ZenjectIntegrationTestFixture
{
    private GameObject birdGO;
    private BirdController birdController;
    private SignalBus signalBus;
    private BirdModel model;

    [UnitySetUp]
    public IEnumerator Given_BirdControllerSetup_When_Initializing_Then_PrepareAllDependencies()
    {
        PreInstall();

        // Setup SignalBus and dependencies
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<AddScoreSignal>();
        Container.DeclareSignal<GameOverSignal>();
        Container.DeclareSignal<PlayerFlapSignal>();

        model = ScriptableObject.CreateInstance<BirdModel>();
        Container.Bind<BirdModel>().FromInstance(model);

        // Create a new GameObject for the BirdController and add necessary components
        birdGO = new GameObject("Bird", typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(BirdController));
        birdGO.transform.position = Vector3.zero;

        // Bind BirdController as a single instance for both BirdController and IBirdController
        Container.Bind<BirdController>().FromComponentOn(birdGO);

        PostInstall();

        // Resolve dependencies
        signalBus = Container.Resolve<SignalBus>();
        birdController = Container.Resolve<BirdController>();

        yield return null;
    }

    [UnityTest]
    public IEnumerator Given_BirdIsIdle_When_PlayerFlaps_Then_LinearVelocityIncreases()
    {
        var rb = birdGO.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        float before = rb.linearVelocity.y;

        signalBus.Fire(new PlayerFlapSignal());
        yield return null;

        float after = rb.linearVelocity.y;
        Assert.Greater(after, before);
    }

    [UnityTest]
    public IEnumerator Given_BirdExists_When_SetParentCalled_Then_ParentAndPositionUpdated()
    {
        var parent = new GameObject("Parent").transform;

        birdController.SetParent(parent);
        yield return null;

        Assert.AreEqual(parent, birdGO.transform.parent);
        Assert.AreEqual(Vector3.zero, birdGO.transform.localPosition);
    }

    [UnityTest]
    public IEnumerator Given_BirdHitsTrigger_When_OnTriggerEnterFires_Then_AddScoreSignalSent()
    {
        bool signalFired = false;
        signalBus.Subscribe<AddScoreSignal>(() => signalFired = true);

        var triggerGO = new GameObject("Trigger", typeof(BoxCollider2D));
        triggerGO.GetComponent<BoxCollider2D>().isTrigger = true;
        triggerGO.transform.position = birdGO.transform.position;

        birdGO.AddComponent<Rigidbody2D>().gravityScale = 0;
        birdGO.GetComponent<CapsuleCollider2D>().isTrigger = false;

        yield return null;
        yield return new WaitForFixedUpdate();

        birdGO.transform.position = triggerGO.transform.position;

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(signalFired);
    }

    [UnityTest]
    public IEnumerator Given_BirdHitsCollider_When_OnCollisionEnterFires_Then_GameOverSignalSent()
    {
        bool signalFired = false;
        signalBus.Subscribe<GameOverSignal>(() => signalFired = true);

        var wall = new GameObject("Wall", typeof(BoxCollider2D));
        wall.transform.position = birdGO.transform.position;

        var rb = birdGO.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        birdGO.GetComponent<CapsuleCollider2D>().isTrigger = false;

        yield return null;
        yield return new WaitForFixedUpdate();

        birdGO.transform.position = wall.transform.position;

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(signalFired);
    }
}

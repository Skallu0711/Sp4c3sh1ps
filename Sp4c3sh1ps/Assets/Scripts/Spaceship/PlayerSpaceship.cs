﻿using System;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

public sealed class PlayerSpaceship : Spaceship
{
    public static PlayerSpaceship Instance { get; private set; }
    
    [ReadOnlyInspector] public bool isInvincible = false;
    private const float SPEED_INCREASE_PER_POINT = 0.025f;

    public static event EventHandler Died;

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            
            base.Awake();
        }
    }

    private void OnEnable() => Point.Collected += OnCollectedPoint;

    private void OnDisable() => Point.Collected -= OnCollectedPoint;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemySpaceship") && !isInvincible)
            Die();
    }

    private void OnCollectedPoint(object sender, EventArgs e)
    {
        var speedIncrease = isSlowedDown ? SPEED_INCREASE_PER_POINT * 0.5f : SPEED_INCREASE_PER_POINT;

        MovementSpeed += speedIncrease;
        RotationSpeed += speedIncrease;
    }

    public override void Die()
    {
        base.Die();
        
        Died?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

}
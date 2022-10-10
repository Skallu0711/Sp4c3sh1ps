﻿using System;

public class Point : Collectable
{
    public static event EventHandler Collected;

    protected override void GetCollected()
    {
        base.GetCollected();
        Collected?.Invoke(this, EventArgs.Empty);
    }
    
}
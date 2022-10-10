﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Self { get; private set; }
    
    [SerializeField] private float _wrapOffset = 0.2f;
    private Camera _mainCam;
    public BoxCollider2D BoundingBox { get; private set; }

    private void Awake()
    {
        if (Self != null && Self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Self = this;
            
            _mainCam = Camera.main;
            BoundingBox = GetComponent<BoxCollider2D>();
            BoundingBox.isTrigger = true; // bounding box has to be trigger
        }
    }

    private void Start()
    {
        transform.position = Vector2.zero;
        
        // set bounds size
        var y = _mainCam.orthographicSize * 2;
        BoundingBox.size = new Vector2(y * _mainCam.aspect, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRandomScreenPosition()
    {
        var bBox = BoundingBox.size;
        return new Vector2(Random.Range(-bBox.x * 0.5f, bBox.x * 0.5f), Random.Range(-bBox.y * 0.5f, bBox.y * 0.5f));
    }

    /// <summary>
    /// Checks if position is out of bounds
    /// </summary>
    /// <param name="worldPos"> world position to check if it is out of screen bounds </param>
    /// <returns> bool value that determines whether provided position is out of screen bounds </returns>
    public bool IsOutOfBounds(Vector3 worldPos)
    {
        return Mathf.Abs(worldPos.x) > Mathf.Abs(BoundingBox.bounds.min.x)
               || Mathf.Abs(worldPos.y) > Mathf.Abs(BoundingBox.bounds.min.y);
    }

    /// <summary>
    /// Calculate wrapped position of an object
    /// </summary>
    /// <param name="tr"> transform used to calculate its wrapped position </param>
    /// <returns> wrapped position </returns>
    public Vector2 CalculateWrappedPosition(Transform tr)
    {
        var warpedPosition = (Vector2)(tr.position - tr.up * 20);
        var div = 1f;

        if (Mathf.Abs(warpedPosition.x) > Mathf.Abs(warpedPosition.y))
        {
            if (Mathf.Abs(warpedPosition.x) > BoundingBox.size.x * 0.5f)
            {
                div = (BoundingBox.size.x * 0.5f) / Mathf.Abs(warpedPosition.x);
                warpedPosition *= div;
            }
            if (Mathf.Abs(warpedPosition.y) > BoundingBox.size.y * 0.5f)
            {
                div = (BoundingBox.size.y * 0.5f) / Mathf.Abs(warpedPosition.y);
                warpedPosition *= div;
            }
        }
        else
        {
            if (Mathf.Abs(warpedPosition.y) > BoundingBox.size.y * 0.5f)
            {
                div = (BoundingBox.size.y * 0.5f) / Mathf.Abs(warpedPosition.y);
                warpedPosition *= div;
            }
            if (Mathf.Abs(warpedPosition.x) > BoundingBox.size.x * 0.5f)
            {
                div = (BoundingBox.size.x * 0.5f) / Mathf.Abs(warpedPosition.x);
                warpedPosition *= div;
            }
        }
        
        return warpedPosition + (Vector2)(tr.up * _wrapOffset);
    }

} 
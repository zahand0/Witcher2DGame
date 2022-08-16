using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HidingObj : MonoBehaviour
{
    void Start()
    {
        TilemapRenderer[] renderers = FindObjectsOfType<TilemapRenderer>();
        foreach(TilemapRenderer renderer in renderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }
        SpriteRenderer[] spRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer renderer in spRenderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }
    }

    void Update()
    {
        SpriteRenderer[] spRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer renderer in spRenderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }
    }
}

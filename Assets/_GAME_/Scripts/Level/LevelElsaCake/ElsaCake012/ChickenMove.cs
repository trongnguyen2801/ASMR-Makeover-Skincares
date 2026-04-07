using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chicken : BaseOb
{
    public GameObject eggPrefab;
    public Transform parent;
    public Transform eggSpawnOffset;
    public bool isStaticChicken;
    public bool hasLaidEgg = false;

    protected override void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (hasLaidEgg || eggPrefab == null) return;
        if (lvCtrl != null) lvCtrl.curBaseOb = this;
        // AudioManager.Instance.PlaySFX(AudioClipId.ChickenScary);
        hasLaidEgg = true;

        Vector3 spawnPos = eggSpawnOffset != null ? eggSpawnOffset.position : transform.position;
        eggPrefab.transform.position = spawnPos;
        eggPrefab.SetActive(true);

        Vector3 targetWorldPos = GetTargetWithinScreen(spawnPos);

        Vector3 controlPoint = (spawnPos + targetWorldPos) / 2 + Vector3.up * 2f;

        eggPrefab.transform.DOPath(new Vector3[] { spawnPos, controlPoint, targetWorldPos }, 1f, PathType.CatmullRom)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                var spEgg = eggPrefab.GetComponent<SpriteRenderer>();
            });

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    private Vector3 GetTargetWithinScreen(Vector3 origin)
    {
        Vector3 screenOrigin = Camera.main.WorldToScreenPoint(origin);

        float minX = 100f;
        float maxX = Screen.width - 100f;
        float minY = 100f;
        float maxY = Screen.height - 200f;

        float targetX = Mathf.Clamp(screenOrigin.x + (isStaticChicken ? 0 : Random.Range(-200f, 200f)), minX, maxX);
        float targetY = Mathf.Clamp(screenOrigin.y + Random.Range(-100f, 100f), minY, maxY);

        Vector3 targetScreen = new Vector3(targetX, targetY, screenOrigin.z);
        return Camera.main.ScreenToWorldPoint(targetScreen);
    }

    protected override void OnMouseUp() { }

    protected override void OnMouseDrag() { }
}
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    protected UIManager UIManager;
    public bool isShowing = false;

    protected virtual void Start()
    {

    }

    protected void OnDestroy()
    {

    }

    public virtual void Initialize(UIManager UI)
    {
        UIManager = UI;
    }

    public virtual void Active()
    {
        gameObject?.SetActive(true);
        isShowing = true;
       // AdvertisementManager.Instance.ShowBannerAd();
    }

    public virtual void DeActive()
    {
        isShowing = false;
        gameObject.SetActive(false);
    }

    public virtual void OnGameStateChanged(GameState newState)
    {
    }
}
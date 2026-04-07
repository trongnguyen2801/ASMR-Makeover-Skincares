using UnityEngine;

public class OpenCloseOb : BaseOb
{
    public AudioClip openSound;
    public AudioClip closeSound;
    public bool isOpen;
    protected override void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;

        if (!isOpen)
            Open();
        else
            Close();
    }

    protected override void OnMouseDrag()
    {
    }

    protected override void OnMouseUp()
    {
    }

    public virtual void Open()
    {
        isOpen = true;
        // if(openSound != null)
        // AudioManager.Instance.PlaySFX(openSound);
        SetCanUseWithCol(false);
    }

    public virtual void Close()
    {
        isOpen = false;
        // if (closeSound != null)
        // AudioManager.Instance.PlaySFX(closeSound);
        SetCanUseWithCol(false);
    }
}

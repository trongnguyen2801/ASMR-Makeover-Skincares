using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadOfMachineBlend : MonoBehaviour
{
    [SerializeField] protected ToolMachineMixer tool;
    [SerializeField] protected GameObject fxPartical;
    [SerializeField] protected AudioClip soundUsing;
    public bool inBowl;
    public bool isHeadCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AreaEffect") && isHeadCheck)
        {
            Debug.Log("trigger bowl");
            // var clip = AudioManager.Instance.GetAudioClip(AudioClipId.BlendMachine);
            // AudioManager.Instance.PlayMusic(AudioManager.Instance.effectsSourceTool, clip, true);

            foreach (var sp in tool.liSprRend)
            {
                sp.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (tool == null) return;
        if (!tool.lvCtrl.isUsingCorrectTool) return;
        if (collision.CompareTag("AreaEffect"))
        {
            fxPartical?.SetActive(true);
            if (tool.isDragging)
            {
                tool.timeSpentInArea += Time.deltaTime;
                tool.ChangeAnim("Using");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AreaEffect") && isHeadCheck)
        {
            inBowl = false;
            Debug.Log("check trigger 123456");
            tool.ChangeAnim("ChangeImage");
            foreach (var sp in tool.liSprRend)
            {
                sp.maskInteraction = SpriteMaskInteraction.None;
            }
        }
        // AudioManager.Instance.StopSound(AudioManager.Instance.effectsSourceTool);
        //AudioManager.Instance.StopEffect();
        fxPartical?.SetActive(false);
    }
}

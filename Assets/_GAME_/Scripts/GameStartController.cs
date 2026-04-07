using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BB
{
    public class GameStartController : MonoBehaviour
    {
        public Button btnPlay;
        public Button btnPolicy;
        public Slider loading;
        public Transform nativeAdParent;

        void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            // init dotween
            // DOTween.Init(true, true, LogBehaviour.Default);
            // DOTween.SetTweensCapacity(3000, 1000);


            // load playerdata
            Model.Instance.Load();

            // init game data
        }

        void Start()
        {
            //TODO: hardcode for testing
            //  PlayerData.current.curLevelId = 10;

            loading.value = 0.1f;
            StartCoroutine(DoLoading(1.0f));
        }

        private void ActiveButtons()
        {
            loading?.gameObject.SetActive(false);
            btnPlay?.gameObject.SetActive(true);
            btnPolicy?.gameObject.SetActive(true);
            btnPlay.transform.localScale = Vector3.zero;
            btnPolicy.transform.localScale = Vector3.zero;
            btnPlay.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f);
            btnPolicy.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
        }

        private IEnumerator DoLoading(float time)
        {
            float curTime = 0;
            float delta = time / 100;
            while (loading.value < 1.0f)
            {
                loading.value += 1 / 110f;
                yield return new WaitForSecondsRealtime(delta);
            }

            loading.value = 1.0f;
            yield return new WaitForSecondsRealtime(1.0f);
            ActiveButtons();
            yield return null;
        }

        public void Play()
        {
            //Load home or game play scene
            LoadSceneUtility.LoadScene(LoadSceneUtility.HomeSceneName);
        }

        public void OpenPolicy()
        {
            Application.OpenURL("https://bbold.vn/policy.html");
        }
    }
}
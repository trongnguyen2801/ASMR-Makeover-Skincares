using System;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.Level.LevelWatermelon
{
    public class FaceMaskArrangeOb : ArrangeOb
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        
        public override void Init(LevelStateCtrl levelCtrl)
        {
            base.Init(levelCtrl);
            posDefault.position = this.transform.position;
        }
        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            _spriteRenderer.enabled = true;
        }
        
        public override void MoveInMultiplePlaces(bool canPlacesManyTime = false, float timeMoving = 0.2f,
            Action action = null)
        {
            if (posCorrect == null) posCorrect = posCorrects[0];
            Vector3 pos = posCorrect.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, posCorrect.position.z);
            transform.DOMove(posCorrect.position, timeMoving).SetEase(Ease.InOutQuad);

            Vector3 scale = posCorrect.localScale;
            transform.localScale = scale;

            Quaternion ro = posCorrect.rotation;
            transform.localRotation = ro;

            if (!canPlacesManyTime)
            {
                col.enabled = false;
                isDragging = false;
            }

            isArranged = true;
            posCorrect.gameObject.SetActive(false);
            SetDefaultLayer();

            if (action != null) action?.Invoke();
        }

        public override void MoveBack(bool isMoving = true)
        {
            base.MoveBack(isMoving);
            _spriteRenderer.enabled = false;
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            // _spriteRenderer.enabled = false;
        }
    }
}
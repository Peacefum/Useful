using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Peace.Coin
{
    public class CoinAnim : MonoBehaviour
    {
        Tween coinMoveTarget;
        Tween createCoin;
        Tween coinmove;
        Tween coinJump;
        Tween coindisable;
        private Vector3 screenToWorldPosition = Vector3.zero;
        float randomX = 0;
        Vector3 moveVec = Vector3.zero;
        Sequence mySequence;
        CoinAnimManager coinmanager;
        Vector3 myPosition;
        private float coinSize = 0.2f;
        private void Start()
        {
            myPosition = transform.localPosition;
            coinmanager = transform.parent.GetComponent<CoinAnimManager>();
          //  screenToWorldPosition = coinmanager.getPos;
            mySequence = DOTween.Sequence();

            createCoin = DOTween.To(() => transform.localScale, x => transform.localScale = x, Vector3.one * coinSize, 0.5f).SetEase(Ease.OutBounce);
            coinmove = transform.DOLocalMoveX(randomX, 0.5f);
            // coinJump = transform.DOLocalJump((Vector3.up * 1f), 1, 2, 0.5f);
            coinJump = transform.DOLocalJump((Vector3.up * 1f), 1, 2, 0.5f).Insert(0.2f, transform.DOLocalMoveY(-2, 0.3f));//.OnComplete(() => { transform.DOLocalMoveY(-1f, 0.5f); });

            coinMoveTarget = DOTween.To(() => transform.position, x => transform.position = x, screenToWorldPosition, 0.4f).SetDelay(0.1f);
            coindisable = DOTween.To(() => transform.localScale, x => transform.localScale = x, Vector3.one * 0.1f, 0.5f);

            mySequence.Append(createCoin).Join(coinJump).Join(coinmove).Append(coinMoveTarget).Insert(0.65f, coindisable).OnComplete(() => {
               // coinmanager.CoinDeQueue(gameObject);
                gameObject.SetActive(false);
            });
            mySequence.SetAutoKill(false);
        }
        private void OnEnable()
        {
            randomX = Random.Range(-1.5f, 1.5f);
            moveVec.x = randomX;
            mySequence.Restart();
        }
        private void OnDisable()
        {
            mySequence.Pause();
            transform.localPosition = Vector3.zero;
        }


    }

}

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Source.MainMenu.Scripts.Animations.AnimationStarters
{
    public class KeyAnimation : MonoBehaviour, IAnimationStarter
    {
        [SerializeField] private Image[] _images;
        [SerializeField] private Vector2 _maxOffset;
        [SerializeField] private float _transparencyDuration;
        [SerializeField] private float _shakeStrength;
        [SerializeField] private Transform _jumpTarget;
        [FormerlySerializedAs("_jumpPowwer")] [FormerlySerializedAs("_jumpForce")] [SerializeField] private float _jumpPower;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _timeBetweenKeys;
        
        private float _normalTransparency = 1;
        private Vector3 _startPosition;
        private WaitForSeconds _waitNextKey; 
        private int _activeImagesCount = 0;
        private float _minTransparency = 0;
        private int _numJumps = 1;
        
        public event UnityAction AnimationEnded;
        
        public void Init()
        {
            _startPosition = transform.position;
            _waitNextKey = new WaitForSeconds(_timeBetweenKeys);

            foreach (var image in _images)
                image.enabled = false;
        }

        public void StartAnimation()
        {
            StartCoroutine(StartAnimationPerTime());
        }

        private IEnumerator StartAnimationPerTime()
        {
            foreach (var image in _images)
            {
                image.enabled = true;
                image.color = new Color(image.color.r, image.color.g, image.color.b, _minTransparency);
                var randomOffset = new Vector3(Random.Range(-_maxOffset.x, _maxOffset.x), Random.Range(-_maxOffset.y, _maxOffset.y));
                image.transform.position = _startPosition + randomOffset;
                _activeImagesCount++;
                
                MakeAnimation(image);
                yield return _waitNextKey;
            }
        }

        private void MakeAnimation(Image image)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(image.DOFade(_normalTransparency, _transparencyDuration));
            sequence.Join(image.transform.DOShakeScale(_transparencyDuration, _shakeStrength));
            sequence.Append(image.transform.DOJump(_jumpTarget.position, _jumpPower, _numJumps, _jumpDuration));
            sequence.OnComplete(() =>
            {
                image.enabled = false;
                OnAnimationEnded();
            });
        }

        private void OnAnimationEnded()
        {
            _activeImagesCount--;

            if (_activeImagesCount <= 0)
            {
                _activeImagesCount = 0;
                AnimationEnded?.Invoke();
            }
        }
    }
}
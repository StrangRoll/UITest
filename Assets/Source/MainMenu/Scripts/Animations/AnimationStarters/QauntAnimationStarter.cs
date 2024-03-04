using System.Collections;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Source.MainMenu.Scripts.Animations.AnimationStarters
{
    public class QauntAnimationStarter : MonoBehaviour, IAnimationStarter
    {
        [SerializeField] private Image[] _images;
        [SerializeField] private float _timeBetweenQuants;
        [SerializeField] private Vector2 _maxOffset;
        [SerializeField] private float _transparencyDuration;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Transform _moveTarget;
        
        private Vector3 _startPosition;
        private WaitForSeconds _waitNextQuant; 
        private float _normalTransparency = 1;
        private float _minTransparency = 0;
        private int  _activeImagesCount = 0;
        private float _normalScale = 1;
        
        public event UnityAction AnimationEnded;
        
        public void Init()
        {
            _startPosition = transform.position;
            _waitNextQuant = new WaitForSeconds(_timeBetweenQuants);

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
                image.transform.localScale = Vector3.zero;
                var randomOffset = new Vector3(Random.Range(-_maxOffset.x, _maxOffset.x), Random.Range(-_maxOffset.y, _maxOffset.y));
                image.transform.position = _startPosition + randomOffset;
                _activeImagesCount++;
                
                MakeAnimation(image);
                yield return _waitNextQuant;
            }
        }

        private void MakeAnimation(Image image)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(image.DOFade(_normalTransparency, _transparencyDuration));
            sequence.Join(image.transform.DOScale(_normalScale, _transparencyDuration));
            sequence.Append(image.transform.DOMove(_moveTarget.position, _moveDuration));
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
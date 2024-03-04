using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Source.MainMenu.Scripts.Animations.AnimationStarters
{
    public class StarAnimationStarter : MonoBehaviour, IAnimationStarter
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _increaseDuration;
        [SerializeField] private float _normalScale;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Transform _wayTarget;
        [SerializeField] private Vector3 _wayPoint;
        [SerializeField] private float _finalScale;
        
        private Sequence _sequence;
        private int _numJumps = 1;
        private Vector3 _fullRotate = new Vector3(0, 0, -360);
        private Vector3 _startPosition;
        
        public event UnityAction AnimationEnded;

        private void Start()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(_normalScale, _increaseDuration).SetEase(Ease.OutElastic));
            
            var path = new Vector3[] { transform.position + _wayPoint, _wayTarget.position };

            _sequence.Append(transform.DOPath(path, _moveDuration, PathType.CatmullRom).SetEase(Ease.InCirc));
            _sequence.Join(transform.DOLocalRotate(_fullRotate, _moveDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));
            _sequence.Join(transform.DOScale(_finalScale, _moveDuration).SetEase(Ease.InCirc));
            _sequence.OnComplete(OnAnimationEnded);
            _sequence.SetAutoKill(false);
            _sequence.Pause();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        public void Init()
        {
            transform.localScale = Vector3.zero;
            _startPosition = transform.position;
        }


        public void StartAnimation()
        {
            transform.position = _startPosition;
            _image.enabled = true;
            _sequence.Restart();
        }

        private void OnAnimationEnded()
        {
            AnimationEnded?.Invoke();
            _image.enabled = false;
            transform.localScale = Vector3.zero;
        }
    }
}
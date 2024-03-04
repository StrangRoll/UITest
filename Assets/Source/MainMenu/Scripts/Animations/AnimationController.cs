using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Source.MainMenu.Scripts.Animations.AnimationStarters;
using UnityEngine;
using UnityEngine.Events;

namespace Source.MainMenu.Scripts.Animations
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private ButtonClickReader _startAnimationButton;
        [SerializeField] private List<InterfaceReference<IAnimationStarter>> _animationStarters; 
            
        private void OnEnable()
        {
            _startAnimationButton.ButtonCliked += OnAnimationButtonClick;
        }

        private void Start()
        {
            foreach (var animationStarter in _animationStarters)
                animationStarter.Value.Init();
        }

        private void OnDisable()
        {
            _startAnimationButton.ButtonCliked -= OnAnimationButtonClick;
        }

        private void OnAnimationButtonClick()
        {
            StartCoroutine(StartingAnimations());
        }

        private IEnumerator StartingAnimations()
        {
            foreach (var animationStarter in _animationStarters)
            {
                animationStarter.Value.StartAnimation();
                var animationEnded = false;
                UnityAction handler = null;
                
                handler = () =>
                {
                    animationEnded = true;
                    animationStarter.Value.AnimationEnded -= handler;
                };

                animationStarter.Value.AnimationEnded += handler;
                yield return new WaitUntil(() => animationEnded);
            }
        }
    }
}
using UnityEngine.Events;

namespace Source.MainMenu.Scripts.Animations.AnimationStarters
{
    public interface IAnimationStarter 
    {
        public event UnityAction AnimationEnded;

        public void Init();
        public void StartAnimation();
    }
}
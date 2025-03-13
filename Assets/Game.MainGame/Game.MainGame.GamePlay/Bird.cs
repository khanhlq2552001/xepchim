using Spine.Unity;
using UnityEngine;

namespace Game.MainGame
{
    public enum StateAnim
    {
        idle,
        fly,
        grounding,
        touching
    }

    public class Bird : MonoBehaviour
    {
        [SerializeField] private int _id;

        private SkeletonAnimation _skeleAnim;
        private StateAnim _stateAnim;

        private void Awake()
        {
            _skeleAnim = GetComponent<SkeletonAnimation>();
        }

        public int ID
        {
            get => _id;

            set => _id = value;
        }

        public void ChangStateAnim(StateAnim state)
        {
            switch (state)
            {
                case StateAnim.idle:
                    _skeleAnim.AnimationState.SetAnimation(0, "idle", true);
                    break;
                case StateAnim.fly:
                    _skeleAnim.AnimationState.SetAnimation(0, "fly", true);
                    break;
                case StateAnim.touching:
                    _skeleAnim.AnimationState.SetAnimation(0, "touching", true);
                    break;
                case StateAnim.grounding:
                    _skeleAnim.AnimationState.SetAnimation(0, "grounding", false);
                    _skeleAnim.AnimationState.AddAnimation(0, "idle", true, 0);
                    break;
            }
        }
    }
}

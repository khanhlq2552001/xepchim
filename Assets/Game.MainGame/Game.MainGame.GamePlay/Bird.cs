using System.Collections;
using DG.Tweening;
using Lean.Pool;
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
        [SerializeField] private float _maxSpeed = 5f; // Tốc độ tối đa
        [SerializeField] private float _minSpeed = 5f; // Tốc độ tối đa
        [SerializeField] private float _acceleration = 2f; // Gia tốc

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

        public void MovingToBranch(Transform target1, Wood woodStart, Wood woodEnd, bool isCheck)
        {
            StartCoroutine(MoveTowardsBranchCoroutine(target1, woodStart, woodEnd, isCheck));
        }

        public void MovingToBranchStart(Transform target1, Wood wood)
        {
            StartCoroutine(MovingStartCoroutine(target1, wood));
        }

        public void MovingToEnd(Vector2 target1)
        {
            StartCoroutine(MovingEndCoroutine(target1));
        }

        IEnumerator MoveTowardsBranchCoroutine(Transform target1, Wood woodStart, Wood woodEnd, bool isCheck)
        {
            if(target1.position.x > transform.position.x) transform.eulerAngles = new Vector3(0, 180, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);

            woodStart.Rung();
            transform.SetParent(woodEnd.tranCha);
            Vector3 posTarget1 = new Vector3(target1.position.x, target1.position.y + 0.2f, 0);

            woodEnd.CanChoose = false;
            ChangStateAnim(StateAnim.fly);

            float speed = 0f; // Bắt đầu với tốc độ = 0
            Vector3 startPosition = transform.position;
            float distance = Vector3.Distance(startPosition, posTarget1);

            while (Vector3.Distance(transform.position, posTarget1) > 1f)
            {
                speed += _acceleration * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Min(speed, _maxSpeed);

                transform.position = Vector3.MoveTowards(transform.position, posTarget1, speed * Time.deltaTime);

                yield return null;
            }

            while (Vector3.Distance(transform.position, posTarget1) > 0.1f)
            {
                speed -= _acceleration * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Max(speed, _minSpeed);

                transform.position = Vector3.MoveTowards(transform.position, posTarget1, speed * Time.deltaTime);

                yield return null;
            }

            transform.position = posTarget1;

            if(woodEnd.Type == TypeWord.left) transform.eulerAngles = new Vector3(0, 180, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);

            speed = 0f;
            ChangStateAnim(StateAnim.grounding);

            while (Vector3.Distance(transform.position, target1.position) > 0.1)
            {
                speed += 3 * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Min(speed, _maxSpeed);

                transform.position = Vector3.MoveTowards(transform.position, target1.position, speed * Time.deltaTime);

                yield return null;
            }
            woodEnd.Rung();

            transform.position = target1.position;

            ChangStateAnim(StateAnim.idle);
            woodEnd.CanChoose = true;

            if (isCheck)
            {
                woodEnd.CheckBirds();
            }
        }

        IEnumerator MovingStartCoroutine(Transform target1, Wood wood)
        {
            if (target1.position.x > transform.position.x) transform.eulerAngles = new Vector3(0, 180, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);

            transform.SetParent(wood.tranCha);
            Vector3 posTarget1 = new Vector3(target1.position.x, target1.position.y + 0.2f, 0);

            wood.CanChoose = false;
            ChangStateAnim(StateAnim.fly);

            float speed = 0f; // Bắt đầu với tốc độ = 0
            Vector3 startPosition = transform.position;
            float distance = Vector3.Distance(startPosition, posTarget1);

            while (Vector3.Distance(transform.position, posTarget1) > 1f)
            {
                speed += _acceleration * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Min(speed, _maxSpeed);

                transform.position = Vector3.MoveTowards(transform.position, posTarget1, speed * Time.deltaTime);

                yield return null;
            }

            while (Vector3.Distance(transform.position, posTarget1) > 0.1f)
            {
                speed -= _acceleration * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Max(speed, _minSpeed);

                transform.position = Vector3.MoveTowards(transform.position, posTarget1, speed * Time.deltaTime);

                yield return null;
            }

            transform.position = posTarget1;

            if (wood.Type == TypeWord.left) transform.eulerAngles = new Vector3(0, 180, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);

            speed = 0f;
            ChangStateAnim(StateAnim.grounding);

            while (Vector3.Distance(transform.position, target1.position) > 0.1)
            {
                speed += 3 * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Min(speed, _maxSpeed);

                transform.position = Vector3.MoveTowards(transform.position, target1.position, speed * Time.deltaTime);

                yield return null;
            }

            transform.position = target1.position;

            ChangStateAnim(StateAnim.idle);
            wood.CanChoose = true;
        }

        IEnumerator MovingEndCoroutine(Vector3 target1)
        {
            if (target1.x > transform.position.x) transform.eulerAngles = new Vector3(0, 180, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);

            Vector3 posTarget1 = new Vector3(target1.x, target1.y, 0);

            ChangStateAnim(StateAnim.fly);

            float speed = 0f; // Bắt đầu với tốc độ = 0
            Vector3 startPosition = transform.position;

            while (Vector3.Distance(transform.position, posTarget1) > 0.1f)
            {
                speed += _acceleration * Time.deltaTime; // Tăng tốc dần dần

                speed = Mathf.Min(speed, _maxSpeed);

                transform.position = Vector3.MoveTowards(transform.position, posTarget1, speed * Time.deltaTime);

                yield return null;
            }

            transform.position = posTarget1;

            LeanPool.Despawn(gameObject);
        }
    }
}

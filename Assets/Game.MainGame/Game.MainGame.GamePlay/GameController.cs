using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    public enum StateController
    {
        Pause,
        Choose,
        DontChoose
    }

    public class GameController : MonoBehaviour
    {
        [SerializeField] private Transform _tranGamePlay;

        private List<Bird> _birdsChoose =  new List<Bird>();
        private StateController _stateController;
        private Wood _woodChoose;

        private void Awake()
        {
            _stateController = StateController.DontChoose;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                    if (hit.collider != null)
                    {
                        CheckHit(hit);
                    }
                }
            }
        }

        private void CheckHit(RaycastHit2D hit)
        {
            if (hit.collider.gameObject.CompareTag("wood"))
            {
                Wood wood = hit.collider.GetComponent<Wood>();

                if (!wood.CanChoose)
                {
                    return;
                }

                if(State == StateController.DontChoose)
                {
                    _birdsChoose = wood.GetBirds();

                    if (_birdsChoose.Count == 0)
                    {
                        return;
                    }
                    _woodChoose = wood;
                    State = StateController.Choose;

                    for (int i=0; i< _birdsChoose.Count; i++)
                    {
                        _birdsChoose[i].ChangStateAnim(StateAnim.touching);
                    }

                    return;
                }

                if(State == StateController.Choose)
                {
                    if(_woodChoose == wood)
                    {
                        ClearBirdChoose();
                        State = StateController.DontChoose;
                        return;
                    }
                    else
                    {
                        Debug.Log("bbb");
                        List<Transform> listTran = new List<Transform>();
                        listTran = wood.GetSlots(_birdsChoose[0].ID, _birdsChoose.Count);

                        if (listTran.Count ==0) // neu nhu khong con slot
                        {
                            Debug.Log("aaaa");
                            ClearBirdChoose();
                            CheckGetBirds(wood);
                            return;
                        }
                        else
                        {
                            MovingBird(listTran, wood);
                        }
                    }
                }
            }
        }

        private void MovingBird(List<Transform> listTran, Wood wood)
        {
            
            for(int i = 0; i < _birdsChoose.Count ; i++)
            {
                if(i < listTran.Count)
                {
                    if(i < listTran.Count -1) _birdsChoose[i].MovingToBranch(listTran[i], _woodChoose, wood, false);
                    else _birdsChoose[i].MovingToBranch(listTran[i], _woodChoose, wood, true);

                    wood.AddBird(_birdsChoose[i]);
                    _woodChoose.RemoveBird();
                }
                else
                {
                    _birdsChoose[i].ChangStateAnim(StateAnim.idle);
                }
            }
            State = StateController.DontChoose;
        }

        private void CheckGetBirds(Wood wood)
        {
            _birdsChoose = wood.GetBirds();

            if (_birdsChoose.Count == 0)
            {
                State = StateController.DontChoose;
                return;
            }
            _woodChoose = wood;
            State = StateController.Choose;

            for (int i = 0; i < _birdsChoose.Count; i++)
            {
                _birdsChoose[i].ChangStateAnim(StateAnim.touching);
            }

            return;
        }

        private void ClearBirdChoose()
        {
            for(int i=0; i< _birdsChoose.Count; i++)
            {
                _birdsChoose[i].ChangStateAnim(StateAnim.idle);
            }
            _birdsChoose.Clear();
        }

        public StateController State
        {
            get => _stateController;

            set => _stateController = value;
        }
    }
}

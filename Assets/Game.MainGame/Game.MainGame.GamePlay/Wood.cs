using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    public enum TypeWord
    {
        left,
        right
    }

    public class Wood : MonoBehaviour
    {
        [SerializeField] private Transform[] _transStopBird = new Transform[4];
        [SerializeField] private Bird[] _birdInBranchs = new Bird[4];

        private TypeWord _type;
        private bool _canChoose;
        private int _countBirds;
        private Animator _anim;

        public Transform tranCha;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public TypeWord Type
        {
            get => _type;

            set => _type = value;
        }

        public bool CanChoose
        {
            get => _canChoose;

            set => _canChoose = value;
        }

        public int CountBirds
        {
            get => _countBirds;

            set => _countBirds = value;
        }

        public Transform[] GetTranBird()
        {
            return _transStopBird;
        }

        public void SetBird(Bird bird, int id)
        {
            _birdInBranchs[id] = bird;
            CountBirds++;
        }

        public void AddBird(Bird bird)
        {
            _birdInBranchs[CountBirds] = bird;
            CountBirds++;
        }

        public void RemoveBird()
        {
            _birdInBranchs[CountBirds - 1] = null;
            CountBirds--;
        }

        public void Rung()
        {
            _anim.SetBool("isNhun", true);
        }

        public void DontRung()
        {
            _anim.SetBool("isNhun", false);
        }

        public void ResetAtribute()
        {
            CountBirds = 0;
        }

        public List<Bird> GetBirds()
        {
            List<Bird> listResult = new List<Bird>();
            int idChoose = -1;
            bool isChoose = false;

            for(int i = 3; i >=0; i--)
            {
                if (_birdInBranchs[i] != null)
                {
                    if (_birdInBranchs[i].ID == idChoose)
                    {
                        listResult.Add(_birdInBranchs[i]);
                    }
                }

                if (_birdInBranchs[i] != null && isChoose == false)
                {
                    isChoose = true;
                    listResult.Add(_birdInBranchs[i]);
                    idChoose = _birdInBranchs[i].ID;
                }

                if (_birdInBranchs[i] != null && _birdInBranchs[i].ID != idChoose) break;
            }

            return listResult;
        }

        public List<Transform> GetSlots(int idBird, int count)
        {
            List<Transform> listTran = new List<Transform>();

            bool checkTrue =false;
            int idi = -1;

            for(int i =3; i>=0; i--)
            {
                if (_birdInBranchs[i] != null)
                {
                    if (_birdInBranchs[i].ID == idBird)
                    {
                        checkTrue = true;
                        idi = i;
                    }
                    break;
                }
            }

            if ((!checkTrue && _birdInBranchs[0] != null) || idi == 3) return listTran;

            int countSlot = 0;

            for(int i= (idi +1); i < 4; i++)
            {
                if(countSlot < count)
                {
                    listTran.Add(_transStopBird[i]);
                    countSlot++;
                }
                else
                {
                    break;
                }
            }

            return listTran;
        }

        public void CheckBirds()
        {
            Vector3 pos1 = LevelManager.Instance.posStartRight;
            Vector3 pos2 = LevelManager.Instance.posStartLeft;

            if (CountBirds != 4) return;

            if (_birdInBranchs[0].ID == _birdInBranchs[1].ID &&
                _birdInBranchs[1].ID == _birdInBranchs[2].ID &&
                _birdInBranchs[2].ID == _birdInBranchs[3].ID)
            {
        

                for(int i=0; i < _birdInBranchs.Length; i++)
                {
                    if(Type == TypeWord.left)
                    {
                        Vector2 target = GetRandomPointAround(pos1, 2f);
                        _birdInBranchs[i].MovingToEnd(target);
                        _birdInBranchs[i] = null;
                    }
                    if (Type == TypeWord.right)
                    {
                        Vector2 target = GetRandomPointAround(pos2, 2f);
                        Debug.Log(target);
                        _birdInBranchs[i].MovingToEnd(target);
                        _birdInBranchs[i] = null;
                    }
                }
                CountBirds = 0;
            }
        }

        public Vector2 GetRandomPointAround(Vector2 center, float radius)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
            return center + randomOffset;
        }
    }
}

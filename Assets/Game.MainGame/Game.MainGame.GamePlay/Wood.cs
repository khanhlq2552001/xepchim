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

        public Transform[] GetTranBird()
        {
            return _transStopBird;
        }

        public void SetBird(Bird bird, int id)
        {
            _birdInBranchs[id] = bird;
        }

        public void ResetAtribute()
        {

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
            }

            return listResult;
        }

        public List<Transform> GetSlots(int idBird, int count)
        {
            List<Transform> listTran = new List<Transform>();

            bool checkTrue =false;
            for(int i =3; i>=0; i--)
            {
                if (_birdInBranchs[i] != null)
                {
                    if (_birdInBranchs[i].ID == idBird)
                    {
                        checkTrue = true;
                        break;
                    }
                }
            }

            if (!checkTrue && _birdInBranchs[0] != null) return listTran;

            return listTran;
        }
    }
}

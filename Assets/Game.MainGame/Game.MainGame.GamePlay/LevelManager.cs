using System;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

namespace Game.MainGame
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [SerializeField] private Data _data;
        [SerializeField] private DataBird _dataBirds;
        [SerializeField] private Wood _objWoodLeft;
        [SerializeField] private Wood _objWoodRight;
        [SerializeField] private float _spacingWood = 1.4f;

        public GameController controller;

        private void Awake()
        {
            if(Instance != null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            GenerateData();
        }

        public void GenerateData()
        {
            int countWord = _data.branches.Count;
            float x = (float)countWord /2;
            int countwoodLeft = (int)Math.Ceiling(x);
            int countWoodRight = countWord - countwoodLeft;

            for(int i=0; i< countwoodLeft; i++)
            {
                float y = (i - (countwoodLeft - 1) / 2.0f) * _spacingWood;
                Vector3 position = new Vector3(-2.83f, y, 0);

                Wood wood =   LeanPool.Spawn(_objWoodLeft);
                wood.transform.position = position;
                wood.Type = TypeWord.left;
                SetUpBranch(_data.branches[i].idBirds, wood);
            }

            for (int i = 0; i < countWoodRight; i++)
            {
                float y = (i - (countWoodRight - 1) / 2.0f) * _spacingWood;
                Vector3 position = new Vector3(2.83f, y, 0);

                Wood wood =   LeanPool.Spawn(_objWoodRight);
                wood.transform.position = position;
                wood.Type = TypeWord.right;
                SetUpBranch(_data.branches[i + countwoodLeft].idBirds, wood);
            }


        }

        private void SetUpBranch(List<int> idBirds, Wood wood)
        {
            Transform[] tranBird = wood.GetTranBird();
            for(int i=0; i< idBirds.Count; i++)
            {
                GameObject bird = LeanPool.Spawn(_dataBirds.birds[idBirds[i]]);
                bird.transform.SetParent(wood.transform);
                bird.transform.position = tranBird[i].position;

                Bird birdI = bird.GetComponent<Bird>();
                wood.SetBird(birdI, i);
                wood.CanChoose = true;

                if (wood.Type == TypeWord.right)
                {
                    bird.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
        }
    }

    [System.Serializable]
    public class Data
    {
        public List<DataBranches> branches;
    }

    [System.Serializable]
    public class DataBranches
    {
        public List<int> idBirds;
    }
}

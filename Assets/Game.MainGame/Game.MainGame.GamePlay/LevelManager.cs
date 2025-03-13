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

        public Transform tranStartLeft;
        public Transform tranStartRight;
        public Vector3 posStartLeft;
        public Vector3 posStartRight;

        public GameController controller;

        private void Awake()
        {
                Instance = this;

        }

        private void Start()
        {
            posStartLeft = tranStartLeft.position;
            posStartRight = tranStartRight.position;
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
                wood.CanChoose = true;
                SetUpBranch(_data.branches[i].idBirds, wood);
            }

            for (int i = 0; i < countWoodRight; i++)
            {
                float y = (i - (countWoodRight - 1) / 2.0f) * _spacingWood;
                Vector3 position = new Vector3(2.83f, y, 0);

                Wood wood =   LeanPool.Spawn(_objWoodRight);
                wood.transform.position = position;
                wood.Type = TypeWord.right;
                wood.CanChoose = true;
                SetUpBranch(_data.branches[i + countwoodLeft].idBirds, wood);
            }


        }

        private void SetUpBranch(List<int> idBirds, Wood wood)
        {
            Transform[] tranBird = wood.GetTranBird();
            for(int i=0; i< idBirds.Count; i++)
            {
                GameObject bird = LeanPool.Spawn(_dataBirds.birds[idBirds[i]]);
                bird.transform.SetParent(wood.tranCha);

                Bird birdI = bird.GetComponent<Bird>();
                birdI.ID = idBirds[i];

                wood.SetBird(birdI, i);

                if (wood.Type == TypeWord.right)
                {
                    bird.transform.eulerAngles = new Vector3(0, 0, 0);

                    Vector2 tranStart = GetRandomPointAround(tranStartRight.position, 2f);
                    birdI.transform.position = tranStart;
                    birdI.MovingToBranchStart(tranBird[i], wood);
                }
                else
                {
                    bird.transform.eulerAngles = new Vector3(0, 180, 0);

                    Vector2 tranStart = GetRandomPointAround(tranStartLeft.position, 2f);
                    birdI.transform.position = tranStart;
                    birdI.MovingToBranchStart(tranBird[i], wood);
                }
            }
        }

        public Vector2 GetRandomPointAround(Vector2 center, float radius)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
            return center + randomOffset;
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

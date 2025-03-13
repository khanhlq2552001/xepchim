using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    [CreateAssetMenu(fileName = "DataBirds", menuName = "ScriptableObjects/DataBirds")]
    public class DataBird : ScriptableObject
    {
        public List<GameObject> birds;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO_Characters
{
    [CreateAssetMenu(fileName = "Characters List", menuName = "Characters /CharactersList")]
    public class Characters : ScriptableObject
    {
        public Animal[] chacterList;
    }

}
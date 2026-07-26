using UnityEngine;
using cpp;

namespace game
{
    public class Game : MonoBehaviour
    {
        public void Update()
        {

        }
        public void Start()
        {
            CompilerRunner.update();
        }
    }
}
using Gameplay.Management;
using UnityEngine;

namespace Gameplay.Actors.Player
{
    public class PlayerSeagullInputController : MonoBehaviour
    {
        [SerializeField]
        private SeagullController seagullController;
        private SeagullInputActionAsset _input;

        public void Awake()
        {
            _input = new SeagullInputActionAsset();
            _input.Enable();

            _input.Default.Pause.performed += context =>
            {
                GameManager.Instance.Pause();
            };

            _input.Default.Drop.performed += context =>
            {
                seagullController.DropFood();
            };
        }

        public void Update()
        {
            seagullController.Turn(_input.Default.Turn.ReadValue<float>());
            if (_input.Default.Brake.IsPressed())
            {
                seagullController.Brake(_input.Default.Brake.ReadValue<float>());
            }
            else
            {
                seagullController.Accelerate(_input.Default.Accelerate.ReadValue<float>());
            }
        }
    }
}

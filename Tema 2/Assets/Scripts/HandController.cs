using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace IRM
{
    public class HandController : MonoBehaviour
    {
        private static readonly int IsGrabbing = Animator.StringToHash("IsGrabbing");
        private static readonly int Trigger = Animator.StringToHash("Trigger");

        [SerializeField] private XRDirectInteractor interactor;
        [SerializeField] private InputActionReference triggerAction;
        
        private Animator _animator;

        private void Awake() =>
            _animator = GetComponent<Animator>();
        
        private void OnEnable()
        {
            interactor.selectEntered.AddListener(OnSelectEnter);
            interactor.selectExited.AddListener(OnSelectExit);
            
            triggerAction.action.performed += OnTriggerPerformed;
        }

        private void OnDisable()
        {
            interactor.selectEntered.RemoveListener(OnSelectEnter);
            interactor.selectExited.RemoveListener(OnSelectExit);
            
            triggerAction.action.performed -= OnTriggerPerformed;
        }

        private void OnSelectEnter(SelectEnterEventArgs _) =>
            _animator.SetBool(IsGrabbing, true);

        private void OnSelectExit(SelectExitEventArgs _) =>
            _animator.SetBool(IsGrabbing, false);

        private void OnTriggerPerformed(InputAction.CallbackContext context)
        {
            bool wasPressed = context.ReadValueAsButton();   
            
            if(wasPressed)
                _animator.SetTrigger(Trigger);
        }
    }
}

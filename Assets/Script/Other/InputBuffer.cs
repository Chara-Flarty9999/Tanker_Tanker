using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [HideInInspector] public bool inputAttack;
    [HideInInspector] public bool inputJump;
    [HideInInspector] public float inputHorizontal = 0;
    [HideInInspector] public float inputVertical = 0;
    [HideInInspector] public float inputReticleHorizontal = 0;
    [HideInInspector] public float inputReticleVertical = 0;

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }
    private void Inputs()
    {
        inputAttack = Input.GetButtonDown("Fire1");
        inputJump = Input.GetButtonDown("Jump");
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputReticleHorizontal = Input.GetAxisRaw("Reticle Horizontal");
        inputReticleVertical = Input.GetAxisRaw("Reticle Vertical");
    }
}

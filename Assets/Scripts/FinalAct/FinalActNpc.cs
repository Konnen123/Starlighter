using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalActNpc : MonoBehaviour
{
    [SerializeField] private GameObject hips,goodEndingText,badEndingText,player;
    private Animator npcAnimator;
    
    private bool isDead;
    private Collider[] colliders;
    private CharacterJoint[] characterJoints;
    private Rigidbody[] rigidbodies;

    private bool isItGoodEnding;
    private bool finalChoiseMade;
    void Start()
    {
        npcAnimator = GetComponent<Animator>();
        colliders = hips.GetComponentsInChildren<Collider>();
        characterJoints = hips.GetComponentsInChildren<CharacterJoint>();
        rigidbodies = hips.GetComponentsInChildren<Rigidbody>();
        
        enableAnimator();
        StartCoroutine(askToGiveTotem());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && finalChoiseMade)
        {
            finalChoiseMade = false;
            GetComponent<Collider>().enabled = false;
            isItGoodEnding = true;
            TipText.Instance.SetText("Thank you sir! Our civilization is saved.");
            StartCoroutine(EndingScenario(goodEndingText));
        }
    }

    IEnumerator askToGiveTotem()
    {
        TipText.Instance.SetColor(Color.white);
        TipText.Instance.SetText("Hello, I see you found our lost totem!");
        yield return new WaitForSeconds(3f);
        TipText.Instance.SetText("Will you give it back to us?");
        yield return new WaitForSeconds(2f);
        finalChoiseMade = true;
        TipText.Instance.SetText("Press E To give totem or shoot him");
    }

    IEnumerator EndingScenario(GameObject endScenario)
    {
        yield return new WaitForSeconds(2f);
        TipText.Instance.SetText("");
        endScenario.SetActive(true);
        yield return new WaitForSeconds(2f);
        endScenario.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(2f);
        player.GetComponent<MovementStateManager>().enabled = false;
        player.GetComponent<ThirdPersonShooterController>().enabled = false;
        endScenario.transform.GetChild(1).GetComponent<Animator>().enabled = true;
        endScenario.transform.GetChild(2).GetChild(0).GetComponent<Animator>().enabled = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
  
    void enableAnimator()
    {
        npcAnimator.enabled = true;
        foreach (var joint in characterJoints)
        {
            joint.enableCollision = false;
        }

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        
    }

   public void enableRagdoll()
    {
     
        npcAnimator.enabled = false;
        isDead = true;
        
        Rigidbody currentRigidbody = GetComponent<Rigidbody>();
        
        currentRigidbody.velocity=Vector3.zero;
        currentRigidbody.detectCollisions = false;
        currentRigidbody.useGravity = false;

        GetComponent<Collider>().enabled = false;
        foreach (var joint in characterJoints)
        {
            joint.enableCollision = true;
        }

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.velocity=Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        StartCoroutine(EndingScenario(badEndingText));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessScript : MonoBehaviour
{
    private Animator mAnimator;
    private GrabState grabState;
    private bool isInteract = false;
    public GameObject key;
    public Transform keySpawnPosition;

    void Start()
    {
        grabState = GrabState.Instance;
        mAnimator = GetComponent<Animator>();
        if (key != null)
        {
            key.SetActive(false);
        }
    }

    void HandleInteract()
    {
        if(grabState.inGrabingState)
        {
            grabState.isGrabing = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (mAnimator != null)
            {
                mAnimator.SetTrigger("ChestOpen");
                StartCoroutine(ShowKeyAfterAnimation());
            }
            else
            {
                Debug.LogError("Animator is not assigned!");
            }
        }

        IEnumerator ShowKeyAfterAnimation()
        {
            // Wait for the chest animation to finish (adjust duration as needed)
            yield return new WaitForSeconds(2.0f);

            if (key != null)
            {
                // Set key active and move it to the spawn position
                key.transform.position = keySpawnPosition.position;
                key.SetActive(true);
            }
            else
            {
                Debug.LogError("Key GameObject is not assigned!");
            }
        }
    }
}

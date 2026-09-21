using System.Collections.Generic;
using UnityEngine;

public class Gripper : MonoBehaviour
{
    public List<Rigidbody> triggeredList = new List<Rigidbody> ();
    public Animator anim;

    private void Awake()
    {
        if(anim == null)
            anim = GetComponent<Animator> ();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.isKinematic)
            return;

        if (triggeredList.Contains(other.attachedRigidbody))
            return;

        triggeredList.Add(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.attachedRigidbody.isKinematic)
            return;

        if (!triggeredList.Contains(other.attachedRigidbody))
            return;

        triggeredList.Remove(other.attachedRigidbody);
    }

    public void Grab(bool isGrab)
    {
        if (isGrab)
        {
            if(triggeredList.Count == 0)
                return;

            triggeredList[0].transform.SetParent(transform);
            triggeredList[0].isKinematic = true;
            anim.SetBool("Grab", true);
        }
        else
        {
            if (triggeredList.Count == 0)
                return;

            triggeredList[0].transform.SetParent(null);
            triggeredList[0].isKinematic = false;

            anim.SetBool("Grab", false);
        }
    }

    public void UpDown(bool isUp)
    {
        anim.SetBool("IsUp", isUp);
    }
}

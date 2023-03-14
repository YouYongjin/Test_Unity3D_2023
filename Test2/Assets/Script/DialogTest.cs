using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    [SerializeField]
    private DialogueSystem dialogSystem;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => dialogSystem.UpdateDialogue(0, true));
    }
}

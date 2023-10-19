using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteractable { get; }
    void EnableInteract();
    void DisableInteract();
}

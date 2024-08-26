using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToTrigger : MonoBehaviour
{
    public abstract IEnumerator DoAction();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    
    public abstract IEnumerator Enter();

    public abstract IEnumerator Execute();

    public abstract IEnumerator Exit();

}

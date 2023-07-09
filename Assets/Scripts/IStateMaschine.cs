using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMaschine
{
    public enum gameStates
    {
        build,
        play,
        after
    }

    public abstract void Build();
    public abstract void Play();
    public abstract void After();

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PowerupCards
{
    protected UserStatus status;

    public virtual void Init(UserStatus status)
    {
        this.status = status;

        CardInit();
    }

    public abstract void CardInit();


}

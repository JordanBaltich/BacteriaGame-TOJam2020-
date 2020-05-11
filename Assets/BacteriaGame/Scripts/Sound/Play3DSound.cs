using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play3DSound : MonoBehaviour
{
    Rigidbody neededBody;

    [FMODUnity.EventRef]
    public string attack;
    [FMODUnity.EventRef]
    public string merge;
    [FMODUnity.EventRef]
    public string split;
    [FMODUnity.EventRef]
    public string dead;
    [FMODUnity.EventRef]
    public string hurt;
    [FMODUnity.EventRef]
    public string captured;
    [FMODUnity.EventRef]
    public string lost;


    FMOD.Studio.EventInstance attackEvent;
    FMOD.Studio.EventInstance mergeEvent;
    FMOD.Studio.EventInstance splitEvent;
    FMOD.Studio.EventInstance deadEvent;
    FMOD.Studio.EventInstance hurtEvent;
    FMOD.Studio.EventInstance capturedEvent;
    FMOD.Studio.EventInstance lostEvent;

    private void Awake()
    {
        neededBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackEvent = FMODUnity.RuntimeManager.CreateInstance(attack);
        mergeEvent = FMODUnity.RuntimeManager.CreateInstance(merge);
        splitEvent = FMODUnity.RuntimeManager.CreateInstance(split);
        deadEvent = FMODUnity.RuntimeManager.CreateInstance(dead);
        hurtEvent = FMODUnity.RuntimeManager.CreateInstance(hurt);
        capturedEvent = FMODUnity.RuntimeManager.CreateInstance(captured);
        lostEvent = FMODUnity.RuntimeManager.CreateInstance(lost);

    }

    // Update is called once per frame
    void Update()
    {
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(mergeEvent, this.transform, neededBody);

        AttachFMODInstance(attackEvent);
        AttachFMODInstance(mergeEvent);
        AttachFMODInstance(splitEvent);
        AttachFMODInstance(deadEvent);
        AttachFMODInstance(hurtEvent);
        AttachFMODInstance(capturedEvent);
        AttachFMODInstance(lostEvent);
    }

    #region PlayMethods
    public void PlayAttack()
    {
        PlaySound(attackEvent);
    }

    public void PlayMerge()
    {
         PlaySound(mergeEvent);
    }

    public void PlaySplit()
    {
        PlaySound(splitEvent);
    }

    public void PlayHurt()
    {
        PlaySound(hurtEvent);
    }

    public void PlayDead()
    {
        PlaySound(deadEvent);
    }

    public void PlayCaptured()
    {
        PlaySound(capturedEvent);
    }

    public void PlayLost()
    {
        PlaySound(lostEvent);
    }
    #endregion

    #region FMOD
    void AttachFMODInstance(FMOD.Studio.EventInstance instance)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, this.transform, neededBody);
    }

    void AssignSoundToEvent(FMOD.Studio.EventInstance instance, string eventString)
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(eventString);
    }

    void PlaySound(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE fmodPBState;
        instance.getPlaybackState(out fmodPBState);
        if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            instance.start();
        }
    }
    #endregion

}

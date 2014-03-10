﻿using System.Collections;
using TouchScript.Gestures;
using UnityEngine;

public class LongPress_Button : MonoBehaviour
{

    public GameObject Plane;

    private Transform button, thebase;
    private float timeToPress;
    private Vector3 startScale, targetScale;

    private void Awake()
    {
        button = transform.FindChild("Button");
        thebase = transform.FindChild("Base");
        startScale = button.localScale;
        targetScale = thebase.localScale;
    }

	private void OnEnable()
	{
	    timeToPress = GetComponent<LongPressGesture>().TimeToPress;

	    GetComponent<PressGesture>().StateChanged += pressStateChanged;
        GetComponent<ReleaseGesture>().StateChanged += releaseStateChanged;
        GetComponent<LongPressGesture>().StateChanged += longPressStateChanged;
	}

    private void OnDisable()
    {
        GetComponent<PressGesture>().StateChanged -= pressStateChanged;
        GetComponent<ReleaseGesture>().StateChanged -= releaseStateChanged;
        GetComponent<LongPressGesture>().StateChanged -= longPressStateChanged;
    }

    private void press()
    {
        button.transform.localPosition = new Vector3(0, -button.transform.localScale.y * .4f, 0);
    }

    private void release()
    {
        button.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void reset()
    {
        button.transform.localScale = startScale;
        StopCoroutine("grow");
    }

    private void changeColor()
    {
        if (Plane == null) return;

        Plane.renderer.material.color = button.renderer.sharedMaterial.color;
    }

    private IEnumerator grow()
    {
        while (true)
        {
            button.transform.localScale += (targetScale.x - startScale.x) / timeToPress * Time.deltaTime * new Vector3(1, 0, 1);
            yield return null;
        }
    }

    private void longPressStateChanged(object sender, GestureStateChangeEventArgs e)
    {
        switch (e.State)
        {
            case Gesture.GestureState.Recognized:
            case Gesture.GestureState.Failed:
            case Gesture.GestureState.Cancelled:
                reset();
                break;
        }

        if (e.State == Gesture.GestureState.Recognized)
        {
            changeColor();
        }
    }

    private void pressStateChanged(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized)
        {
            press();
            StartCoroutine("grow");
        }
    }

    private void releaseStateChanged(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized)
        {
            release();
        }
    }

}
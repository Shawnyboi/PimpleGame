using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpiderReact : MonoBehaviour
{
  [SerializeField] UnityEvent onReactStart = null;
  [SerializeField] UnityEvent onReactEnd = null;

  [SerializeField] GameObject blinkRenderer;

  bool reacting = false;

  [SerializeField] float reactTime = 1;
  float reactRemaining = 0;
  [SerializeField] float blinkRate = 10;

  private void Update()
  {
    if (reacting)
    {
      if (reactRemaining <= 0)
      {
        reacting = false;
        onReactEnd.Invoke();
      }
      reactRemaining -= Time.deltaTime;
    }
  }

  IEnumerator blink()
  {
    if (blinkRenderer != null)
    {
      blinkRenderer.SetActive(!blinkRenderer.activeSelf);
      yield return new WaitForSeconds(Mathf.Min(1f / blinkRate, reactRemaining));

      if (reacting)
      {
        yield return blink();
      }
      else
      {
        blinkRenderer.SetActive(true);
      }
    }
  }


  private void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.gameObject.tag == "Spider" && !reacting)
    {
      if (!reacting)
      {
        reacting = true;
        reactRemaining = reactTime;
        StartCoroutine(blink());
        onReactStart.Invoke();
      }
    }
  }
}

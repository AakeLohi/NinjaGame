using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboScript : MonoBehaviour
{
    public int number;
    public float duration;
    public float moveSpeed;
    public float fadeDuration;

    private float deathTimer;
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = number.ToString("F0");
        deathTimer = duration;
        StartCoroutine(FadeOutCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        deathTimer -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);

        if (deathTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color startingColor = text.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.color = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}

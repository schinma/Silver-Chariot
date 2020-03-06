using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstAlly : MonoBehaviour
{

    public Text helpText;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        helpText.text = "Help ! Help !";
        helpText.gameObject.SetActive(true);
    }

    IEnumerator HelpText()
    {
        Time.timeScale = 0f;
        helpText.text = "Thank you ! I'll help you pull your chariot, but we'll need more food";
        yield return new WaitForSecondsRealtime(4f);
        helpText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnTriggerEnter(Collider other)
    {

        StartCoroutine(HelpText());
    }
}

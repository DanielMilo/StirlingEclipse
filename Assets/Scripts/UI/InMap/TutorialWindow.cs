using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialWindow : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] Text titleText;
    [SerializeField] Text contentText;

    [Header("Content")]
    [SerializeField] string[] resourcesInfoTitle;
    [SerializeField] string[] resourcesInfoContent;
    [SerializeField] string[] zonesInfoTitle;
    [SerializeField] string[] zonesInfoContent;
    [SerializeField] string[] drawbackInfoTitle;
    [SerializeField] string[] drawbackInfoContent;
    [SerializeField] string[] finishInfoTitle;
    [SerializeField] string[] finishInfoContent;

    [HideInInspector] public int currentInfoIndex;

    TutorialManager tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentInfoIndex < getCurrentSize())
        {
            window.SetActive(true);
            titleText.text = getCurrentTitle();
            contentText.text = getCurrentContent();
        }
        else
        {
            window.SetActive(false);
        }
    }

    string getCurrentTitle()
    {

        switch(tutorialManager.step)
        {
            case TutorialStep.resources:
                return resourcesInfoTitle[currentInfoIndex];

            case TutorialStep.zones:
                return zonesInfoTitle[currentInfoIndex];

            case TutorialStep.drawback:
                return drawbackInfoTitle[currentInfoIndex];

            case TutorialStep.finish:
                return finishInfoTitle[currentInfoIndex];
        }

        return null;
    }

    string getCurrentContent()
    {

        switch(tutorialManager.step)
        {
            case TutorialStep.resources:
                return resourcesInfoContent[currentInfoIndex];

            case TutorialStep.zones:
                return zonesInfoContent[currentInfoIndex];

            case TutorialStep.drawback:
                return drawbackInfoContent[currentInfoIndex];

            case TutorialStep.finish:
                return finishInfoContent[currentInfoIndex];
        }

        return null;
    }

    public int getCurrentSize()
    {
        switch(tutorialManager.step)
        {
            case TutorialStep.resources:
                return resourcesInfoTitle.Length;

            case TutorialStep.zones:
                return zonesInfoTitle.Length;

            case TutorialStep.drawback:
                return drawbackInfoTitle.Length;

            case TutorialStep.finish:
                return finishInfoTitle.Length;
        }

        return -1;
    }

    public void OnBackButton()
    {
        if(currentInfoIndex > 0)
        {
            currentInfoIndex--;
        }
    }

    public void OnNextButton()
    {
            currentInfoIndex++;
    }

}

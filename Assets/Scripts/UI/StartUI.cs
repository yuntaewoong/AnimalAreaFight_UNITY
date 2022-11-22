using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialObject;//Ʃ�丮��â ���۷���
    [SerializeField] private TextMeshProUGUI[] mBlinkingTexts;//�����̰Ե� �ؽ�Ʈ��
    private void OnEnable()
    {
        StartCoroutine(BlinkText());//���� �����̰�
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(tutorialObject.activeSelf)
            {
                tutorialObject.SetActive(false);
            }
            else
            {
                tutorialObject.SetActive(true);
            }
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            GameManager.instance.ChangeStateStartToCharacterSelection();
        }
    }
    public IEnumerator BlinkText()
    {
        while (true)
        {
            List<string> textList = new List<string>();
            for (int i = 0;i<mBlinkingTexts.Length;i++)
            {
                textList.Add(mBlinkingTexts[i].text);
                mBlinkingTexts[i].text = "";
            }
            yield return new WaitForSeconds(.25f);
            for (int i = 0; i < mBlinkingTexts.Length; i++)
            {
                mBlinkingTexts[i].text = textList[i];
            }
            yield return new WaitForSeconds(.25f);
        }
    }
}

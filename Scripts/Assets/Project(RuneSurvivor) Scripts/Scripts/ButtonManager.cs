using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [Header("�ɼ�â")]
    public GameObject settingPanel;
    public float fadeTime = 1.5f;
    [Header("����")]
    public CanvasGroup storePanelCanGroup;
    public RectTransform storePanelrectTransform;
    public Text storeText;
    [Header("����")]
    public CanvasGroup[] skillImageFadeGroup;
    public List<GameObject> items = new List<GameObject>();
    public void PanelFadeIn(CanvasGroup canvasGroup) //, RectTransform rectTransform
    {
        storeText.GetComponent<Text>().DOColor(Color.yellow, 2); //.From() ���̸� �Ųٷ� ���������
        canvasGroup.alpha = 0;
        storePanelrectTransform.transform.localPosition = new Vector3(0, -1000, 0);
        storePanelrectTransform.DOAnchorPos(new Vector2(0, 0), fadeTime, false).SetEase(Ease.OutElastic).SetUpdate(true);
        canvasGroup.DOFade(1, fadeTime).SetUpdate(true);
        StartCoroutine(slotAnimation());
    }
    public void PanelFadeOut(CanvasGroup canvasGroup) //, RectTransform rectTransform
    {
        canvasGroup.alpha = 1;
        //rectTransform.transform.localPosition = new Vector3(0, -1000, 0);
        //rectTransform.DOAnchorPos(new Vector2(0, 0), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(0, fadeTime).SetUpdate(true);
    }
    IEnumerator slotAnimation()
    {
        foreach (GameObject item in items)
        {
            item.transform.localScale = Vector3.zero;
        }
        foreach (GameObject item in items)
        {
            item.transform.DOScale(1f, 2).SetEase(Ease.OutBounce).SetUpdate(true); //.SetUpdate(true) timeScale�� 0���� ������൵ �۵��� �� �ְ� �ϴ� �Լ�
            yield return null;
        }
    }
    //��ư----------------------------------------
    public void HomeButton()
    {
        SceneManager.LoadScene("FirstScene");
    } //���ӿ��� Ȩ��ư
    public void ReplayButton()
    {
        SceneManager.LoadScene("GameScene");
    } // ���ӿ��� �ٽ��ϱ� ��ư

    public void SettingButton() //�ɼǹ�ư
    {
        if (settingPanel.activeSelf)
        {
            settingPanel.SetActive(false);
        }
        else
        {
            settingPanel.SetActive(true);
        }

    }
    //--------------------------------------------


}

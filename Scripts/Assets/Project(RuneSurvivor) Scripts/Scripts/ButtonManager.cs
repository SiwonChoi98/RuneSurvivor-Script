using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [Header("옵션창")]
    public GameObject settingPanel;
    public float fadeTime = 1.5f;
    [Header("상점")]
    public CanvasGroup storePanelCanGroup;
    public RectTransform storePanelrectTransform;
    public Text storeText;
    [Header("슬롯")]
    public CanvasGroup[] skillImageFadeGroup;
    public List<GameObject> items = new List<GameObject>();
    public void PanelFadeIn(CanvasGroup canvasGroup) //, RectTransform rectTransform
    {
        storeText.GetComponent<Text>().DOColor(Color.yellow, 2); //.From() 붙이면 거꾸로 원래색깔로
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
            item.transform.DOScale(1f, 2).SetEase(Ease.OutBounce).SetUpdate(true); //.SetUpdate(true) timeScale을 0으로 만들어줘도 작동할 수 있게 하는 함수
            yield return null;
        }
    }
    //버튼----------------------------------------
    public void HomeButton()
    {
        SceneManager.LoadScene("FirstScene");
    } //게임오버 홈버튼
    public void ReplayButton()
    {
        SceneManager.LoadScene("GameScene");
    } // 게임오버 다시하기 버튼

    public void SettingButton() //옵션버튼
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

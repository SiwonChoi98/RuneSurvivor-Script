using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    public Image progressBar;
    public Text text;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene"); // LoadScene�� �������̶� ���� �ҷ����������� �ٸ��۾��� �Ҽ�������.
    }
    void Start()
    {
        text = text.GetComponent<Text>();
        text.DOText("���� ���� �� .....", 3);

        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //LoadSceneAsync�� �񵿱����̶� ���� �ҷ����鼭 �ٸ��۾��� �����ϴ�.
        op.allowSceneActivation = false; //���� �񵿱�� �ҷ��ö� ���� �ε��� ������ �ڵ����� �ҷ��� ������ �̵��Ұ������� �����ϴ°� //�̰��� false�� �����ϸ� ���� 90�ۼ�Ʈ������ �����ϰ� ���������� �Ѿ�°� ��ٸ��� true�� �����ϸ� ���� 10�۸� ä���� �Ѿ�� ���

        float timer = 0f;
        while (!op.isDone) //���ε��� ������ ���� �����϶� //isDone�� ���� �ִ� �Լ��̴�.
        {
            yield return null; //�ݺ����� �ѹ��ݺ��Ҷ����� ����Ƽ������ ������� �ѱ�� �� ������� �Ѱ����������� �ݺ����� ������������ ȭ���� ���ŵ��� �ʾƼ� ����ٰ� �������°� �ȵȴ�.
            IEnumerator WaitandStart()
            {
                yield return new WaitForSeconds(2f);
                op.allowSceneActivation = true;
            }
            if (op.progress < 0.1f)
            {
                progressBar.fillAmount = op.progress;

            }
            else
            {
                timer += Time.unscaledDeltaTime;
                if (1 >= timer)
                {
                    StartCoroutine(WaitandStart()); //��� ��ٷȴٰ� ��������� //tip ������ �����ַ��� ��¦��ٷ��ش�.

                    yield break;
                }
            }
                
            // progressBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer);

           

        }

    }



}

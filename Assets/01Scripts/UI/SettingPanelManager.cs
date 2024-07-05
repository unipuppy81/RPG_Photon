using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelManager : MonoBehaviour
{
    [Header("Setting UI")]
    [SerializeField]
    private Button ExitBtn;
    [SerializeField]
    private Button SettingBtn;
    [SerializeField]
    private Button QuestBtn;

    [SerializeField]
    private GameObject ExitPanel;
    [SerializeField]
    private GameObject SettingPanel;
    [SerializeField]
    private GameObject QuestPanel;


    [Header("Exit")]
    [SerializeField]
    private Button gameBackBtn;
    [SerializeField]
    private Button gameExitBtn;

    [Header("Setting")]
    [SerializeField]
    private Button SettingExitButton;

    [Header("Brightness")]
    [SerializeField] 
    private Image brightnessPanel; // ȭ�� ��⸦ ������ �г�
    [SerializeField]
    private Slider brightnessSlider; // ��� ���� �����̴�

    [Header("Graphic")]
    [SerializeField]
    private Button lowSetBtn;
    [SerializeField]
    private Button middleSetBtn;
    [SerializeField]
    private Button highSetBtn;

    private void Start()
    {
        ExitBtn.onClick.AddListener(ExitPanelActive);
        gameBackBtn.onClick.AddListener(BackGame);
        gameExitBtn.onClick.AddListener(GameExit);

        SettingBtn.onClick.AddListener(SettingPanelActive);
        SettingExitButton.onClick.AddListener(SettingPanelExit);





        // �����̴� �� ���� �� OnBrightnessChange �޼��� ȣ��
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChange);

        brightnessSlider.value = 0f;
        brightnessSlider.maxValue = 180f;
    }

    private void ExitPanelActive()
    {
        ExitPanel.SetActive(true);
    }

    private void BackGame()
    {
        ExitPanel.SetActive(false);
    }

    private void GameExit()
    {
        Debug.Log("Game Finish");
    }

    private void SettingPanelActive()
    {
        SettingPanel.SetActive(true);
    }
    private void SettingPanelExit()
    {
        SettingPanel.SetActive(false);
    }
    public void OnBrightnessChange(float value)
    {
        // �г��� ���� ����
        Color color = brightnessPanel.color;
        color.a = value / 255f; // ���� ���� 0���� 180 ���̷� ����
        brightnessPanel.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour {
    public GameObject baslat;
    public GameObject ayarlar;
    public GameObject cikis;
    public GameObject quality;
    public GameObject geri;
    public GameObject music;
    public GameObject ayarlarImage;
    public AudioMixer audioMixer;

    public List<StageScriptable> stageList;
    public Transform stagePanelFolder;
    private List<GameObject> pageList, prefabStages;
    int currentPageIndex;
    static readonly int STAGE_PER_PAGE = 9;
    private void Start() {
        baslat.SetActive(true);
        ayarlar.SetActive(true);
        cikis.SetActive(true);
        quality.SetActive(false);
        geri.SetActive(false);
        music.SetActive(false);
        ayarlarImage.SetActive(false);

        // Ilk bölüm kilitliyse aç
        if (Utility.LoadStage(0) == StageState.LOCKED)
            Utility.SaveStage(0, StageState.ZERO_STAR);


        Load();
        GetPrefabs();
        CreateMenu();

        for (int i = 0; i < stageList.Count; i++)
            stageList[i] = Instantiate(stageList[i]);
    }
    public void Baslat() {
        currentPageIndex = 0;
        OpenPage();

        stagePanelFolder.gameObject.SetActive(true);
    }
    public void OnAyarlarClicked() {
        baslat.SetActive(false);
        ayarlar.SetActive(false);
        cikis.SetActive(false);
        quality.SetActive(true);
        geri.SetActive(true);
        music.SetActive(true);
        ayarlarImage.SetActive(true);
    }
    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void OnGeriClicked() {
        baslat.SetActive(true);
        ayarlar.SetActive(true);
        cikis.SetActive(true);
        quality.SetActive(false);
        geri.SetActive(false);
        music.SetActive(false);
        ayarlarImage.SetActive(false);
    }
    public void OnCikisClicked()
    {
        Application.Quit();
    }
    public void MusicBoolClicked(bool temp)
    {
        if (temp)
            audioMixer.SetFloat("volume", 0);
        else
            audioMixer.SetFloat("volume", -80);
    }

    private void GetPrefabs() {
        prefabStages = new List<GameObject>();

        for (int i = 0; i < (int)StageState.COUNT; i++)
            prefabStages.Add(Resources.Load<GameObject>("Prefabs/" + (StageState)i));
    }
    private void CreateMenu() {
        pageList = new List<GameObject>();

        for (int i = 0; i - 1 < stageList.Count / STAGE_PER_PAGE; i++) {
            // Create Empty Page Folder
            pageList.Add(new GameObject());
            pageList[i].transform.SetParent(stagePanelFolder);
            pageList[i].transform.localPosition = new Vector3(0f, 100f, 0f);
            pageList[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            pageList[i].name = "Page_" + i;

            for (int j = 0; j < STAGE_PER_PAGE; j++) {
                int index = STAGE_PER_PAGE * i + j;
                GameObject obj;

                if (index < stageList.Count) {
                    obj = CreateStage(index, pageList[i].transform);
                } else // Insert Dummy
                    obj = Instantiate(prefabStages[(int)StageState.LOCKED], pageList[i].transform);

                obj.GetComponent<RectTransform>().localPosition = new Vector3((-334.25f + ((j % 3) * 334.25f)), 100f - ((j / 3) * 334.25f), 0);
            }
        }
    }
    private void OpenPage() {
        for (int i = 0; i < pageList.Count; i++)
            pageList[i].SetActive(false);

        pageList[currentPageIndex].SetActive(true);

        Save();
    }
    public void NextPage() {
        if (currentPageIndex + 1 >= pageList.Count)
            return;

        currentPageIndex++;
        OpenPage();
    }
    public void PreviousPage() {
        if (0 > currentPageIndex - 1)
            return;

        currentPageIndex--;
        OpenPage();
    }
    private GameObject CreateStage(int index, Transform pageFolder) {
        StageState stageState = Utility.LoadStage(index);

        if (index == 0 && stageState == StageState.LOCKED)
            stageState = StageState.ZERO_STAR;

        GameObject obj = Instantiate(prefabStages[(int)stageState], pageFolder);
        obj.GetComponent<StageUI>().Set(index, stageState, stageList[index].sceneName);

        return obj;
    }
    private void Load() {
        currentPageIndex = EncryptedPlayerPrefs.GetInt("currentPageIndex", 0);
    }
    private void Save() {
        EncryptedPlayerPrefs.SetInt("currentPageIndex", currentPageIndex);
    }
}

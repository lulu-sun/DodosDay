using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    public static AlbumManager Instance { get; private set; }

    // Start is called before the first frame update
    [SerializeField] List<GameObject> pages;
    [SerializeField] List<GameObject> selectors;
    [SerializeField] GameObject photoPage;
    [SerializeField] GameObject albumEntry;
    [SerializeField] Text albumDescription;
    [SerializeField] Text leftPageNum;
    [SerializeField] Text rightPageNum;

    [SerializeField] Button closeButton;

    public List<GameObject> Photos;
    public List<GameObject> Descriptions;

    private int pageIndex = 0;
    private int selectedPictureIndex;

    private float timeSinceSelected = 0f;
    [SerializeField] float requiredTimeToDeselect = 0.1f;

    private void GoToPage(int currentPageIndex, int targetPageIndex)
    {
        leftPageNum.text = (targetPageIndex * 2 + 1).ToString();
        rightPageNum.text = (targetPageIndex * 2 + 2).ToString();

        pages[currentPageIndex].SetActive(false);
        pages[targetPageIndex].SetActive(true);

        pageIndex = targetPageIndex;
    }
    
    private void UpdateMoveSelection(int previouslySelectedIndex, int currentlySelectedIndex)
    {
        selectors[previouslySelectedIndex].SetActive(false);
        selectors[currentlySelectedIndex].SetActive(true);
    }

    private void SetPageImage(int selectedPhotoIndex)
    {
        albumEntry.GetComponent<RawImage>().texture = Photos[selectedPhotoIndex].GetComponent<RawImage>().texture;
        albumDescription.text = Descriptions[selectedPhotoIndex].GetComponent<Text>().text.ToString();
    }

    private void TurnPageForward()
    {
        GoToPage(pageIndex, pageIndex + 1);
    }

    private void TurnPageBackward()
    {
        GoToPage(pageIndex, pageIndex - 1);
    }

    private void HandleUpdate()
    {
        int previousIndex = selectedPictureIndex;

        if (!photoPage.activeSelf)
        {

            if (Controls.GetRightKeyDown())
            {
                if (pageIndex < pages.Count - 1 && ((selectedPictureIndex + 1) % 4 == 0))
                {
                    TurnPageForward();
                }
                else if (((selectedPictureIndex + 1) % 4 != 0) && selectedPictureIndex < selectors.Count - 1)
                {
                    ++selectedPictureIndex;
                }
            }

            else if (Controls.GetLeftKeyDown())
            {
                if (pageIndex > 0 && ((selectedPictureIndex % 4) == 0))
                {
                    TurnPageBackward();
                }
                else if (((selectedPictureIndex % 4) != 0) && selectedPictureIndex > 0)
                {
                    --selectedPictureIndex;
                }                    
            }

            else if (Controls.GetDownKeyDown())
            {

                if (selectedPictureIndex < Photos.Count - 4)
                {
                    selectedPictureIndex += 4;
                }
            }

            else if (Controls.GetUpKeyDown())
            {
                if (selectedPictureIndex >= 4)
                {
                    selectedPictureIndex -= 4;
                }
            }

            if (previousIndex != selectedPictureIndex)
            {
                UpdateMoveSelection(previousIndex, selectedPictureIndex);
            }

            if (Controls.GetSelectKeyDown())
            {
                timeSinceSelected = 0f;
                SetPageImage(selectedPictureIndex);
                EnablePhotoPage(true);
            }
        }

        if (Controls.GetSelectKeyDown() && timeSinceSelected >= requiredTimeToDeselect)
        {
            EnablePhotoPage(false);
        }
    }


    public void EnablePhotoPage(bool enabled)
    {
        photoPage.SetActive(enabled);
    }

    
    private void GetChildren()
    {
        foreach (Transform child in pages[pageIndex].transform)
            {
                Photos.Add(child.gameObject);
            }

        foreach (var photo in Photos)
        {
            foreach (Transform child in photo.transform)
            {
                Descriptions.Add(child.gameObject);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GetChildren();
        closeButton.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleUpdate();
        timeSinceSelected += Time.deltaTime;
    }

    public void LeaveAlbum()
    {
        closeButton.interactable = false;
        StartCoroutine(DelayedLeaveAlbum(1.5f, () =>
        {
            TitleScreen.Instance.ShowTitle();
            SceneManager.LoadScene(SceneMapper.Instance.GetBuildIndexBySceneName("Intro"));
        }));
    }

    private IEnumerator DelayedLeaveAlbum(float delay, Action onFinished = null)
    {
        AudioManager.Instance.FadeMusic(delay, 0);
        yield return new WaitForSeconds(delay);
        onFinished?.Invoke();
    }
}

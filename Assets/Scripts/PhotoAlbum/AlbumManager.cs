using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> pages;
    [SerializeField] List<GameObject> selectors;
    [SerializeField] GameObject photoPage;
    [SerializeField] GameObject albumEntry;
    [SerializeField] Text albumDescription;
    [SerializeField] Text leftPageNum;
    [SerializeField] Text rightPageNum;

    public List<GameObject> Photos;
    public List<GameObject> Descriptions;

    private int pageIndex = 0;
    private int selectedPictureIndex;

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

    private void TurnPage()
    {
        if (selectedPictureIndex >= Photos.Count)
        {
            if (pageIndex < pages.Count - 1)
            {
                GoToPage(pageIndex, pageIndex + 1);
                selectedPictureIndex = 0;
            }
            
        }

        if (selectedPictureIndex < 0)
        {
            if (pageIndex > 0)
            {   
                GoToPage(pageIndex, pageIndex - 1);
            }
            selectedPictureIndex = 11;
        }
    }

    private void ArrowKeyMovement()
    {
        // Debug.Log(selectedPictureIndex);

        int previousIndex = selectedPictureIndex;

        if (!photoPage.activeSelf)
        {

            if (Controls.GetRightKeyDown())
            {
                if (!(pageIndex == pages.Count - 1 && selectedPictureIndex == 11))
                {
                    ++ selectedPictureIndex;
                    TurnPage();
                }
            }

            else if (Controls.GetLeftKeyDown())
            {
                if (!(pageIndex == 0 && selectedPictureIndex == 0))
                {
                    -- selectedPictureIndex;
                    TurnPage();
                }

            }

            else if (Controls.GetDownKeyDown())
            {

                if (selectedPictureIndex < Photos.Count - 2)
                {
                    selectedPictureIndex += 2;
                }
            }

            else if (Controls.GetUpKeyDown())
            {
                if (selectedPictureIndex > 1)
                {
                    selectedPictureIndex -= 2;
                }
            }

            if (previousIndex != selectedPictureIndex)
            {
                UpdateMoveSelection(previousIndex, selectedPictureIndex);
            }

            if (Controls.GetSelectKeyDown())
            {
                SetPageImage(selectedPictureIndex);
                EnablePhotoPage(true);
            }
        }

        if (Controls.GetDeselectKeyDown())
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

        Debug.Log(Descriptions);

    }

    void Start()
    {
        GetChildren();
        AudioManager.Instance.PlayAlbumMusic();


    }

    // Update is called once per frame
    void Update()
    {
        ArrowKeyMovement();
    }
}

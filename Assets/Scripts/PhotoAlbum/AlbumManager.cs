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
    [SerializeField] Text leftPageNum;
    [SerializeField] Text rightPageNum;

    public List<GameObject> Photos;
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

        if (!photoPage.active)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!(pageIndex == pages.Count - 1 && selectedPictureIndex == 11))
                {
                    ++ selectedPictureIndex;
                    TurnPage();
                }
            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!(pageIndex == 0 && selectedPictureIndex == 0))
                {
                    -- selectedPictureIndex;
                    TurnPage();
                }

            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                if (selectedPictureIndex < Photos.Count - 2)
                {
                    selectedPictureIndex += 2;
                }
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow))
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetPageImage(selectedPictureIndex);
                EnablePhotoPage(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    void Start()
    {
        GetChildren();        

    }

    // Update is called once per frame
    void Update()
    {
        ArrowKeyMovement();
    }
}

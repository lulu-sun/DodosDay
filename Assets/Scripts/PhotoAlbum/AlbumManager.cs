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
    private int indexSelected = 0;
    private int currentSelection;

    private void GoToPage(int index)
    {
        leftPageNum.text = (pageIndex + 1).ToString();
        rightPageNum.text = (pageIndex + 2).ToString();
        pages[pageIndex].SetActive(false);
        pages[pageIndex + 1].SetActive(true);
    }
    
    private void UpdateMoveSelection(int selectedPhotoIndex)
    {
        selectors[indexSelected].SetActive(false);
        selectors[selectedPhotoIndex].SetActive(true);
        indexSelected = selectedPhotoIndex;
    }

    private void SetPageImage(int selectedPhotoIndex)
    {
        albumEntry.GetComponent<RawImage>().texture = Photos[selectedPhotoIndex].GetComponent<RawImage>().texture;
    }

    private void ArrowKeyMovement()
    {
        // Debug.Log(currentSelection);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (currentSelection < Photos.Count)
            {
                ++ currentSelection;
            }

            if (currentSelection == Photos.Count)
            {
                GoToPage(pageIndex);
                currentSelection = 0;
                indexSelected = 1;
                pageIndex += 1;  
            }

        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentSelection > 0)
            {
                -- currentSelection;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentSelection < Photos.Count - 2)
            {
                currentSelection += 2;
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentSelection > 1)
            {
                currentSelection -= 2;
            }
        }

        UpdateMoveSelection(currentSelection);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPageImage(currentSelection);
            EnablePhotoPage(true);
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
        Debug.Log(pages.Count);
        Debug.Log(Photos.Count);
    }

    // Update is called once per frame
    void Update()
    {
        ArrowKeyMovement();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour
{

    public static InstructionsController instance;
    public GameObject[] pages;
    public GameObject nextPageButton;
    public GameObject prevPageButton;
    public GameObject backButton;

    // State
    GameObject currPage;
    int currPageNo;
    
    void Awake() {
        instance = this;
    }
    void Start()
    {
        currPageNo = 0;
        refreshPage();
        // currPage = Instantiate(pages[currPageNo], Vector3.zero, Quaternion.identity);
        // currPage.transform.SetParent(gameObject.GetComponent<Transform>());
        // prevPageButton.GetComponent<GameObject>().SetActive(false);

    }

    public void onNextClick() {
        ++currPageNo;
        refreshPage();
    }

    public void onPrevClick() {
        --currPageNo;
        refreshPage();
    }

    public void onBackClick() {
        Debug.Log("Close instructions");
        Destroy(gameObject);
    }

    void refreshPage() {
        Destroy(currPage);
        currPage = Instantiate(pages[currPageNo], Vector3.zero, Quaternion.identity);
        currPage.transform.SetParent(gameObject.GetComponent<Transform>());
        nextPageButton.SetActive(currPageNo < pages.Length - 1);
        prevPageButton.SetActive(currPageNo > 0);
        // Debug.Log("NextPageButton active: " + nextPageButton.activeSelf);
        // Debug.Log("PrevPageButton active: " + prevPageButton.activeSelf);

    }

}

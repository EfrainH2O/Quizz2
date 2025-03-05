using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGamplay : MonoBehaviour
{
    [SerializeField]
    private SceneField loadingScene;
    [SerializeField]
    private List<SceneField> scenetoLoad;

    private List<AsyncOperation> loadingProcess;
    private AsyncOperation loadingloading ;

    void Awake()
    {
        loadingProcess = new List<AsyncOperation>();
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    void OnEnable(){
        ChangeScene();
    }

    // Update is called once per frame
    public void ChangeScene()
    {
        loadingloading = SceneManager.LoadSceneAsync(loadingScene);
        StartCoroutine(loadScenes());

    }


    private IEnumerator loadScenes(){
        
        Debug.Log("Loading Scene Done");
        foreach(SceneField scene in scenetoLoad){
            Debug.Log(scenetoLoad.Count);
            loadingProcess.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            
        }
        
        foreach(AsyncOperation operation in loadingProcess){
            operation.allowSceneActivation = false;
            while(operation.progress < 0.9f){
                yield return null;
            }
        }
        //Data recopilation for later
        
        foreach(AsyncOperation operation in loadingProcess){
            operation.allowSceneActivation = true;
        }
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenetoLoad[0]));
        SceneManager.UnloadSceneAsync(loadingScene);
        Destroy(gameObject);

    } 
}

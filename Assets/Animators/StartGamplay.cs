// using System.Collections.Generic;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class StartGamplay : MonoBehaviour
// {
//     [SerializeField]
//     private SceneField loadingScene;
//     [SerializeField]
//     private List<SceneField> scenetoLoad;

//     private List<AsyncOperation> loadingProcess;
//     private AsyncOperation loadingloading ;

//     void Awake()
//     {
//         loadingProcess = new List<AsyncOperation>();
//         DontDestroyOnLoad(gameObject);
//     }
//     void Start()
//     {
        
//     }
//     void OnEnable(){
//         ChangeScene();
//     }

//     // Update is called once per frame
//     public void ChangeScene()
//     {
//         loadingloading = SceneManager.LoadSceneAsync(loadingScene);
//         StartCoroutine(loadScenes());

//     }


//     private IEnumerator loadScenes(){
        
//         Debug.Log("Loading Scene Done");
//         foreach(SceneField scene in scenetoLoad){
//             Debug.Log($"Escenas Activas: {scenetoLoad.Count}");
//             loadingProcess.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            
//         }
        
//         foreach(AsyncOperation operation in loadingProcess){
//             operation.allowSceneActivation = false;
//             while(operation.progress < 0.9f){
//                 yield return null;
//                 Debug.Log(operation.progress); //Progreso de escenas
//             }
//         }
//         //Data recopilation for later
        
//         foreach(AsyncOperation operation in loadingProcess){
//             operation.allowSceneActivation = true;
//             Debug.Log("Loading Scene Done"); //Cargar escena
//             yield return null;
//         }
        
//         SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenetoLoad[0]));
//         Debug.Log(scenetoLoad[0]); //Cargar escena
//         SceneManager.UnloadSceneAsync(loadingScene);
//         Debug.Log("Loading Scene Done__Killing"); //Cargar escena
//         Destroy(gameObject);

//     } 
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameplay : MonoBehaviour
{
    [SerializeField]
    private SceneField loadingScene;
    
    [SerializeField]
    private List<SceneField> scenesToLoad;

    private List<AsyncOperation> loadingProcesses;
    private loadingGIF gif;

    void Awake()
    {
        loadingProcesses = new List<AsyncOperation>();
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        ChangeScene();
    }

    public void ChangeScene()
    {
        StartCoroutine(LoadEverything());
    }

    private IEnumerator LoadEverything()
    {

        
        // 1. Cargar loading scene
        AsyncOperation loading = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Single);
        yield return loading; // Esperar a que termine

        Debug.Log("Loading scene loaded");

        yield return new WaitForSeconds(0.5f); // Esperar medio segundo (WebGL stability)

        //carga gif
        gif = Object.FindFirstObjectByType<loadingGIF>();
        if (gif == null)
        {
            Debug.LogWarning("No se encontró loadingGIF en la escena. ¿Olvidaste instanciarlo?");
        }

        // 2. Cargar todas las demás escenas como Additive
        foreach (SceneField scene in scenesToLoad)
        {
            Debug.Log($"Preparando cargar escena: {scene}");
            AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            loadingProcesses.Add(op);
            yield return op; // Esperar cada escena
        }

        // 3. Esperar para asegurarse
        yield return new WaitForSeconds(0.5f);

        //eliminar gif hbdskjv bczh
        if (gif != null)
        {
            gif.Hide(); // Esto inicia el desvanecimiento del GIF
        }

        // Aquí es donde ponemos la verificación antes de activar
        if (scenesToLoad != null && scenesToLoad.Count > 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenesToLoad[0]));
            Debug.Log($"Escena activa: {scenesToLoad[0]}");
        }
        else
        {
            Debug.LogError("No hay escenas para activar. Verifica que 'scenesToLoad' tenga escenas asignadas.");
        }

        // Luego sigues descargando la loadingScene
        AsyncOperation unload = SceneManager.UnloadSceneAsync(loadingScene);
        yield return unload;

        Destroy(gameObject);
        }
}

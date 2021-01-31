using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class APIController : MonoBehaviour
{
    private const string URL = "https://api.github.com/users/LeviathanDominator";
    private const string reposURL = URL + "/repos";
    public Text nameField;
    public Text publicReposField;
    public Text locationField;
    public List<GameObject> list;
    public GameObject project;

    void Start()
    {
        StartCoroutine(RequestData());
        StartCoroutine(RequestRepos());
    }

    IEnumerator RequestData()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
        yield return request.Send();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            JSONNode data = JSON.Parse(request.downloadHandler.text);
            /*foreach (JSONNode day in data)
            {
                nameField.text += day.Value;
            }*/
            nameField.text = data["name"].Value;
            publicReposField.text = "Repositorios: " + data["public_repos"].Value;
            locationField.text = "Localización: " + data["location"].Value;
        }
    }

    IEnumerator RequestRepos()
    {
        UnityWebRequest request = UnityWebRequest.Get(reposURL);
        yield return request.Send();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            JSONNode data = JSON.Parse(request.downloadHandler.text);
            list = new List<GameObject>();
            var x = project.transform.position.x;
            foreach (JSONNode row in data)
            {
                    var newProject = project;
                    x+=10; // Moves project to the right
                    newProject = Instantiate(project);
                    newProject.transform.position = new Vector3(x, project.transform.position.y, project.transform.position.z);
                    var name = GameObject.FindGameObjectWithTag("NameCanvas").GetComponent<Text>();
                    name.text = row["name"].Value;
                    list.Add(newProject);
                    newProject.SetActive(false);
            }
            list.ForEach(element => element.SetActive(true));
        }
    }


}

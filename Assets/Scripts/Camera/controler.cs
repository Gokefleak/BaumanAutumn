using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class controler : MonoBehaviour
{
    public string[] lines;
    public List<string> history = new List<string> { };
    private string pattern = @"(.*?): (.*)";
    private string comand = @"/(.*?) (.*)";
    private string answer = @"'(.*?)': (.*)";
    public bool nextphrase = true;
    public int currentIndex = 0;
    public int oldIndex = 0;
    public TextMeshPro generoustext;
    public TextMeshPro nametext;
    private float typeSpeed = 0.05f;
    public float startSpeed = 0.05f;
    private string[] currentParts;
    public int allIndex = 0;
    public int nowIndex = 0;
    public bool isDisplaying = false;
    private string[] words;
    public bool nowprint = false;
    public bool test = false;
    public bool test1 = false;
    private bool pause = false;
    public bool printAll = true;

    //Создание объектов
    public Dictionary<string, GameObject> allObjectsDictionary;
    public List<GameObject> allObjects;
    public Vector3 spawnPosition = new Vector3(0f, 0f, 0f);

    //Создание спрайтов
    public Dictionary<string, Sprite> spriteDictionary;
    public List<Sprite> sprites;

    //Дрожание камеры
    public CameraShake cameraShake;

    //Музыка
    public Dictionary<string, AudioClip> audioClipDictionary;
    public List<AudioClip> audioClips;
    public Dictionary<string, AudioClip> sfxClipDictionary;
    public List<AudioClip> sfxClips;
    public AudioSource audioSourcePrefab;
    public int numberOfAudioSources = 10;
    private List<AudioSource> audioSources = new List<AudioSource>();

    public string targetTag = "button";

    //Словарь для фонов и персонажей
    public Dictionary<string, (float x, float y, float z, float scale, float rotationX, float rotationY, float rotationZ)> spawnDictionary = new Dictionary<string, (float x, float y, float z, float scale, float rotationX, float rotationY, float rotationZ)>();

    public Dictionary<string, int> pointDictionary = new Dictionary<string, int>();
    void Start()
    {
        for (int i = 0; i < numberOfAudioSources; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            audioSources.Add(source);
        }
        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }
        typeSpeed = startSpeed;
        fileName = "days/day0";
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        string fileContent = textAsset.text;
        lines = fileContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
     
        allObjectsDictionary = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in allObjects)
        {
            if (prefab != null)
            {
                allObjectsDictionary[prefab.name] = prefab;
            }
        }


        audioClipDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in audioClips)
        {
            if (clip != null)
            {
                audioClipDictionary[clip.name] = clip;
            }
        }
        sfxClipDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in sfxClips)
        {
            if (clip != null)
            {
                sfxClipDictionary[clip.name] = clip;
            }
        }
        spriteDictionary = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in sprites)
        {
            if (sprite != null)
            {
                spriteDictionary[sprite.name] = sprite;
            }
        }
    }
    void CheckObjectUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.CompareTag(targetTag) && !hit.collider.CompareTag("menu") && !hit.collider.CompareTag("save"))
            {
                if (boostskip)
                {
                    if (boostskipause == 1)
                    {
                        boostskipause = 10f;
                    }
                    else
                    {
                        boostskipause = 1f;
                    }
                    boostskip = false;
                }
                Game();
            }
        }
        else
        {
            if (boostskip)
            {
                if (boostskipause == 1)
                {
                    boostskipause = 10f;
                }
                else
                {
                    boostskipause = 1f;
                }
                boostskip = false;
            }
            Game();
        }
    }
    void Game()
    {
        if (rand)
        {
            rand = false;
        }
        if (pause == false)
        {
            nextphrase = false;
            if (nowIndex == (allIndex - 1) && nowIndex != 0)
            {
                isDisplaying = false;
                allIndex = 1;
                nowIndex = 0;
                currentIndex++;
            }
            if (nowprint == true && printAll == false)
            {
                printAll = true;
            }
            if ((currentIndex < lines.Length) && isDisplaying == false && nowprint == false)
            {
                printAll = false;
                generoustext.text = "";
                nametext.text = "";
                oldtext = "";
                ProcessLine(lines[currentIndex]);

            }
            else if ((nowIndex < allIndex) && isDisplaying == true && nowprint == false)
            {
                oldtext = generoustext.text;
                printAll = false;
                nowIndex++;
                ProcessLine(lines[currentIndex]);

            }
        }
        
    }
    public bool menuOpen = false;
    void Update()
    {
        if (!mainMenu)
        {
            if (!menuOpen && !question && Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width && Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                CheckObjectUnderCursor();
            }
            if (!menuOpen && nextphrase == true || boostskip && !question)
            {
                question = false;
                Game();
            }
        }
    }
    public bool mainMenu = true;
    public bool loadsaves = false;
    public bool boostskip = false;
    public float boostskipause = 1f;
    private IEnumerator ProcessStringArray()
    {
        foreach (string word in words)
        {
            yield return StartCoroutine(ProcessString(word));
        }

        typeSpeed = startSpeed;
        nowprint = false;
    }

    private IEnumerator ProcessString(string word)
    {
        int length = word.Length;
        string colorTag = "<color=#FFFFFF00>";

        for (int i = 0; i < length; i++)
        {
            if (printAll && !loadsaves)
            {
                generoustext.text = word;
                nowprint = false;
                yield break;
            }
            if (i == length - 1)
            {
                generoustext.text = word;
            }
            else
            {
                string before = word.Substring(0, i);
                string after = word.Substring(i);
                generoustext.text = before + colorTag + after + "</color>";
            }

            yield return new WaitForSeconds(typeSpeed);
            if (loadsaves)
            {
                nowprint = false;
                nextphrase = true;
                loadsaves = false;
                yield break;
            }
        }
        nowprint = false;
    }

    public string oldtext = "";
    private IEnumerator DisplayText(string word)
    {
        if (allIndex == 1 && !rand)
        {
            currentIndex++;
            StartCoroutine(ProcessStringArray());
        }
        else
        {
            if (!rand) 
            { 
                isDisplaying = true;
            }
            int length = word.Length;
            string colorTag = "<color=#FFFFFF00>";
            for (int i = 0; i < length; i++)
            {
                if (printAll)
                {
                    generoustext.text = oldtext + word;
                    nowprint = false;
                    
                    yield break;
                }

                if (i == length - 1)
                {
                    generoustext.text = oldtext + word;
                }
                else
                {
                    string before = word.Substring(0, i);
                    string after = word.Substring(i);

                    generoustext.text = oldtext + before + colorTag + after + "</color>";
                }
                yield return new WaitForSeconds(typeSpeed);
                if (loadsaves)
                {
                    nowprint = false;
                    nextphrase = true;
                    loadsaves = false;
                    yield break;
                }
            }
            nowprint = false;
        }
        
        
    }
    public bool question = false;
    public string saveQuestion = "";
    public GameObject questionPrefab;
    public GameObject trueAnswerPrefab;
    public GameObject falseAnswerPrefab;
    public float verticalSpacing = 1.5f;
    public bool show = false;
    public bool hide = false;
    public string fileName = "";
    public string clipName = "";
    public float VolumeNum = 0;
    public float SpeedNum = 0;
    public bool rand = false;
    public float[] RGB = { 1f, 1f, 1f };
    public string oldName = "";
    void ProcessLine(string line)
    {
        Match match = Regex.Match(line, pattern);
        if (match.Success)
        {
            nowprint = true;
            string result1 = match.Groups[1].Value;
            string after = match.Groups[2].Value;
            string [] result = result1.Split('|');
            nametext.text = result[0];
            if (result.Length == 4)
            {
                RGB = new float[] { float.Parse(result[1]) / 255f, float.Parse(result[2]) / 255f, float.Parse(result[3]) / 255f };
                
                nametext.color = new Color(RGB[0], RGB[1], RGB[2]);
                //nametext.color = new Color(1, 1, 1);
            }
            else
            {
                RGB = new float[] { 1f, 1f, 1f };
                nametext.color = new Color(RGB[0], RGB[1], RGB[2]);
            }
            
            words = after.Split('*');
            allIndex = words.Length;
            if (oldtext.Contains("<color=#FFFFFF00>") && oldtext.Contains("</color>"))
            {
                nowIndex = 0;
                oldtext = "";
            }
            
            if (oldName != result[0])
            {
                history.Add("");
            }
            oldName = result[0];
            if (result[0]!= "" && nowIndex == 0)
            {
                history.Add(result[0]);
            }

            if (nowIndex == 0)
            {
                history.Add(after.Replace("*", ""));
            }
            StartCoroutine(DisplayText(words[nowIndex]));
        }
        else
        {
            currentIndex++;
            Match com = Regex.Match(line, comand);
            if (com.Success)
            {
                string namecomand = com.Groups[1].Value;
                string action = com.Groups[2].Value;
                if (namecomand == "go")
                {
                    oldIndex = currentIndex;
                    currentIndex = int.Parse(action)-1;
                    nextphrase = true;
                }
                else if (namecomand == "return")
                {
                    currentIndex = oldIndex;
                    nextphrase = true;
                }
                else if (namecomand == "window")
                {
                    string patternWindow = @"(.*?); (.*)";
                    Match durationWindow = Regex.Match(action, patternWindow);
                    if (durationWindow.Success)
                    {
                        string act = durationWindow.Groups[1].Value;
                        string timer = durationWindow.Groups[2].Value;
                        float timerr = float.Parse(timer);
                        if (act == "hide")
                        {
                            nextphrase = true;
                            test hideWindow = GameObject.Find("dialogWindows").GetComponent<test>();
                            hide = true;
                            show = false;
                            hideWindow.hide = hide;
                            hideWindow.fadeDuration = timerr/boostskipause;
                        }
                        else if (act == "show")
                        {
                            nextphrase = true;
                            test hideWindow = GameObject.Find("dialogWindows").GetComponent<test>();
                            show = true;
                            hide = false;
                            hideWindow.show = show;
                            hideWindow.fadeDuration = timerr/boostskipause;
                        }
                    }
                }
                else if (namecomand == "random")
                {
                    string pattern1 = @"(.*?):(.*)";
                    Match randomWord = Regex.Match(action, pattern1);
                    if (randomWord.Success)
                    {
                        nowprint = true;
                        string namePerson = randomWord.Groups[1].Value;
                        string storyText = randomWord.Groups[2].Value;
                        string[] phraseArray = storyText.Split(';');
                        int randomIndex = Random.Range(0, phraseArray.Length);
                        string randomPhrase = phraseArray[randomIndex].Trim();
                        rand = true;
                        StartCoroutine(DisplayText(randomPhrase));
                    }

                }
                else if (namecomand == "if")
                {
                    string inputString = action;
                    string[] parts = inputString.Split('|');
                    int trueMean = int.Parse(parts[1]);
                    int falseMean = int.Parse(parts[2]);
                    CheckCondition(parts[0], trueMean, falseMean);

                }
                else if (namecomand == "pause")
                {
                    float pauseTime = float.Parse(action);
                    StartCoroutine(WaitAndPrint(pauseTime/boostskipause));
                    nextphrase = true;

                }
                else if (namecomand == "question1")
                {
                    saveQuestion = action;
                    CreateQuestion(saveQuestion);

                }
                else if (namecomand == "end")
                {
                    nextphrase = true;
                    fileName = "days/" + action;
                    TextAsset textAsset = Resources.Load<TextAsset>(fileName);
                    string fileContent = textAsset.text;
                    lines = fileContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                    currentIndex = 0;
                }
                else if (namecomand == "shake")
                {
                    nextphrase = true;
                    string patternShake = @"(.*?); (.*)";
                    Match durationAmplitude = Regex.Match(action, patternShake);
                    if (durationAmplitude.Success)
                    {
                        string duration = durationAmplitude.Groups[1].Value;
                        float durationNum = float.Parse(duration);

                        string amplitude = durationAmplitude.Groups[2].Value;
                        float amplitudeNum = float.Parse(amplitude);

                        cameraShake.Shake(durationNum/boostskipause, amplitudeNum);
                    }
                }
                else if (namecomand == "sfx")
                {
                    nextphrase = true;
                    string[] parts = action.Split("; ");
                    string sfxName = parts[0];
                    float Volume = float.Parse(parts[1]);
                    float Speed = float.Parse(parts[2]);
                    PlaySfx(sfxName, Volume, Speed * boostskipause);
                }
                else if (namecomand == "spawnObj")
                {
                    nextphrase = true;
                    string[] parts = action.Split("; ");
                    string spawnName = parts[0];
                    float xPosition = float.Parse(parts[1]);
                    float yPosition = float.Parse(parts[2]);
                    float zPosition = float.Parse(parts[3]);
                    float scale = float.Parse(parts[4]);
                    float timer = float.Parse(parts[5]);
                    if (parts.Length > 8)
                    {
                        
                        float xRotation = float.Parse(parts[6]);
                        float yRotation = float.Parse(parts[7]);
                        float zRotation = float.Parse(parts[8]);
                        spawnPosition = new Vector3(xPosition, yPosition, zPosition);
                        spawnDictionary[spawnName] = (xPosition, yPosition, zPosition, scale, xRotation, yRotation, zRotation);
                        CreateObject(spawnName, spawnPosition, scale, timer, xRotation, yRotation, zRotation);
                    }
                    else
                    {
                        float xRotation = 0f;
                        float yRotation = 0f;
                        float zRotation = 0f;
                        spawnPosition = new Vector3(xPosition, yPosition, zPosition);
                        spawnDictionary[spawnName] = (xPosition, yPosition, zPosition, scale, xRotation, yRotation, zRotation);
                        CreateObject(spawnName, spawnPosition, scale, timer, xRotation, yRotation, zRotation);
                    }
                }
                else if (namecomand == "move")
                {
                    nextphrase = true;
                    string patternShake = @"(.*?); (.*); (.*); (.*); (.*)";
                    Match positionXYZ = Regex.Match(action, patternShake);
                    if (positionXYZ.Success)
                    {
                        string moveName = positionXYZ.Groups[1].Value;
                        string x = positionXYZ.Groups[2].Value;
                        float xPosition = float.Parse(x);
                        string y = positionXYZ.Groups[3].Value;
                        float yPosition = float.Parse(y);
                        string z = positionXYZ.Groups[4].Value;
                        float zPosition = float.Parse(z);
                        string timer = positionXYZ.Groups[5].Value;
                        float speed = float.Parse(timer);
                        Vector3 movePosition = new Vector3(xPosition, yPosition, zPosition);
                        var currentValue = spawnDictionary[moveName];
                        spawnDictionary[moveName] = (xPosition, yPosition, zPosition, currentValue.scale, currentValue.rotationX, currentValue.rotationY, currentValue.rotationZ);
                        StartCoroutine(MoveObjectToPosition(moveName, movePosition, speed / boostskipause));
                    }
                }
                else if (namecomand == "scale")
                {
                    nextphrase = true;
                    string patternShake = @"(.*?); (.*); (.*)";
                    Match positionXYZ = Regex.Match(action, patternShake);
                    if (positionXYZ.Success)
                    {
                        string moveName = positionXYZ.Groups[1].Value;
                        string scaleStr = positionXYZ.Groups[2].Value;
                        float scale = float.Parse(scaleStr);
                        string timer = positionXYZ.Groups[3].Value;
                        float speed = float.Parse(timer);
                        Vector3 ScaleVector= new Vector3 (scale, scale, scale);
                        var currentValue = spawnDictionary[moveName];
                        spawnDictionary[moveName] = (currentValue.x, currentValue.y, currentValue.z, scale, currentValue.rotationX, currentValue.rotationY, currentValue.rotationZ);
                        StartCoroutine(ScaleObjectToScale(moveName, ScaleVector, speed / boostskipause));
                        
                    }
                }
                else if (namecomand == "rotation")
                {
                    nextphrase = true;
                    string patternShake = @"(.*?); (.*); (.*); (.*); (.*)";
                    Match positionXYZ = Regex.Match(action, patternShake);
                    if (positionXYZ.Success)
                    {
                        string rotationName = positionXYZ.Groups[1].Value;
                        string x = positionXYZ.Groups[2].Value;
                        float xRotation = float.Parse(x);
                        string y = positionXYZ.Groups[3].Value;
                        float yRotation = float.Parse(y);
                        string z = positionXYZ.Groups[4].Value;
                        float zRotation = float.Parse(z);
                        string timer = positionXYZ.Groups[5].Value;
                        float speed = float.Parse(timer);
                        var currentValue = spawnDictionary[rotationName];
                        spawnDictionary[rotationName] = (currentValue.x, currentValue.y, currentValue.z, currentValue.scale, xRotation, yRotation, zRotation);
                        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
                        StartCoroutine(RotateObjectToRotation(rotationName, rotation, speed / boostskipause));
                    }
                }
                else if (namecomand == "destroyObj")
                {
                    nextphrase = true;
                    string patternDestroy = @"(.*?); (.*)";
                    Match Destroyp = Regex.Match(action, patternDestroy);
                    if (Destroyp.Success)
                    {
                        string Name = Destroyp.Groups[1].Value;
                        string Timer = Destroyp.Groups[2].Value;
                        float Timerr = float.Parse(Timer);
                        ICollection<string> keys = spawnDictionary.Keys;

                        

                        spawnDictionary.Remove(Name);
                        foreach (string key in keys)
                        {
                            Debug.Log("Key: " + key);
                        }
                        DeleteObj(Name, Timerr/boostskipause);
                    }
                    
                }
                else if (namecomand == "music")
                {
                    nextphrase = true;
                    string patternSound = @"(.*?); (.*); (.*)";
                    Match Soundtreak = Regex.Match(action, patternSound);
                    if (Soundtreak.Success)
                    {
                        clipName = Soundtreak.Groups[1].Value;
                        string Volume = Soundtreak.Groups[2].Value;
                        VolumeNum = float.Parse(Volume);
                        string Speed = Soundtreak.Groups[3].Value;
                        SpeedNum = float.Parse(Speed);
                        PlayMusic(clipName, VolumeNum, SpeedNum*boostskipause);
                    }
                }
                else if (namecomand == "offMusic")
                {
                    nextphrase = true;
                    clipName = "";
                    VolumeNum = 0f;
                    SpeedNum = 0f;
                    StopMusicByName(action);
                }
                /*else if (namecomand == "settigsMusic")
                {
                    nextphrase = true;
                    string patternSound = @"(.*?); (.*)";
                    Match Soundtreak = Regex.Match(action, patternSound);
                    if (Soundtreak.Success)
                    {
                        string Volume = Soundtreak.Groups[1].Value;
                        VolumeNum = float.Parse(Volume);
                        string Speed = Soundtreak.Groups[2].Value;
                        SpeedNum = float.Parse(Speed)*boostskipause;
                        audioSource.volume = Mathf.Clamp(VolumeNum, 0f, 3f);
                        audioSource.pitch = Mathf.Clamp(SpeedNum, 0.1f, 3f);
                    }
                }*/
            }
        }
    }

    void CheckCondition(string condition, int trueMean, int falseMean)
    {
        if (EvaluateCondition(condition, pointDictionary))
        {
            oldIndex = currentIndex;
            currentIndex = trueMean - 1;
            nextphrase = true;
        }
        else
        {
            oldIndex = currentIndex;
            currentIndex = falseMean - 1;
            nextphrase = true;
        }
    }

    bool EvaluateCondition(string condition, Dictionary<string, int> variables)
    {
        Regex regex = new Regex(@"(\w+)\s*(>=|<=|>|<|==|!=)\s*(\d+)");
        Match match = regex.Match(condition);

        if (match.Success)
        {
            string variable = match.Groups[1].Value;
            string operation = match.Groups[2].Value;
            int conditionValue = int.Parse(match.Groups[3].Value);

            if (variables.ContainsKey(variable))
            {
                int variableValue = variables[variable];

                switch (operation)
                {
                    case ">=":
                        return variableValue >= conditionValue;
                    case "<=":
                        return variableValue <= conditionValue;
                    case ">":
                        return variableValue > conditionValue;
                    case "<":
                        return variableValue < conditionValue;
                    case "==":
                        return variableValue == conditionValue;
                    case "!=":
                        return variableValue != conditionValue;
                    default:
                        return false;
                }
            }
        }
        return false;
    }
    public void StopMusicByName(string trackName)
    {
        // Перебираем все AudioSource в пуле
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying && source.clip != null && source.clip.name == trackName)
            {
                source.Stop();
            }
        }
    }



    public void CreateSprite(string spriteName, Vector3 spawnPosition)
    {

        if (spriteDictionary.TryGetValue(spriteName, out Sprite sprite))
        {
            GameObject newSpriteObject = new GameObject(spriteName);
            SpriteRenderer spriteRenderer = newSpriteObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            newSpriteObject.transform.position = spawnPosition;
        }


    }
    public void CreateQuestion(string question1)
    {
        oldIndex = currentIndex;
        question = true;
        string inputString = question1;
        string[] parts = inputString.Split(';');
        int numberOfAnswers = int.Parse(parts[0].Trim());
        List<string> answers = new List<string>();
        for (int i = 1; i < parts.Length; i++)
        {
            answers.Add(parts[i].Trim());
        }
        float yOffset = 2.5f;
        foreach (string answer in answers)
        {
            GameObject obj;
            if (answer.StartsWith("true"))
            {
                obj = Instantiate(trueAnswerPrefab, new Vector3(0, yOffset, 1), Quaternion.identity);
                TrueAnswer point = obj.GetComponent<TrueAnswer>();
                point.name = answer.Split('|')[2];
                point.count = int.Parse(answer.Split('|')[3]);
                point.index = int.Parse(answer.Split("|")[4]) - 1;


            }
            else if (answer.StartsWith("question"))
            {
                history.Add("");
                questionbool = true;
                obj = Instantiate(questionPrefab, new Vector3(0, 3.3f, 1), Quaternion.identity);
            }
            else
            {
                obj = Instantiate(falseAnswerPrefab, new Vector3(0, yOffset, 1), Quaternion.identity);
                FalseAnswer point = obj.GetComponent<FalseAnswer>();
                point.name = answer.Split('|')[2];
                point.count = int.Parse(answer.Split('|')[3]);
                point.index = int.Parse(answer.Split("|")[4]) - 1;
            }
            test showObject = obj.GetComponent<test>();
            showObject.Initialize(1, true);
            TMP_Text textMeshPro = obj.GetComponentInChildren<TMP_Text>();
            if (textMeshPro != null)
            {
                string answerText = answer.Split('|')[1];
                answerText = answerText.Replace("\\n", "\n");
                if (questionbool)
                {
                    questionbool = false;
                    history.Add("Выбор: " + answerText);
                }
                textMeshPro.text = answerText;
            }

            yOffset -= verticalSpacing;
        }
    }
    public bool questionbool = false;
    IEnumerator WaitAndPrint(float pauseTime)
    {
        pause = true;
        yield return new WaitForSeconds(pauseTime);
        pause = false;
    }
    public void PlaySfx(string sfxName, float Volume, float Speed)
    {
        if (sfxClipDictionary.TryGetValue(sfxName, out AudioClip clip))
        {
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.clip = clip;
                availableSource.loop = false;
                availableSource.volume = Mathf.Clamp(Volume, 0f, 3f);
                availableSource.pitch = Mathf.Clamp(Speed, 0.1f, 3f);
                availableSource.Play();
            }
        }
    }
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
    public void PlayMusic(string clipName, float VolumeNum, float SpeedNum)
    {

        if (audioClipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.clip = clip;
                availableSource.loop = true;
                availableSource.volume = Mathf.Clamp(VolumeNum, 0f, 3f);
                availableSource.pitch = Mathf.Clamp(SpeedNum, 0.1f, 3f);
                availableSource.Play();
            }
        }
    }
    public void DeleteObj(string objectName, float timer)
    {
        Objects showObject = GameObject.Find(objectName + "(Clone)").GetComponent<Objects>();
        showObject.fadeDuration = timer;
        showObject.hide = true;
        GameObject obj = GameObject.Find(objectName + "(Clone)");
        Destroy(obj, timer);
    }
    public IEnumerator RotateObjectToRotation(string objectName, Quaternion targetRotation, float time)
    {
        GameObject targetObject = GameObject.Find(objectName + "(Clone)");
        Quaternion startRotation = targetObject.transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            targetObject.transform.rotation = Quaternion.RotateTowards(startRotation, targetRotation, (Quaternion.Angle(startRotation, targetRotation) / time) * elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.rotation = targetRotation;
    }
    IEnumerator MoveObjectToPosition(string objectName, Vector3 targetPosition, float time)
    {
        Debug.Log(objectName);
        GameObject targetObject = GameObject.Find(objectName + "(Clone)");
        Vector3 startPosition = targetObject.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            targetObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = targetPosition;
    }
    IEnumerator ScaleObjectToScale(string objectName, Vector3 targetScale, float time)
    {
        GameObject targetObject = GameObject.Find(objectName + "(Clone)");
        Vector3 startScale = targetObject.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.localScale = targetScale; // убедиться, что объект точно достиг целевого масштаба
    }
    public void CreateObject(string objectName, Vector3 spawnPosition, float scale, float timer, float rotationX, float rotationY, float rotationZ)
    {

        if (allObjectsDictionary.TryGetValue(objectName, out GameObject prefab))
        {
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
            GameObject newObject = Instantiate(prefab, spawnPosition, rotation);
            Objects showObject = newObject.GetComponent<Objects>();
            if (scale != 0)
            {
                showObject.transform.localScale = new Vector3 (scale, scale, scale);
            }
            showObject.Initialize(timer, true);
        }
    }
}

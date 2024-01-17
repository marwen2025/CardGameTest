using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AnimalsContainer _animalContainer;
    [SerializeField] private List<Animal> _allAnimals = new List<Animal>();
    [SerializeField] private int _rounds = 0;
    [SerializeField] private int _attempts;
    [SerializeField] private float _timer = 60f;
    [SerializeField] private GameObject _timerMeshPro;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _progressBarText;
    [SerializeField] private Image[] _animalImagesContainer= new Image[3] ;
    [SerializeField] private TextMeshProUGUI[] _animalNamesContainer = new TextMeshProUGUI[3];
    private List<Animal> _loadedAnimals = new List<Animal>();
    const int MaxloadedSprites = 3;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
    private int _stars;
    private int _coins;
    [SerializeField] private List<Image> _starImages;
    [SerializeField] private Button _repeatButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _checkResultButton;
    [SerializeField] private Button _looseRepeatButton;
    [SerializeField] private List<LineRendrerHandler> _lineRendererHandlerList;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _gameOver;

    void Start()
    {
        _progressBar.fillAmount = 0.0f;
        _progressBarText.text = "(Progress bar )" + _rounds * 10 + "%";
        _allAnimals.AddRange(_animalContainer.Animals);
        loadAnimals();
        setUpAnimals();
        _attempts = 1;
        _coinsText.text = _coins + " (Coins)";
        _checkResultButton.onClick.AddListener(onResultCHeck);
        _nextButton.onClick.AddListener(setUpNextLevel);
        _looseRepeatButton.onClick.AddListener(onRepeat);
        _repeatButton.onClick.AddListener(onRepeat);
        
        _gameOver.text = "You Lose !";

    }
    private void Update()
    {
        Timer();

    }
    private void coinCollected()
    {
        
            switch (_stars)
            {
                case 0:
                _coins +=10;
                break;
                case 1:
                _coins +=30;
                break;
                case 2:
                _coins +=60;
                break;
                case 3:
                _coins +=100;
                break;
            }
    }

    private void setUpNextLevel()
    {
        _winPanel.SetActive(false);
        for (int i = 0; i < _stars; i++)
        {
            _starImages[i].color = Color.white;
        }
        //ken msakkra xD 
        _repeatButton.gameObject.SetActive(true);
        loadAnimals();
        setUpAnimals();
        _lineRendererHandlerList[1].toggleCanDrag(true);
        coinCollected();
        _coinsText.text = _coins + " (Coins)";
        _attempts = 1;
        _rounds += 1;
        _timer = 60f;
        _progressBar.fillAmount =  (float)_rounds/ 10;
        _progressBarText.text = "(Progress bar )"+_rounds*10+"%";
        gameOver();
        


    }
    private void onRepeat()
    {
        setUpAnimals();
        _losePanel.SetActive(false);
        _winPanel.SetActive(false);
        resetLines();
        _timer = 60f;

    }
    private void resetLines()
    {
        foreach (var line in _lineRendererHandlerList)
        {
            line._lineRenderer.positionCount = 0;
            line._lineRenderer.positionCount = 2;
            line.toggleCanDrag(true) ;
            line.verif=false;

        }
    }
    private void setUpAnimals()
    {
        List<int> usedInexes = new List<int>();
        int shufledIndex;
        for (int i = 0; i < MaxloadedSprites; i++)
        { 
            _animalImagesContainer[i].sprite = _loadedAnimals[i].animalSprite;
            _animalImagesContainer[i].gameObject.name = _loadedAnimals[i].name;

            do
            {
                shufledIndex = Random.Range(0, MaxloadedSprites);
            } while ( usedInexes.Contains(shufledIndex));

            _animalNamesContainer[shufledIndex].text = _loadedAnimals[i].name;
            _animalNamesContainer[shufledIndex].gameObject.name = _loadedAnimals[i].name;
            usedInexes.Add(shufledIndex);

        }

    }


    private bool gameOver()
    {
        if (_rounds == 10)
        {
            Debug.Log("wfet sakkarna");
            _gameOver.text = "Game Is Over";
            _losePanel.SetActive(true);
            _looseRepeatButton.onClick.RemoveAllListeners();
            _looseRepeatButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            return true;
        }
        return false;
    }
    private void loadAnimals()
    {   
        
        _loadedAnimals.Clear();
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, _allAnimals.Count);
            _loadedAnimals.Add(_allAnimals[index]);
            _allAnimals.RemoveAt(index);
            //Debug.Log("loaded animals: " + _loadedAnimals.Count);
            //Debug.Log("all animals: " + _allAnimals.Count);
        }
    }


    public void Timer()
    {

        if (_timer > 1 )
        {
            _timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timer / 60);
            int seconds = Mathf.FloorToInt(_timer % 60);
            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
            //Debug.Log(timeText);
            _timerMeshPro.GetComponent<TextMeshProUGUI>().SetText(timeText);

        }
        else
        {
            _losePanel.SetActive(true);
            resetLines();
        }

    }
    
    private void onResultCHeck()
    {

        bool _roundWon = true;
        foreach (var lineHandler in _lineRendererHandlerList){
            if (lineHandler.verif==false)
            {
                _attempts += 1;
                _losePanel.SetActive(true);
                _roundWon=false;
                break;

            }
            
        }
        if (_roundWon)
        {
            //reset Colors
            for (int i = 0; i < 3; i++)
            {
                _starImages[i].color = Color.white;
            }

            if (_attempts == 1)
            {
                _stars = 3;
                _repeatButton.gameObject.SetActive(false);
            }
            else if(_attempts<4) 
            {
                _stars = 2;
            }
            else if (_attempts<6)
            {
                _stars = 1;
            }
            /*else
            {
                _stars = 0;
            }*/
            //set Colors
            for (int i = 0; i < _stars; i++)
            {
                _starImages[i].color = Color.yellow;
            }           
            _winPanel.SetActive(true);

        }
        resetLines();




    }
     


}

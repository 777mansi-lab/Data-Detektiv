using UnityEngine;
using TMPro;

public class WordGameController : MonoBehaviour
{
    [System.Serializable]
    public class WordQuestion
    {
        public string wordText;
        public string[] options;
        public int correctIndex;

        public string situationText;
        public string[] situationOptions;
        public int correctSituationIndex;
    }

    [System.Serializable]
    public class WordQuestionList
    {
        public WordQuestion[] questions;
    }

    [System.Serializable]
    public class TutorialStep
    {
        public string tutorialText;
    }

    [System.Serializable]
    public class TutorialList
    {
        public TutorialStep[] steps;
    }

    public GameObject wordPanel;
    public GameObject situationPanel;
    public GameObject wonPanel;
    public GameObject tutorialPanel;
    public GameObject wrongPanel;

    public TMP_Text wrongText;

    public TMP_Text wordText;
    public TMP_Text[] optionTexts;

    public TMP_Text situationText;
    public TMP_Text[] situationOptionTexts;

    public TMP_Text pointText;
    public TMP_Text wonText;
    public TMP_Text tutorialText;

    private WordQuestion[] questions;
    private TutorialStep[] tutorialSteps;

    private int currentIndex = 0;
    private int tutorialIndex = 0;
    private int points = 0;

    public AudioSource rightAnswer;
    public AudioSource wrongAnswer;

    void Start() //kører når spillet starter
    {
        wordPanel.SetActive(false); //false gør panel usynlig
        situationPanel.SetActive(false);
        wonPanel.SetActive(false);
        tutorialPanel.SetActive(true); //true gør dem synlig
        wrongPanel.SetActive(false);
        //kalder 2 funktioner efter de andre er sat som de skal være
        LoadTutorial(); 
        ShowTutorial();
    }

    void LoadTutorial() //funktion der skal få tutorial tekst til at stå i tutorialen når man går frem og tilbage i den
    {
        TextAsset json = Resources.Load<TextAsset>("TutorialText"); //Loader en TextAsset, tekstfil, som hedder TutorialText der er json fil fra Resource mappen
        TutorialList list = JsonUtility.FromJson<TutorialList>(json.text); //JsonUtility er Unity indbygget ting som kan parse json filer. Parse konverterer json fil til et C# objekt som kan arbejdes med. Tutorial list siger bare at det skal laves om til den specifikke data type.
        tutorialSteps = list.steps; //Gemmer data fra C# objekt fra over ind i tutorialSteps
    }

    void ShowTutorial()
    {
        tutorialText.text = tutorialSteps[tutorialIndex].tutorialText;
        /*
        tutorialSteps er et array med alle tutoria trin. tutorialIndex holder styr på hvilket trin man er nået til i tutorialen så det fungerer både at gå frem og tilbage.
        Ved at skrive tutorialSteps[tutorialIndex] henter den TutorialStep objekt fra arrayet
        tutorialText.text bruges til at placere teksten ind i et TMP UI-elementer så det kan ses på skærmen
         */
    }

    public void NextTutorial()
    {
        tutorialIndex++; //lægger en til tutorialIndex så næste gang ShowTutorial bliver loadet viser den en ny tekstblok som blev hentet fra json fil

        if (tutorialIndex >= tutorialSteps.Length) //if-statement der gør at hvis tutorialIndex er større eller lig med tutorialSteps, som er variablen der blev lavet ovenfor, så kører det under
        {
            tutorialPanel.SetActive(false); //gemmer tutorialPanel
            StartGame(); //kører StarGame funktion
            return; //går tilbage og ud af det her
        }

        ShowTutorial(); //kalder bare funktion
    }

    public void PreviousTutorial() //samme som over, går bare tilbage
    {
        if (tutorialIndex > 0) //gør at man ikke kan gå tilbage når man er på første tutorial side
        {
            tutorialIndex--;
            ShowTutorial();
        }
        else 
        {
            Debug.Log("Kan ikke gå længere tilbage"); //brugte jeg bare til at tjekke noget, men Debug.Log er nede i bunden af Unity eller under Console. Viser bare beskeder eller fejl. Det her sender beskeden
        }
    }

    public void StartGame()
    {
        currentIndex = 0; //sættes til 0 så spilleren starter på første spørgsmål
        points = 0; //same ^^
        //starter 3 funktioner
        LoadQuestions();
        ShowWordQuestion();
        UpdatePoints();
        //gemmer 2 paneler
        tutorialPanel.SetActive(false);
        wonPanel.SetActive(false);
    }

    void LoadQuestions()
    {
        TextAsset json = Resources.Load<TextAsset>("WordQuestions"); //henter TextAsset, altså tekst fil som er typen json. Henter fra mappen Resources og json fil skal have navnet WordQuestions
        WordQuestionList list = JsonUtility.FromJson<WordQuestionList>(json.text); //Her bliver WordQuestions sat som en liste, altså ligesom i LoadTutorial
        questions = list.questions; //variablen questions bliver sat her og bruges længere nede
    }

    void ShowWordQuestion()
    { //gemmer 2 paneler og viser wordPanel så spillet kan  starte
        wordPanel.SetActive(true);
        situationPanel.SetActive(false);
        wonPanel.SetActive(false);
        //teksten bliver sat ind ift questions hvor currentIndex
        wordText.text = questions[currentIndex].wordText;

        for (int i = 0; i < 3; i++) //det her er et for-loop. Den kører igennem noget indtil der ikke er mere af det, Her laves variabel i inline, som er hvad kører.
            //Til start sættes int i til 0. Derefter kører for-loopet indtil at i passer med 3. Hver gang den kører går i op med 1.
        {
            optionTexts[i].text = questions[currentIndex].options[i]; //tager option tekst og fylder ud. Så hver gang i kører, fyldes en optionText. Fylder element 0, 1 og 2 ud med tekst fra json fil.
        }
    }

    public void AnswerWord(int selectedIndex) //AnswerWord kører med knapper når de trykkes. Hver knap har sin egen selectedIndex som bliver sendt med knappen.
    {
        if (selectedIndex == questions[currentIndex].correctIndex) //Her tjekker den om man svarede rigtig, ved at sammenligne selectedIndex fra svaret på knappen med correctIndex fra json fil WordQuestions. Den bruger currentIndex til at tjekke hvilket spørgsmål man er på, da forskelige spørgsmål har forskellige svar.
        {
            points++; //plus point og så kører 2 kommandoer
            UpdatePoints();
            ShowSituationQuestion();
            rightAnswer.Play(); //spiller rigtig lyd
        }
        else //hvis det er forkert
        {
            if (points > 0) //og man har over 0 point
            {
                points--; //fjern
                UpdatePoints(); //kør det så man kan se det
                ShowWrongAnswer("Det var ikke helt korrekt.\nPrøv datatypen igen."); //forkert besked
                wrongAnswer.Play(); //forkert lydeffekt
            }
            else 
            {
                ShowWrongAnswer("Det var ikke helt korrekt.\nPrøv datatypen igen.");
                wrongAnswer.Play();
            }

        }
    }

    void ShowSituationQuestion()
    {
        wordPanel.SetActive(false);
        situationPanel.SetActive(true);

        situationText.text = questions[currentIndex].situationText; //skriver situationen i tekst ud fra questions listen med currentIndex til at bestemme placering

        for (int i = 0; i < 2; i++) //her kører et for-loop ligesom ovenfor, her kører der dog kun 0 og 1, så 2 i alt
        {
            situationOptionTexts[i].text = questions[currentIndex].situationOptions[i];
        }
    }

    public void AnswerSituation(int selectedIndex) //samme som i AnswerWord
    {
        if (selectedIndex == questions[currentIndex].correctSituationIndex)
        {
            points++;
            UpdatePoints();
            NextQuestion(); //forskel er dog at den kører den her funktion som er under
            rightAnswer.Play();
        }
        else
        {
            if (points > 0)
            {
                points--;
                UpdatePoints();
                ShowWrongAnswer("Det var ikke helt korrekt.\nPrøv situationen igen.");
                wrongAnswer.Play();
            }
            else
            {
                ShowWrongAnswer("Det var ikke helt korrekt.\nPrøv situationen igen.");
                wrongAnswer.Play();
            }
        }
    }

    void NextQuestion() //den her kører når man svarer rigtig på situation
    {
        currentIndex++; //index går op

        if (currentIndex >= questions.Length) //hvis index er lige så stor eller større end questions.Length så kører det her
        {
            wordPanel.SetActive(false);
            situationPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            wonPanel.SetActive(true); //gemmer alle paneler og viser wonPanel, så man har vundet
            if (points >= 18) //et par if-elseif-else-statements som kører efter hvor mange point spilleren har
            {
                wonText.text = "Spillet er færdigt! Du har vundet med " + points + " point ud af 20. Godt gået!";
            }
            else if (points >= 12)
            {
                wonText.text = "Det virker til at du har styr på det med " + points + " ud af 20. Stærkt! Vi ville dog stadig foreslå at du kiggede på datatilsynet.dk og læste mere";
            }
            else if (points >= 6)
            {
                wonText.text = "Vi mener at du kunne gøre brug af datatilsynet.dk da du fik " + points + " ud af 20";
            }
            else
            {
                wonText.text = "Vi ville foreslå at du læste lidt på datatilsynet.dk og derefter prøvede spillet igen, da du fik " + points + " ud af 20";
            }
            return;
        }

        ShowWordQuestion(); //hvis man ikke er færdig så kører den bare videre til næste WordQuestion
    }

    void UpdatePoints() //bruges bare til at opdatere point efter en ændring
    {
        pointText.text = "Point: " + points + "/20"; //skriver point i pointText i hjørnet
    }

    void ShowWrongAnswer(string forkertSvarBesked) //inline string så 2 forskellige beskeder kan vises, efter om man svarer forkert på del 1 eller 2
    {
        wrongText.text = forkertSvarBesked; //sætter inline til besked tmp lavet oppe
        wrongPanel.SetActive(true); //sættes aktiv og vises som pop-up
    }

    public void TryAgain() //funktion på knap inde i wrongPanel. Kan trykkes på så panel forsvinder og man kan svare igen.
    {
        wrongPanel.SetActive(false); 
    }
}
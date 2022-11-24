using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Invisible Buttons")]
    public GameObject progressButton; //Full-screened button that progresses the game forward when only the dialogue is present.
    public GameObject misstepButton; //Full screened button that progresses the game forward when the misstep is made.

    [Header("Progress Variables")] 
    public int progressStep = 0; //Integer variable that indicates on which step of the game is the player currently. 
    public int misstep = 0; //Integer variable that helps with the text displays during missteps.
    public int misstepCount = 0; //Integer variable that counts the missteps the player has made.
    public bool grandmaGuiding; //Bool variable that helps triggering right events for different buttons (turned true when grandma is guiding the player).
    public float periodTime = 10f; //Loop time for updates in outside-text-and-buttons-events.
    public int grandmaPoints; //Points for potential upgrades.
    public TextMeshProUGUI grandmaPointsDisplayed; //For display of grandma points.

    //Bool variables that helps triggering right events for different buttons (turned true when the player can or must use the tool or ingredient).
    public bool timeForMixingBowl; 
    public bool timeForButterKnife;
    public bool timeForScale;
    public bool timeForLadle;
    public bool timeForCup;
    public bool timeForPan;
    public bool timeForTurkishPot;
    public bool timeForWhisk;
    public bool timeForTablespoon;
    public bool timeForButter;
    public bool timeForEggs;
    public bool timeForFlour; 
    public bool timeForMilk;
    public bool timeForOil;
    public bool timeForSalt;
    public bool timeForSugar;
    public bool firstMeasureIsCorrect;
    public bool secondMeasureIsCorrect;
    public bool thirdMeasureIsCorrect;

    [Header("Text During Play")]
    public GameObject textBox; //Text box inside wich the text will be typed during play at the bottom of the screen.
    public TextMeshProUGUI displayedText; //Text that would be displayed inside the text box during play.

    [Header("Play Screens")]
    public GameObject startMenuScreen; //Screen at the start of the game.
    public GameObject fakeSceneView; //Image that shows the view of the location before all the other objects are introdced at the games start.
    public GameObject toolsScreen; //Gives us the view of the available tools when the game needs it.
    public GameObject ingredientsScreen; //Gives us the view of the available ingredients when the game needs it.
    public GameObject measuresScreen; //Gives us the view of the available measures when the game needs it.
    public GameObject specialToolsScreen; //Upgradaeable tools.
    public GameObject choicesScreen; //For upgrade make-sure-question.
    public GameObject objectsIn3D; //On scene objectsthat will be turned off during certian events.

    //Images for the measures screen.
    public GameObject butterImage;
    public GameObject eggsImage;
    public GameObject flourImage;
    public GameObject milkImage;
    public GameObject saltImage;
    public GameObject sugarImage;

    //Images for the upgradeable tools.
    public GameObject panLvl01Upgrade;
    public GameObject panLvl02Upgrade;
    public GameObject panLvl01Tool;
    public GameObject panLvl02Tool;
    public GameObject grandma;

    //Texts for the available measures.
    public TextMeshProUGUI firstMeasureText;
    public TextMeshProUGUI secondMeasureText;
    public TextMeshProUGUI thirdMeasureText;

    [Header("Melting Butter Event")] //Variables for said event.
    public Slider meltRed;
    public Slider meltGreen;
    public float meltTime;
    public Slider interactableSlider;
    public bool meltButterEvent;
    public GameObject meltButterScreen;
    public Animator TurkishMovement;

    [Header("Whisk Event")] //Variables for said event
    public int whiskPoint;
    public GameObject whiskEventScreen;
    public Animator WhiskMovement;

    [Header("Pancake Flip Event")] //Variables for said event
    public GameObject pancakePanScreen;
    public Slider fryGreen;
    public Slider fryRed;
    public Slider pancakeSlider;
    public float fryTime;
    public bool flipNow;
    public bool pancakeFlipEvent;
    public bool pancakeFlipped;
    public Animator pancake;
    public Animator cake;
    public GameObject circle;

    [Header("Audios")]
    public AudioSource startMenuMusic;
    public AudioSource introductionMusic;
    public AudioSource recipeMusic;
    public AudioSource suspenseMusic;
    public AudioSource celebrationMusic;
    public AudioSource theEndMusic;
    public AudioSource buttonSound;
    public AudioSource textSound;


    private void Start()
    {
        Application.targetFrameRate = 30;
        
        startMenuScreen.SetActive(true); //Start screen will be displayed at the game's start.
        
        fakeSceneView.SetActive(false); //Fake scene view is turned off because its start starts the dissolving animation needed at the start of the new game.
        
        misstepButton.SetActive(false); //No need to guide us through the misstep-texts at the game's start.
        toolsScreen.SetActive(false); //There is no need for tools to be shown at the game's start.
        ingredientsScreen.SetActive(false); //There is no need for ingredients to be shown at the game's start.
        measuresScreen.SetActive(false); //There is no need for measures to be shown at the game's start.

        //There is no need for the images from the measures screen to be shown at the game's start.
        butterImage.SetActive(false);
        eggsImage.SetActive(false);
        flourImage.SetActive(false);
        milkImage.SetActive(false);
        saltImage.SetActive(false);
        sugarImage.SetActive(false);

        whiskPoint = 0;

        displayedText.text = ""; ////No text is needed to be displayed in the text box during the game's start and at the very start of play.

        objectsIn3D.SetActive(true);

        startMenuMusic.Play();
    }

    private void FixedUpdate() //For outside-text-and-buttons-events.
    {
        //FOR MELT BUTTER EVENT:
        if (meltButterEvent == true)
        {
            meltTime -= Time.deltaTime;

            meltGreen.value = meltTime;
            meltRed.value = meltTime;

            if (meltTime <= 2f)
            {
                meltGreen.gameObject.SetActive(false);
                meltRed.gameObject.SetActive(true);
            }

            if (meltTime <= 1.5f)
            {
                ProgressForward();
                meltButterScreen.SetActive(false);
                meltButterEvent = false;
                meltGreen.value = meltTime;
                meltRed.value = meltTime;
            }
        }

        //FOR PANCAKE EVENT
        if (pancakeFlipEvent == true)
        {
            fryTime += Time.deltaTime;         
            fryGreen.value = fryTime;

            if (fryTime >= 6f)
            {
                fryGreen.gameObject.SetActive(false);
                fryRed.gameObject.SetActive(true);
                fryRed.value = fryTime;
                flipNow = true;
                displayedText.text = "Now is the time! <b><color=#C0392B>Flip the crepe!</color></b>";
            }


            if (pancakeFlipped == true && flipNow == true)
            {
                pancakeFlipped = true;
                fryGreen.gameObject.SetActive(false);
                fryRed.gameObject.SetActive(false);
                pancake.Play("PancakeFlip");
                cake.Play("Flipping");
                pancakeSlider.gameObject.SetActive(false);
            }

            if (pancakeFlipped == true && flipNow == false)
            {
                pancakeSlider.gameObject.SetActive(false);
                pancakePanScreen.SetActive(false);
                MisstepProgress();
                pancakeFlipEvent = false;
            }

            if (pancakeFlipped == true && fryTime >= 1.66)
            {
                pancakePanScreen.SetActive(false);
                ProgressForward();
                pancakeFlipEvent = false;
            }

            if(pancakeFlipped == false && fryTime >= 8f || pancakeFlipped == true && flipNow == false)
            {
                pancakePanScreen.SetActive(false);
                ProgressForward();
                pancakeFlipEvent = false;
            }
        }
    }

    public void StartNewGame() //Public method that starts the new game.
    {
        //We are at the very start of the game so there is no progress yet.
        progressStep = 0; 
        misstep = 0; 
        misstepCount = 0;
        timeForMixingBowl = false;
        timeForButterKnife = false;
        timeForScale = false;
        timeForTablespoon = false;
        timeForLadle = false;
        timeForCup = false;
        timeForPan = false;
        timeForTurkishPot = false;
        timeForWhisk = false;
        timeForButter = false;
        timeForEggs = false;
        timeForFlour = false;
        timeForMilk = false;
        timeForOil = false;
        timeForSalt = false;
        timeForSugar = false;
        firstMeasureIsCorrect = false;
        secondMeasureIsCorrect = false;
        thirdMeasureIsCorrect = false;

        startMenuScreen.SetActive(false); //Start screen is turned off at the start of the new game.
        fakeSceneView.SetActive(true); //Fake scene view is activated and its dissolving animation starts, revealing the characters and the in-play UI.
        toolsScreen.SetActive(false); //There is no need for tools to be shown at the game's start.
        ingredientsScreen.SetActive(false); //There is no need for ingredients to be shown at the game's start.
        measuresScreen.SetActive(false); //There is no need for measures to be shown at the game's start.

        //There is no need for the images from the measures screen to be shown at the game's start.
        butterImage.SetActive(false);
        eggsImage.SetActive(false);
        flourImage.SetActive(false);
        milkImage.SetActive(false);
        saltImage.SetActive(false);
        sugarImage.SetActive(false);

        misstepButton.SetActive(false); //No need to guide us through the misstep-texts at the game's start.

        grandmaGuiding = true; //It's first half of the level so grandma is guiding the player through the recipe.
        
        StartCoroutine(NewGameStarter()); //Coroutine that will make the first step in progress of the level after the animation of the fake scene view is finished is being called.

        displayedText.text = ""; ////No text is needed to be displayed in the text box during the game's start and at the very start of play.

        startMenuMusic.Stop();
        introductionMusic.Play();
        theEndMusic.Stop();
        buttonSound.Play();
    }

    IEnumerator NewGameStarter() //Coroutine that will make the first step in progress of the level after the animation of the fake scene view is finished.
    {
        yield return new WaitForSeconds(0.9f); //Actions will be called after the almost-one-second-animation of the fake scene is finished.
        ProgressForward(); //The method for progressing the level forward is being called.
        progressButton.gameObject.SetActive(true); //The progress button that sets the ProgressForward() method is turned on.
        fakeSceneView.SetActive(false); //Animation is finished so dissoved fake scene view is no longer needed and turned off.
    }

    public void ProgressForward() //The method that progresses the level forward (attached to the progress button).
    {
        progressStep++; //Step forward in the progress of the level is made.
        CrepeLevel(); //The method that puts the player on the step of the level is being called.
        textSound.Play();
    }

    public void MisstepProgress() //The method that progresses the game forward when a misstep has been made (attached to the misstep button).
    {
        misstep++;
        MisstepTextDisplay();
        Debug.Log("Desio se");
        textSound.Play();
    }

    private void CrepeLevel() //Skeleton of the whole level with every step of the progress.
    {
        misstepButton.SetActive(false);
        misstep = 0;

        if (progressStep == 1)
        {
            displayedText.text = "Oh, hello there, my favorite muffin!";
        }

        if (progressStep == 2)
        {
            displayedText.text = "If only you knew how happy I was when I heard that you were coming to help your grandma cook!";
        }

        if (progressStep == 3)
        {
            displayedText.text = "Oh, I know you had to get up early.";
        }

        if (progressStep == 4)
        {
            displayedText.text = "But how else are the neighbor's kids going to have their breakfast before they go to school?";
        }

        if (progressStep == 5)
        {
            displayedText.text = "What could I've done after I promised the neighbors that I would feed their children?";
        }

        if (progressStep == 6)
        {
            displayedText.text = "You don't want your grandma to turn out to be a rude old hag who doesn't keep her word, do you?";
        }

        if (progressStep == 7)
        {
            displayedText.text = "Okay, maybe I wasn't thinking of you when I told them there wouldn't be any problem...";
        }

        if (progressStep == 8)
        {
            displayedText.text = "But I really thought I could do everything myself!";
        }

        if (progressStep == 9)
        {
            displayedText.text = "After all, this is a convenient opportunity for you to learn a few more things from me.";
        }

        if (progressStep == 10)
        {
            displayedText.text = "And we have never cooked anything together.";
        }

        if (progressStep == 11)
        {
            displayedText.text = "It's always me who is cooking for her dearest muffin!";
        }

        if (progressStep == 12)
        {
            displayedText.text = "The neighbor's kids love <b><color=#C0392B>crepes</color></b>, those thin types of pancakes, so I thought I'd make them mine for breakfast today.";
        }

        if (progressStep == 13)
        {
            displayedText.text = "We will make them exactly like they taught me in France - oui, oui!";
        }

        if (progressStep == 14)
        {
            displayedText.text = "I think <b><color=#C0392B>the batter for around 10 crepes</color></b> will do just fine, so we’ll pick ingredients in amounts that suit that number.";
        }

        if (progressStep == 15)
        {
            displayedText.text = "Being so tall as you are, will you be the one to get the ingredients off the shelf for me, honey?";
        }

        if (progressStep == 16)
        {
            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "To begin with, we will need <b><color=#C0392B>a mixing bowl</color></b> for the batter.";
            }

            timeForMixingBowl = true; //Now is the time to click the mixing bowl button on the tools screen.
            toolsScreen.SetActive(true); //The view of the tools is turned on because the game needs it.
            introductionMusic.Stop();
            recipeMusic.Play();
        }

        if (progressStep == 17)
        {
            toolsScreen.SetActive(false); //The view of the tools is turned off because the game doesn't need it at the moment.
            timeForMixingBowl = false; //The time to click the mixing bowl stopped.
            displayedText.text = "Thank you, honey!";
            misstepButton.SetActive(false);
        }

        if (progressStep == 18)
        {
            displayedText.text = "Oh, I'm so happy that we're doing this together!";
        }

        if (progressStep == 19)
        {
            timeForFlour = true; //Now is the time to click the flour button on the ingredients screen.
            ingredientsScreen.SetActive(true); //The view of the ingredients is turned on because the game needs it.

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "We'll need <b><color=#C0392B>flour</color></b> for the batter, so grab that for me as well.";
            }
        }

        if (progressStep == 20)
        {
            timeForFlour = false;
            timeForScale = true;
            ingredientsScreen.SetActive(false);
            toolsScreen.SetActive(true);

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Now, put <b><color=#C0392B>300 grams</color></b> of flour in the bowl. Use <b><color=#C0392B>the kitchen scale</color></b> to help yourself.";
            }

        }

        if (progressStep == 21)
        {
            toolsScreen.SetActive(false); //The tools screen is no longer needed.
            timeForScale = false;

            //The measures screen is activated and the second of them is the correct one.
            measuresScreen.SetActive(true);
            flourImage.SetActive(true); //The measure of the flour is in question.
            secondMeasureIsCorrect = true;
            firstMeasureText.text = "150 GRAMS";
            secondMeasureText.text = "300 GRAMS";
            thirdMeasureText.text = "500 GRAMS";

            if (misstepCount > 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "We need <b><color=#C0392B>300 grams</color></b> of flour.";
            }
        }

        if (progressStep == 22)
        {
            measuresScreen.SetActive(false); //The measures screen is no longer needed.

            //The measure of the flour is no longer in question.
            flourImage.SetActive(false); 
            secondMeasureIsCorrect = false;

            //The sugar ingredient is needed.
            ingredientsScreen.SetActive(true);
            timeForSugar = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Good job! Can you reach <b><color=#C0392B>the sugar</color></b> on the shelf for me, please?";
            }
        }

        if (progressStep == 23)
        {
            //The sugar ingredient is no longer needed.
            ingredientsScreen.SetActive(false);
            timeForSugar = false;

            //The tablespoon is needed.
            toolsScreen.SetActive(true);
            timeForTablespoon = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Sweet! <b><color=#C0392B>One tablespoon</color></b> will be enough for us.";
            }
        }

        if (progressStep == 24)
        {
            //The tools screen is no longer needed.
            toolsScreen.SetActive(false); 
            timeForTablespoon = false;

            //The measures screen is activated and the first of them is the correct one.
            measuresScreen.SetActive(true);
            sugarImage.SetActive(true); //The measure of the sugar is in question.
            firstMeasureIsCorrect = true;
            firstMeasureText.text = "ONE TABLESPOON";
            secondMeasureText.text = "FIVE TABLESPOONS";
            thirdMeasureText.text = "SIX TABLESPOONS";

            if (misstepCount > 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "<b><color=#C0392B>One tablespoon</color></b> will be enough.";
            }
        }

        if (progressStep == 25)
        {
            //The measuring is no longer needed.
            measuresScreen.SetActive(false);
            sugarImage.SetActive(false); //The measure of the sugar is no longer in question.
            firstMeasureIsCorrect = false;

            //The salt is needed.
            ingredientsScreen.SetActive(true);
            timeForSalt = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Great! Now pass the <b><color=#C0392B>salt</color></b>.";
            }
        }

        if (progressStep == 26)
        {
            //The ingredients screen is no longer needed.
            ingredientsScreen.SetActive(false);
            timeForSalt = false;

            //The measures screen is activated and the frst of them is the correct one.
            measuresScreen.SetActive(true);
            saltImage.SetActive(true); //The measure of the salt is in question.
            firstMeasureIsCorrect = true;
            firstMeasureText.text = "A PINCH";
            secondMeasureText.text = "ONE TABLESPOON";
            thirdMeasureText.text = "100 GRAMS";

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "<b><color=#C0392B>A pinch</color></b> of it will be enough.";
            }
        }

        if (progressStep == 27)
        {
            //The measuring is no longer needed.
            measuresScreen.SetActive(false);
            saltImage.SetActive(false); //The measure of the salt is no longer in question.
            firstMeasureIsCorrect = false;

            //The egg ingredient is needed.
            ingredientsScreen.SetActive(true);
            timeForEggs = true;


            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Great, my dear! Get us <b><color=#C0392B>three eggs</color></b> now, please.";
            }
        }

        if (progressStep == 28)
        {
            //The ingredients screen is no longer needed.
            ingredientsScreen.SetActive(false);
            timeForEggs = false;

            //The measures screen is activated and the secomd of them is the correct one.
            measuresScreen.SetActive(true);
            eggsImage.SetActive(true); //The measure of the eggs is in question.
            secondMeasureIsCorrect = true;
            firstMeasureText.text = "ONE EGG";
            secondMeasureText.text = "THREE EGGS";
            thirdMeasureText.text = "SIX EGGS";

            if (misstepCount > 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Well done, mon petit! Now crack <b><color=#C0392B>three eggs</color></b> into the mixing bowl.";
            }
        }

        if (progressStep == 29)
        {
            //The measuring is no longer needed.
            measuresScreen.SetActive(false);
            eggsImage.SetActive(false); //The measure of the salt is no longer in question.
            secondMeasureIsCorrect = false;

            displayedText.text = "You are cracking eggs better than your grandma!";
        }

        if (progressStep == 30)
        {
            displayedText.text = "Not a single shell fell into the mixing bowl!";
        }

        if (progressStep == 31)
        {
            //The milk ingredient is needed.
            ingredientsScreen.SetActive(true);
            timeForMilk = true;


            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "We will need <b><color=#C0392B>milk</color></b> now.";
            }
        }

        if (progressStep == 32)
        {
            //The milk ingredient is no longer needed.
            ingredientsScreen.SetActive(false);
            timeForMilk = false;

            //The cup is needed.
            toolsScreen.SetActive(true);
            timeForCup = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Pour <b><color=#C0392B>half a liter of milk</color></b> into the mixing bowl. Use <b><color=#C0392B>the liquid measuring cup</color></b> to help yourself.";
            }
        }

        if (progressStep == 33)
        {
            //The tools screen is no longer needed.
            toolsScreen.SetActive(false);
            timeForMilk = false;

            //The measures screen is activated and the first of them is the correct one.
            measuresScreen.SetActive(true);
            milkImage.SetActive(true); //The measure of the sugar is in question.
            firstMeasureIsCorrect = true;
            firstMeasureText.text = "HALF A LITER";
            secondMeasureText.text = "A LITER";
            thirdMeasureText.text = "A LITER AND A HALF";

            if (misstepCount > 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Pour <b><color=#C0392B>half a liter of milk</color></b> into the bowl instead.";
            }
        }

        if (progressStep == 34)
        {
            //The measuring is no longer needed.
            measuresScreen.SetActive(false);
            milkImage.SetActive(false); //The measure of the salt is no longer in question.
            firstMeasureIsCorrect = false;

            displayedText.text = "We are near the end!";
        }

        if (progressStep == 35)
        {
            displayedText.text = "Now, since the butter cannot go unmelted in the bowl, we'll have to melt it first.";
        }

        if (progressStep == 36)
        {
            //The Turkish pot is needed.
            toolsScreen.SetActive(true);
            timeForTurkishPot = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Get us <b><color=#C0392B>the Turkish pot</color></b> in which we'll do that.";
            }
        }

        if (progressStep == 37)
        {
            //The Turkish pot is no longer needed.
            toolsScreen.SetActive(false);
            timeForTurkishPot = false;

            //The butter is needed.
            ingredientsScreen.SetActive(true);
            timeForButter = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "Good job, honey! Now pass us <b><color=#C0392B>the butter</color></b>.";
            }
        }

        if (progressStep == 38)
        {
            //The butter is no longer needed.
            ingredientsScreen.SetActive(false);
            timeForButter = false;

            //We need the butter knife.
            toolsScreen.SetActive(true);
            timeForButterKnife = true;

            if (misstepCount == 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "We will need <b><color=#C0392B>50 grams</color></b> of butter. Help yourself with <b><color=#C0392B>the butter knife</color></b> to do that.";
            }
        }

        if (progressStep == 39)
        {
            //The butter knife is no longer needed.
            toolsScreen.SetActive(false);
            timeForButterKnife = false;

            //We need to measure the butter
            measuresScreen.SetActive(true);
            butterImage.SetActive(true);
            firstMeasureIsCorrect = true;
            firstMeasureText.text = "50 GRAMS";
            secondMeasureText.text = "100 GRAMS";
            thirdMeasureText.text = "150 GRAMS";

            if (misstepCount > 0) //Different text will be displayed if the misstep has been made.
            {
                displayedText.text = "We only need <b><color=#C0392B>50 grams</color></b> of butter.";
            }
        }

        if (progressStep == 40)
        {
            //No need to measure the butter
            measuresScreen.SetActive(false);
            butterImage.SetActive(false);
            firstMeasureIsCorrect = false;
            displayedText.text = "Excellent! Now melt the butter using the small pot.";
            objectsIn3D.SetActive(false);

            meltButterScreen.SetActive(true);
        }

        if (progressStep == 41)
        {
            objectsIn3D.SetActive(true);
            displayedText.text = "Well done, my little chef!";
        }

        if (progressStep == 42)
        {
            displayedText.text = "For the end, I like to add some cinnamon to add some of the grandma's touch to my crepes.";
        }

        if (progressStep == 43)
        {
            displayedText.text = "But we won't do that. I have other plans for this batter.";
        }

        if (progressStep == 44)
        {
            displayedText.text = "Ugh, this part is very exhausting for your old grandma.";
        }

        if (progressStep == 45)
        {            
            //Whisk time and whisk need.
            toolsScreen.SetActive(true);
            timeForWhisk = true;

            if (misstepCount == 0)
            {
                displayedText.text = "Would you please <b><color=#C0392B>whisk</color></b> all the ingredients in the bowl for us?";
            }
        }

        if (progressStep == 46)
        {
            //No whisk needed.
            toolsScreen.SetActive(false);
            timeForWhisk = false;

            //Event is starting
            whiskEventScreen.SetActive(true);
            objectsIn3D.SetActive(false);
        }

        if (progressStep == 47)
        {
            displayedText.text = "Isn’t it great that you came to your grandma’s!";
            objectsIn3D.SetActive(true);
        }

        if (progressStep == 48)
        {
            //Pan needed.
            toolsScreen.SetActive(true);
            timeForPan = true;

            if (misstepCount == 0)
            {
                displayedText.text = "Now grab <b><color=#C0392B>the pan</color></b>.";
            }
        }

        if (progressStep == 49)
        {
            //No longer pan needed.
            toolsScreen.SetActive(false);
            timeForPan = false;

            //Oil needed.
            ingredientsScreen.SetActive(true);
            timeForOil = true;

            if (misstepCount == 0)
            {
                displayedText.text = "We will cook the crepes in a pan, but we will have to <b><color=#C0392B>oil</color></b> it first.";
            }
        }

        if (progressStep == 50)
        {
            //Oil no longer needed.
            ingredientsScreen.SetActive(false);
            timeForOil = false;

            displayedText.text = "Super! Now we have everything we need to cook crepes!";
        }

        if (progressStep == 51)
        {
            displayedText.text = "Super! Now we have everything we need to cook crepes!";
            ProgressForward(); //Bug fix.
        }

        if (progressStep == 52)
        {
            displayedText.text = "When your grandmother was young, she could flip pancakes in the air with the pan while cooking them.";
        }

        if (progressStep == 53)
        {
            displayedText.text = "I'd love to see if you could pull it off!";
        }

        if (progressStep == 54)
        {
            //Time for laddle
            toolsScreen.SetActive(true);
            timeForLadle = true;

            if (misstepCount == 0)
            {
                displayedText.text = "Get yourself <b><color=#C0392B>a ladle</color></b> so you can pour the batter onto the pan.";
            }
        }

        if (progressStep == 55)
        {
            //No longer time for laddle
            toolsScreen.SetActive(false);
            timeForLadle = false;

            displayedText.text = "Now let's see how my little muffin turns crepes in the air!";
        }

        if (progressStep == 56)
        {
            StartPancakeFlipEvent();
            recipeMusic.Stop();
            suspenseMusic.Play();
            objectsIn3D.SetActive(false);

            if (misstepCount == 0)
            {
                displayedText.text = "Cook the crepe until it's golden in places on bottom before flipping it over, but do not overcook it!";
            }
        }

        if (progressStep == 57)
        {
            displayedText.text = "Good job, mon chéri!";
            suspenseMusic.Stop();
            celebrationMusic.Play();
            objectsIn3D.SetActive(true);
        }

        if (progressStep == 58)
        {
            displayedText.text = "Mmm... This crepe looks delicious!";
        }

        if (progressStep == 59)
        {
            displayedText.text = "There...";
        }

        if (progressStep == 60)
        {
            displayedText.text = "What are you looking at me like that for, my sugar?";
        }

        if (progressStep == 61)
        {
            displayedText.text = "Yes, I know I threw the whole batter into the trash!";
        }

        if (progressStep == 62)
        {
            displayedText.text = "Oh, it's just a batter!";
        }

        if (progressStep == 63)
        {
            displayedText.text = "And I did say that you'll learn something from your grandma today.";
        }

        if (progressStep == 64)
        {
            displayedText.text = "You will make a new batch of crepes with as little of my guidance as possible.";
        }

        if (progressStep == 65)
        {
            displayedText.text = "Of course, grandma will help you if you get lost in some of the steps, don't worry.";
        }

        if (progressStep == 66)
        {
            displayedText.text = "I'm not as crazy as you think!";
        }

        if (progressStep == 67)
        {
            displayedText.text = "Furthermore, I would say that you earned some <b><color=#C0392B>extra points</color></b> for your grandma!";
            grandma.SetActive(true);
            StartCoroutine(AddPoints());
            
        }

        if (progressStep == 68)
        {
            specialToolsScreen.SetActive(true);
            displayedText.text = "And for that, I think you can now access some of your grandma's special tools.";
        }

        if (progressStep == 69)
        {
            displayedText.text = "Now let's see you make your own crepes!";
        }

        if (progressStep == 70)
        {
            grandmaGuiding = false; //Grandma is no longer guiding us, the missteps are counting.
            startMenuScreen.SetActive(true);
            celebrationMusic.Stop();
            theEndMusic.Play();
            objectsIn3D.SetActive(false);
        }

        misstepCount = 0;


    }


    public void MisstepTextDisplay() //Displays the text after the misstep has been made.
    {
        misstepButton.SetActive(true);

        //PROGRESS STEP = 16
        if (progressStep == 16 && misstep == 1)
        {
            misstepCount++;
            toolsScreen.SetActive(false);
            displayedText.text = "Are you still asleep, dear?";
        }

        if (progressStep == 16 && misstep == 2)
        {
            displayedText.text = "We need <b><color=#C0392B>the mixing bowl</color></b> for the batter. It's right over there.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 19
        if (progressStep == 19 && misstep == 1)
        {
            misstepCount++;
            ingredientsScreen.SetActive(false);
            displayedText.text = "I can tell that you are still sleepy...";
        }

        if (progressStep == 19 && misstep == 2)
        {
            displayedText.text = "I said <b><color=#C0392B>flour</color></b>, boy!";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 20
        if (progressStep == 20 && misstep == 1)
        {
            misstepCount++;
            ingredientsScreen.SetActive(false);
            displayedText.text = "You won't be able to measure weight with this! ";
        }

        if (progressStep == 20 && misstep == 2)
        {
            displayedText.text = "<b><color=#C0392B>The kitchen scale</color></b> is right over there.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 21
        if (progressStep == 21 && misstep == 1)
        {
            misstepCount++;
            measuresScreen.SetActive(false);
            displayedText.text = "Stop it, dear!";
        }

        if (progressStep == 21 && misstep == 2)
        {
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 22
        if (progressStep == 22 && misstep == 1)
        {
            misstepCount++;
            ingredientsScreen.SetActive(false);
            displayedText.text = "Mon Dieu! We won't be able to sweeten our crepes with that!";
        }

        if (progressStep == 22 && misstep == 2)
        {
            displayedText.text = "We need <b><color=#C0392B>sugar</color></b>!";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 23
        if (progressStep == 23 && misstep == 1)
        {
            displayedText.text = "Honey, what is good to use if we want to take <b><color=#C0392B>a tablespoon</color></b> of something?";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 24
        if (progressStep == 24 && misstep == 1)
        {
            measuresScreen.SetActive(false);
            displayedText.text = "We don't want our crepes to be too sweet. ";
            misstepCount++;
        }

        if (progressStep == 24 && misstep == 2)
        {
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 25
        if (progressStep == 25 && misstep == 1)
        {
            displayedText.text = "The <b><color=#C0392B>salt</color></b> is right over there, honey.";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 26
        if (progressStep == 26 && misstep == 1)
        {
            measuresScreen.SetActive(false);
            displayedText.text = "We don't want our crepes to be too salty.";
            misstepCount++;
        }

        if (progressStep == 26 && misstep == 2)
        {
            displayedText.text = "Just <b><color=#C0392B>a pinch of it</color></b> will be fine.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 27
        if (progressStep == 27 && misstep == 1)
        {
            displayedText.text = "We are looking for <b><color=#C0392B>the eggs</color></b>, honey.";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 28
        if (progressStep == 28 && misstep == 1)
        {
            measuresScreen.SetActive(false);
            displayedText.text = "You like to tease your grandma, don't you?";
            misstepCount++;
        }

        if (progressStep == 28 && misstep == 2)
        {
            displayedText.text = "You need to crack <b><color=#C0392B>three eggs</color></b> into the mixing bowl.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 31
        if (progressStep == 31 && misstep == 1)
        {
            displayedText.text = "How can you not see where <b><color=#C0392B>the milk</color></b> is?";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 32
        if (progressStep == 32 && misstep == 1)
        {
            displayedText.text = "My dear, what is good to use if we want to <b><color=#C0392B>measure the amount of some liquid</color></b>?";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
        }

        //PROGRESS STEP = 33
        if (progressStep == 33 && misstep == 1)
        {
            measuresScreen.SetActive(false);
            misstepCount++;
            displayedText.text = "This amount wont work.";
        }

        if (progressStep == 33 && misstep == 2)
        {
            displayedText.text = "Pour <b><color=#C0392B>half a liter of milk</color></b> into the mixing bowl instead.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP = 36
        if (progressStep == 36 && misstep == 1)
        {
            displayedText.text = "No, we're going to melt the butter in <b><color=#C0392B>a Turkish pot</color></b>, dear.";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
        }

        //PROGRESS STEP = 37
        if (progressStep == 37 && misstep == 1)
        {
            ingredientsScreen.SetActive(false);
            misstepCount++;
            displayedText.text = "No, my petit muffin!";
        }

        if (progressStep == 37 && misstep == 2)
        {
            displayedText.text = "We will need <b><color=#C0392B>the butter</color></b> to finish the batter.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 38
        if (progressStep == 38 && misstep == 1)
        {
            misstepButton.SetActive(false);
            misstepCount++;
            displayedText.text = "My little muffin, use <b><color=#C0392B>the butter knife</color></b> to cut the butter!";
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 39
        if (progressStep == 39 && misstep == 1)
        {
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 45
        if (progressStep == 45 && misstep == 1)
        {
            displayedText.text = "It is better if you use <b><color=#C0392B>the whisk</color></b> for whisking the ingredients.";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 48
        if (progressStep == 48 && misstep == 1)
        {
            toolsScreen.SetActive(false);
            displayedText.text = "We can't cook pancakes on that, my dear!";
            misstepCount++;
        }

        if (progressStep == 48 && misstep == 2)
        {
            displayedText.text = "We'll need <b><color=#C0392B>a pan</color></b>.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 49
        if (progressStep == 49 && misstep == 1)
        {
            displayedText.text = "You won't be able to <b><color=#C0392B>oil</color></b> the pan with that, honey!";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 54
        if (progressStep == 54 && misstep == 1)
        {
            displayedText.text = "It's easier if you use <b><color=#C0392B>the ladle</color></b> to pour the batter onto the pan.";
            misstepButton.SetActive(false);
            misstepCount++;
            CrepeLevel();
            misstep = 0;
        }

        //PROGRESS STEP 56
        if (progressStep == 56 && misstep == 1)
        {
            objectsIn3D.SetActive(true);
            displayedText.text = "Ugh, this crepe seems to be ruined.";
            misstepCount++;
        }

        if (progressStep == 56 && misstep == 2)
        {
            displayedText.text = "We can't give that to the kids.";
        }

        if (progressStep == 56 && misstep == 3)
        {
            displayedText.text = "But don't worry about it, honey.";
        }

        if (progressStep == 56 && misstep == 4)
        {
            displayedText.text = "That still happens to me even today.";
        }

        if (progressStep == 56 && misstep == 5)
        {
            displayedText.text = "Try again.";
            misstepButton.SetActive(false);
            CrepeLevel();
            misstep = 0;
        }
    }


    //METHODS ATTACHED TO THE BUTTONS ON THE TOOL SCREEN:
    public void MixingBowl() //The method that is attached to the mixing bowl button.
    {
        buttonSound.Play();

        if (timeForMixingBowl == true)
        {
            ProgressForward();
        }

        else if (timeForMixingBowl == false)
        {
            MisstepProgress();
        }
    }


    public void ButterKnife() //The method that is attached to the butter knife button.
    {
        buttonSound.Play();

        if (timeForButterKnife == true)
        {
            ProgressForward();
        }

        else if (timeForButterKnife == false)
        {
            MisstepProgress();
        }
    }


    public void Tablespoon() //The method that is attached to the hammer knife button.
    {
        buttonSound.Play();

        if (timeForTablespoon == true)
        {
            ProgressForward();
        }

        else if (timeForTablespoon == false)
        {
            MisstepProgress();
        }
    }


    public void Scale() //The method that is attached to the scale button.
    {
        buttonSound.Play();

        if (timeForScale == true)
        {
            ProgressForward();
        }

        else if (timeForScale == false)
        {
            MisstepProgress();
        }
    }


    public void Ladle() //The method that is attached to the ladle button.
    {
        buttonSound.Play();

        if (timeForLadle == true)
        {
            ProgressForward();
        }

        else if (timeForLadle == false)
        {
            MisstepProgress();
        }
    }

    public void Cup() //The method that is attached to the cup button.
    {
        buttonSound.Play();

        if (timeForCup == true)
        {
            ProgressForward();
        }

        else if (timeForCup == false)
        {
            MisstepProgress();
        }
    }


    public void Pan() //The method that is attached to the pan button.
    {
        buttonSound.Play();

        if (timeForPan == true)
        {
            ProgressForward();
        }

        else if (timeForPan == false)
        {
            MisstepProgress();
        }
    }


    public void TurkishPot() //The method that is attached to the turkish pot button.
    {
        buttonSound.Play();

        if (timeForTurkishPot == true)
        {
            ProgressForward();
        }

        else if (timeForTurkishPot == false)
        {
            MisstepProgress();
        }
    }


    public void Whisk() //The method that is attached to the whisk button.
    {
        buttonSound.Play();

        if (timeForWhisk == true)
        {
            ProgressForward();
        }

        else if (timeForWhisk == false)
        {
            MisstepProgress();
        }
    }


    //METHODS ATTACHED TO THE BUTTONS ON THE INGREDIENTS SCREEN:
    public void Butter()
    {
        buttonSound.Play();

        if (timeForButter == true)
        {
            ProgressForward();
        }

        else if (timeForButter == false)
        {
            MisstepProgress();
        }
    }

    public void Eggs() //The method that is attached to the eggs button.
    {
        buttonSound.Play();

        if (timeForEggs == true)
        {
            ProgressForward();
        }

        else if (timeForEggs == false)
        {
            MisstepProgress();
        }
    }

    public void Flour() //The method that is attached to the flour button.
    {
        buttonSound.Play();

        if (timeForFlour == true)
        {
            ProgressForward();
        }

        else if (timeForFlour == false)
        {
            MisstepProgress();
        }
    }

    public void Meat() //The method that is attached to the meat button.
    {
        buttonSound.Play();

        MisstepProgress();

    }

    public void Milk() //The method that is attached to the milk button.
    {
        buttonSound.Play();

        if (timeForMilk == true)
        {
            ProgressForward();
        }

        else if (timeForMilk == false)
        {
            MisstepProgress();
        }
    }

    public void Oil() //The method that is attached to the oil button.
    {
        buttonSound.Play();

        if (timeForOil == true)
        {
            ProgressForward();
        }

        else if (timeForOil == false)
        {
            MisstepProgress();
        }
    }

    public void Salt() //The method that is attached to the salt button.
    {
        buttonSound.Play();

        if (timeForSalt == true)
        {
            ProgressForward();
        }

        else if (timeForSalt == false)
        {
            MisstepProgress();
        }
    }

    public void Sugar() //The method that is attached to the sugar button.
    {
        buttonSound.Play();

        if (timeForSugar == true)
        {
            ProgressForward();
        }

        else if (timeForSugar == false)
        {
            MisstepProgress();
        }
    }

    public void Tuna() //The method that is attached to the tuna button.
    {
        buttonSound.Play();

        MisstepProgress();

    }

    //METHODS ATTACHED TO MEASURE BUTTONS:
    public void FirstMeasure()
    {

        buttonSound.Play();

        if (firstMeasureIsCorrect == true)
        {
            ProgressForward();
        }

        else if (firstMeasureIsCorrect  == false)
        {
            MisstepProgress();
        }
    }

    public void SecondMeasure()
    {
        buttonSound.Play();


        if (secondMeasureIsCorrect == true)
        {
            ProgressForward();
        }

        else if (secondMeasureIsCorrect == false)
        {
            MisstepProgress();
        }
    }

    public void ThirdMeasure()
    {
        buttonSound.Play();

        if (thirdMeasureIsCorrect == true)
        {
            ProgressForward();
        }

        else if (thirdMeasureIsCorrect == false)
        {
            MisstepProgress();
        }
    }

    //OUTSIDE TEXT-AND-BUTTONS-EVENTS:
    public void StartMeltingButterEvent()
    {
        meltButterEvent = true;

        TurkishMovement.Play("TurkishAnimation");

        meltTime = 8f;

        meltGreen.gameObject.SetActive(true);
        
        meltGreen.value = meltTime;
        meltRed.value = meltTime;
        meltRed.maxValue = 8f;
        meltGreen.maxValue = 8f;
    }

    public void StartWhiskEvent()
    {
        whiskPoint++;

        if (whiskPoint == 1)
        {
            WhiskMovement.Play("WhiskMovement02");
        }

        if (whiskPoint == 2)
        {
            WhiskMovement.Play("WhiskMovement01");
        }

        if (whiskPoint == 3)
        {
            WhiskMovement.Play("WhiskMovement02");
        }

        if (whiskPoint == 4)
        {
            WhiskMovement.Play("WhiskMovement01");
        }

        if (whiskPoint == 5)
        {
            WhiskMovement.Play("WhiskMovement02");
        }

        if (whiskPoint == 6)
        {
            WhiskMovement.Play("WhiskMovement01");
            ProgressForward();
            whiskEventScreen.SetActive(false);
        }


    }

    public void StartPancakeFlipEvent()
    {
        pancakePanScreen.SetActive(true);

        fryTime = 0f;
        fryGreen.gameObject.SetActive(true);
        fryRed.gameObject.SetActive(false);
        fryGreen.value = fryTime;
        fryGreen.maxValue = 8f;
        fryRed.value = fryTime;
        fryRed.maxValue = 8f;

        flipNow = false;

        pancakeSlider.gameObject.SetActive(true);
        
        pancakeFlipEvent = true;
        pancakeFlipped = false;
    }

    public void PancakeFlip()
    {
        pancakeFlipped = true;
        fryTime = 2f;
        circle.SetActive(false);
    }

    //FOR UPGRADE
    public void SpecialToolUpgrade()
    {
        choicesScreen.SetActive(true);
        displayedText.text = "Are you sure? It will cost you <b><color=#C0392B>60 GRANDMA POINTS</color></b>.";
    }

    public void YesChoice()
    {
        choicesScreen.SetActive(false);
        panLvl01Upgrade.SetActive(false);
        panLvl02Upgrade.SetActive(true);
        panLvl01Tool.SetActive(false);
        panLvl02Tool.SetActive(true);
        StartCoroutine(TakePoints());
        StartCoroutine(CloseSpecialTools());
        ProgressForward();
        buttonSound.Play();
    }

    public void NoChoice()
    {
        choicesScreen.SetActive(false);
        specialToolsScreen.SetActive(false);
        ProgressForward();
        buttonSound.Play();
    }

    IEnumerator AddPoints()
    {
        yield return new WaitForSeconds(1f);
        grandmaPoints += 70;
        grandmaPointsDisplayed.text = "" + grandmaPoints;
    }

    IEnumerator TakePoints()
    {
        yield return new WaitForSeconds(1f);
        grandmaPoints -= 60;
        grandmaPointsDisplayed.text = "" + grandmaPoints;
    }

    IEnumerator CloseSpecialTools()
    {
        yield return new WaitForSeconds(3f);
        specialToolsScreen.SetActive(false);
    }
}

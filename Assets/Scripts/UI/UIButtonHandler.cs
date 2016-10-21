using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UIButtonHandler : MonoBehaviour {
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void OpenAchievementPane()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    public void OnLoadSelected()
    {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = false;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnLoadGameSelected);
    }

    public void OnLoadGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            OpenSavedGameForLoad(game.Filename);
        }
        else
        {
            // handle cancel or error
            Debug.Log("Load game UI error");
        }
    }

    void OpenSavedGameForLoad(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, LoadGameData);
    }

    void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            PlayerData playerData = new PlayerData();
            using(MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                stream.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                playerData = (PlayerData)bf.Deserialize(stream);
            }
            gameManager.ResetPoints();
            gameManager.GainPoints(playerData.score);

            Vector3 shipPosition = new Vector3(playerData.shipPosition_x, playerData.shipPosition_y, playerData.shipPosition_z);
            Vector3 eulerRotation = new Vector3(playerData.shipEuler_x, playerData.shipEuler_y, playerData.shipEuler_z);
            Debug.Log(eulerRotation);
            Quaternion shipRotation = Quaternion.identity;
            shipRotation.eulerAngles = eulerRotation;

            GameObject ship = GameObject.Find("Ship");
            ship.transform.position = shipPosition;
            ship.transform.rotation = Quaternion.Euler(eulerRotation);
        }
        else
        {
            // handle error
        }
    }

    public void OnSaveSelected()
    {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = true;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);
    }

    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            string fileName = game.Filename;
            if(String.IsNullOrEmpty(fileName))
            {
                fileName = GenerateSaveGameName();
            }
            OpenSavedGameForSave(fileName);
        }
        else
        {
            // handle cancel or error
            Debug.Log("Save game UI error");
        }
    }

    void OpenSavedGameForSave(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            byte[] savedData;
            PlayerData playerData = new PlayerData();
            playerData.score = gameManager.score;

            GameObject ship = GameObject.Find("Ship");
            Vector3 shipPosition = ship.transform.position;
            playerData.shipPosition_x = shipPosition.x;
            playerData.shipPosition_y = shipPosition.y;
            playerData.shipPosition_z = shipPosition.z;

            Vector3 eulerRotation = ship.transform.rotation.eulerAngles;
            playerData.shipEuler_x = eulerRotation.x;
            playerData.shipEuler_y = eulerRotation.y;
            playerData.shipEuler_z = eulerRotation.z;

            Debug.Log(eulerRotation);

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                bf.Serialize(stream, playerData);
                savedData = stream.ToArray();
            }
            
            SaveGame(game, savedData, TimeSpan.FromSeconds(Time.realtimeSinceStartup));
        }
        else
        {
            // handle error
        }
    }

    private string GenerateSaveGameName()
    {
        DateTime now = DateTime.Now;
        string fileName = String.Format("SaveGame_{0:yyyyMMddhhmmss}", now);
        return fileName;
    }

    void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        //Texture2D savedImage = getScreenshot();
        Texture2D savedImage = null;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);
        if (savedImage != null)
        {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below
            byte[] pngData = savedImage.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);
        }
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
        }
        else
        {
            // handle error
        }
    }

    public Texture2D getScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be
        // extracted
        Texture2D screenShot = new Texture2D(200, 200);

        // Takes the screenshot from top left hand corner of screen and maps to top
        // left hand corner of screenShot texture
        screenShot.ReadPixels(
            new Rect(0, 0, 200, 200), 0, 0);
        screenShot.Apply();
        return screenShot;
    }

    [Serializable]
    class PlayerData
    {
        public int score;
        public float shipPosition_x;
        public float shipPosition_y;
        public float shipPosition_z;
        public float shipEuler_x;
        public float shipEuler_y;
        public float shipEuler_z;
    }
}

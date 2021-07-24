using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

public class Playfab_Controller : MonoBehaviour
{
    public static Playfab_Controller PFC;


    private string userEmail;
    private string userPassword;
    private string userName;
    [SerializeField]
    private GameObject LoginMenu;
    [SerializeField]
    private GameObject RegisterMenu;

    private void OnEnable()
    {
        if(Playfab_Controller.PFC == null)
        {
            Playfab_Controller.PFC = this;
        }
        else
        {
            if(Playfab_Controller.PFC!=this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "5B583";
        }
        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        //var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    #region Login
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you are successfully logged in!");
        GetStats();
    }

    private void OnRegisterSucesss(RegisterPlayFabUserResult result )
    {
        Debug.Log("Congratulations, you are registered!");
    }

    

    private void OnLoginFailure(PlayFabError error)
    {
        print("Login failed");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        print("registration failed");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void getUserEmail(string emailin)
    {
        userEmail = emailin;
    }

    public void getUserPassword(string pwin)
    {
        userPassword = pwin;
    }

    public void getUserName(string usernameIN)
    {
        userName = usernameIN;
    }


    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void OnClickCreateAccount()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSucesss, OnRegisterFailure);
    }
    public void OnClickRegister()
    {
        LoginMenu.SetActive(false);
        RegisterMenu.SetActive(true);
    }

    public void OnClickBack()
    {
        LoginMenu.SetActive(true);
        RegisterMenu.SetActive(false);
    }

    #endregion Login

    #region player_stats

    public int Total_Wins;
    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "Total_Wins", Value = Total_Wins },
        }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            
            switch(eachStat.StatisticName)
            {
                case "Total_Wins":
                    Total_Wins = eachStat.Value;
                    break;
            }
        }

    }

    #endregion player_stats

}
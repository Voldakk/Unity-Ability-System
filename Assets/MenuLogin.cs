using UnityEngine;
using UnityEngine.UI;

public class MenuLogin : MonoBehaviour
{
    public InputField username;
    public InputField password;

    public void OnLoginClicked()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            Debug.Log("Please enter an username");
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            Debug.Log("Please enter an password");
        }
        else
        {
            new GameSparks.Api.Requests.AuthenticationRequest().SetUserName(username.text).SetPassword(password.text).Send((response) => {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Authenticated...");
                    MenuManager.ChangeMenu("MainMenu");
                }
                else
                {
                    Debug.Log("Error Authenticating Player...");
                }
            });
        }
    }
}
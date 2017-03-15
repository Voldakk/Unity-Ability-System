using UnityEngine;
using UnityEngine.UI;

public class MenuRegister : MonoBehaviour
{
    public InputField username;
    public InputField password;

    public void OnRegisterClicked()
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
            new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(username.text)
            .SetPassword(password.text)
            .SetUserName(username.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Registered…");
                }
                else
                {
                    Debug.Log("Error Registering Player");
                }
            }
          );
        }
    }
}
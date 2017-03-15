using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuMatchmaking : MonoBehaviour
{
    public Text playerList;
    void Start()
    {
        GameSparks.Api.Messages.MatchNotFoundMessage.Listener = (message) => {
            playerList.text = "No Match Found...";
        };
        GameSparks.Api.Messages.MatchFoundMessage.Listener += OnMatchFound;
    }
    public void FindPlayers()
    {
        MenuManager.ChangeMenu("Matchmaking");
        playerList.text = "Looking for players...";

        Debug.Log("GSM| Attempting Matchmaking...");
        new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode("TEST1") // set the shortCode to be the same as the one we created in the first tutorial
            .SetSkill(0) // in this case we assume all players have skill level zero and we want anyone to be able to join so the skill level for the request is set to zero
            .Send((response) => {
                if (response.HasErrors)
                { // check for errors
                    Debug.LogError("GSM| MatchMaking Error \n" + response.Errors.JSON);
                }
            });
    }
    private void OnMatchFound(GameSparks.Api.Messages.MatchFoundMessage _message)
    {
        Debug.Log("Match Found!...");
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.AppendLine("Match Found...");
        sBuilder.AppendLine("Host URL:" + _message.Host);
        sBuilder.AppendLine("Port:" + _message.Port);
        sBuilder.AppendLine("Access Token:" + _message.AccessToken);
        sBuilder.AppendLine("MatchId:" + _message.MatchId);
        sBuilder.AppendLine("Opponents:" + _message.Participants.Count());
        sBuilder.AppendLine("_________________");
        sBuilder.AppendLine(); // we'll leave a space between the player-list and the match data
        foreach (GameSparks.Api.Messages.MatchFoundMessage._Participant player in _message.Participants)
        {
            sBuilder.AppendLine("Player:" + player.PeerId + " User Name:" + player.DisplayName); // add the player number and the display name to the list
        }
        playerList.text = sBuilder.ToString(); // set the string to be the player-list field
    }
}
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TMPro; // Include this
using UnityEngine;
using UnityEngine.UI;

public class ClientNumberGuesser : MonoBehaviour
{
    public TMP_InputField nameField;  
    public TMP_InputField numberField;
    public Button guessButton;    
    public TMP_Text responseText; 

    private HttpClient httpClient = new HttpClient();

    private void Start()
    {
        // Attach the Click event to the button
        guessButton.onClick.AddListener(OnGuessButtonClicked);
    }

    public async void OnGuessButtonClicked()
    {
        // Ensure the inputs are valid
        if (string.IsNullOrEmpty(nameField.text) || !int.TryParse(numberField.text, out int guess))
        {
            responseText.text = "Invalid input. Please enter a name and a valid number."; // Update the text
            return;
        }

        // Build the URI safely
        var baseUri = "http://192.168.1.71:7010/randomnumber"; // Adjust to your server's IP
        var completeUri = baseUri; // Complete URI is just the base URI

        try
        {
            // Create the request content
            var content = new StringContent(guess.ToString(), Encoding.UTF8, "application/json"); // Send the guess in the body

            // Send the request as a PUT
            var request = new HttpRequestMessage(HttpMethod.Put, completeUri)
            {
                Content = content
            };
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                responseText.text = $"{responseBody}"; 
            }
            else
            {
                responseText.text = $"Error: {response.StatusCode}"; 
            }
        }
        catch (Exception ex)
        {
            responseText.text = $"Exception occurred: {ex.Message}"; 
        }
    }

    private void OnDestroy()
    {
        httpClient.Dispose(); 
    }
}

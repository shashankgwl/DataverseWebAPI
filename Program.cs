// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;

Console.WriteLine("Hello, World!");

string token = "<bearer token>";
string dataverseUrl = "https://org798d1247.crm.dynamics.com/api/data/v9.2/$batch";
await ExecuteTransactionAsync();

async Task ExecuteTransactionAsync()
{
    using var httpClient = new HttpClient();
    var batchBoundary = $"batch_{Guid.NewGuid()}";
    var changesetBoundary = $"changeset_{Guid.NewGuid()}";

    var batchContent = new MultipartContent("mixed", batchBoundary);
    var changesetContent = new MultipartContent("mixed", changesetBoundary);

AddRequestContent(changesetContent, "POST", "contacts", GetContact("Jony", "Doe", "Software Engineer", "john.doe@example.com", "1234567890"), "1");
AddRequestContent(changesetContent, "POST", "accounts", GetOrganization("Sample Corp1", "$1", "info@example.com", "0987654321", "http://www.example.com"), "2");

    batchContent.Add(changesetContent);

    var totalBatchContent = await batchContent.ReadAsStringAsync();

    var requestMessage = new HttpRequestMessage(HttpMethod.Post, dataverseUrl)
    {
        Content = batchContent
    };

    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var response = await httpClient.SendAsync(requestMessage);
    
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}

void AddRequestContent(MultipartContent changesetContent, string method, string entity, string jsonContent, string contentId)
{
    var requestContent = new StringContent($"{method} /api/data/v9.2/{entity} HTTP/1.1{Environment.NewLine}Content-Type: application/json; type=entry{Environment.NewLine}{Environment.NewLine}{jsonContent}", new MediaTypeHeaderValue("application/http"));
    requestContent.Headers.Add("Content-Transfer-Encoding", "binary");
    requestContent.Headers.Add("Content-ID", contentId);
    changesetContent.Add(requestContent);
}

string GetContact(string firstname, string lastname, string jobTitle, string emailAddr1, string mobilePhone)
{
    var contact = new { firstname, lastname, jobtitle = jobTitle, emailaddress1 = emailAddr1, mobilephone = mobilePhone };
    return JsonSerializer.Serialize(contact, new JsonSerializerOptions { WriteIndented = true });
}

string GetOrganization(string organizationName, string primaryContactId, string emailAddress, string telephone, string websiteUrl)
{
    var organization = new Dictionary<string, object>
    {
        { "name", organizationName },
        { "primarycontactid@odata.bind", primaryContactId },
        { "emailaddress1", emailAddress },
        { "telephone1", telephone },
        { "websiteurl", websiteUrl }
    };
    return JsonSerializer.Serialize(organization, new JsonSerializerOptions { WriteIndented = true });
}


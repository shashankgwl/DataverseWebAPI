# C# Console Application for Microsoft Dataverse Batch Request

This C# console application demonstrates how to create and send a batch request to Microsoft Dataverse using the Web API. The batch request includes operations to create a contact and an account.

## Code Explanation

### Imports

*Import necessary namespaces for HTTP operations and JSON serialization.*

### Greeting

**Print a "Hello" message.**

### Token and URL

**Define the authentication token and Dataverse URL for the batch request.**

### ExecuteTransactionAsync Method

- Initialize an `HttpClient`.
- Define unique batch and changeset boundaries.
- Create `MultipartContent` for the batch and changeset.
- Add request content to the changeset for creating a contact and an account.
- Add the changeset to the batch content.
- Create an `HttpRequestMessage` with the batch content and set the authorization header.
- Send the request and print the response.

### AddRequestContent Method

*Helper method to add request content to the changeset.*

### GetContact Method

*Helper method to create a JSON representation of a contact.*

### GetOrganization Method

*Helper method to create a JSON representation of an organization.*

> **Note:** Replace `"YOUR_ACCESS_TOKEN"` with your actual access token before running the code.

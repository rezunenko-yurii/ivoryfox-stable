/* 
 * Content Delivery Client API
 *
 * <p>Cloud Content Delivery is a managed cloud service that hosts and delivers content to end users worldwide.</p> <p>You are currently viewing the documentation for the <b>Client API</b>, which is intended to be used at runtime by your game client. The <a href=\"https://content-api.cloud.unity3d.com/doc/\">Content Delivery Management API</a> is intended to be used by developers at build time for managing content.</p> <h2>Client API Domain</h2> <p>The Client API base domain uses a different domain from the Management API and is in the format:</p> <pre>https://9a474388-ba4d-469a-bf3c-8b557f89012d.client-api.unity3dusercontent.com/</pre> <h2>Client SDK</h2> <p>The Content Delivery Client API is based on Swagger. You can use the <a href=\"https://swagger.io/tools/swagger-codegen/\">Swagger Code Generator</a> and a client schema document to integrate client libraries with your projects.</p> <p>Once you create a bucket with the Management API, the Client API is accessible via a per-project API domain. To download the schema document necessary to generate a client library, put in your Project ID and click Download.</p> <form action=\"https://content-api.cloud.unity3d.com/doc_client/doc.json\" method=\"GET\"> <div><label for=\"projectID\">Project ID</label><input type=\"text\" name=\"projectID\" id=\"projectID\" required /></div> <div><button type=\"submit\">Download</button></div> </form> <h2>Authentication</h2> <p>Everything in the Client API requires NO AUTHENTICATION. The data served from this API is effectively public.</p>
 *
 * OpenAPI spec version: 0.9.112
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */


using System.Collections.Generic;

namespace CloudContentDeliveryClient.Client
{
    /// <summary>
    /// Represents a readable-only configuration contract.
    /// </summary>
    public interface IReadableConfiguration
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>Access token.</value>
        string AccessToken { get; }

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>API key.</value>
        IDictionary<string, string> ApiKey { get; }

        /// <summary>
        /// Gets the API key prefix.
        /// </summary>
        /// <value>API key prefix.</value>
        IDictionary<string, string> ApiKeyPrefix { get; }

        /// <summary>
        /// Gets the base path.
        /// </summary>
        /// <value>Base path.</value>
        string BasePath { get; }

        /// <summary>
        /// Gets the date time format.
        /// </summary>
        /// <value>Date time foramt.</value>
        string DateTimeFormat { get; }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <value>Default header.</value>
        IDictionary<string, string> DefaultHeader { get; }

        /// <summary>
        /// Gets the temp folder path.
        /// </summary>
        /// <value>Temp folder path.</value>
        string TempFolderPath { get; }

        /// <summary>
        /// Gets the HTTP connection timeout (in milliseconds)
        /// </summary>
        /// <value>HTTP connection timeout.</value>
        int Timeout { get; }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>User agent.</value>
        string UserAgent { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>Username.</value>
        string Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>Password.</value>
        string Password { get; }

        /// <summary>
        /// Gets the API key with prefix.
        /// </summary>
        /// <param name="apiKeyIdentifier">API key identifier (authentication scheme).</param>
        /// <returns>API key with prefix.</returns>
        string GetApiKeyWithPrefix(string apiKeyIdentifier);
    }
}

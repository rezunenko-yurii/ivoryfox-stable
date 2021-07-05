/* 
 * Content Delivery Management API
 *
 * <p>Cloud Content Delivery is a managed cloud service that hosts and delivers content to end users worldwide.</p> <p>You are currently viewing the documentation for the <b>Management API</b>, intended to be used by developers at build time for managing content. Refer to the <a href=\"https://content-api.cloud.unity3d.com/doc_client/\">Content Delivery Client API</a> for documentation about the API intended to be used at runtime by your game client.</p> <h2>Client SDK</h2> <p>The Content Delivery Management API is based on Swagger. The <a href=\"https://swagger.io/tools/swagger-codegen/\">Swagger Code Generator</a> can generate client libraries to integrate with your projects.</p> <p>A <a href=\"https://content-api.cloud.unity3d.com/doc/doc.json\">JSON schema</a> is required to generate a client for this API version.</p> <h2>Authentication</h2> <p>The Content Delivery Management API requires an API key associated with your Unity developer account. To access your API Key, please visit the <a href=\"https://developer.cloud.unity3d.com\">developer dashboard</a>.</p> <p>To authenticate requests, include a Basic Authentication header as a base64-encoded string 'username:password', using your API key as the password (and empty username).</p> <p>For example, an API key value of 'd6d2c026bac44b1ea7ac0332694a830e' would include an Authorization header like:</p> <p><b>Authorization: Basic OmQ2ZDJjMDI2YmFjNDRiMWVhN2FjMDMzMjY5NGE4MzBl</b></p> <h2>Pagination</h2> <p>Paged results take two parameters: the number of results to return per page (?per_page) and the page number based on that per page amount (?page). Page numbering starts with 1 and the default page size is 10.</p> <p>For instance, if there are 40 results and you specify page=2&per_page=10, you will receive records 11-20. Paged results will also return a Content-Range header. In the example above the content range header will look like this:</p> <p><b>Content-Range: items 11-20/40</b></p>
 *
 * OpenAPI spec version: 0.9.112
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */


using System.Collections.Generic;

namespace CloudContentDeliveryManagment.Client
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
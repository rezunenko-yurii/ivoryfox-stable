/* 
 * Content Delivery Management API
 *
 * <p>Cloud Content Delivery is a managed cloud service that hosts and delivers content to end users worldwide.</p> <p>You are currently viewing the documentation for the <b>Management API</b>, intended to be used by developers at build time for managing content. Refer to the <a href=\"https://content-api.cloud.unity3d.com/doc_client/\">Content Delivery Client API</a> for documentation about the API intended to be used at runtime by your game client.</p> <h2>Client SDK</h2> <p>The Content Delivery Management API is based on Swagger. The <a href=\"https://swagger.io/tools/swagger-codegen/\">Swagger Code Generator</a> can generate client libraries to integrate with your projects.</p> <p>A <a href=\"https://content-api.cloud.unity3d.com/doc/doc.json\">JSON schema</a> is required to generate a client for this API version.</p> <h2>Authentication</h2> <p>The Content Delivery Management API requires an API key associated with your Unity developer account. To access your API Key, please visit the <a href=\"https://developer.cloud.unity3d.com\">developer dashboard</a>.</p> <p>To authenticate requests, include a Basic Authentication header as a base64-encoded string 'username:password', using your API key as the password (and empty username).</p> <p>For example, an API key value of 'd6d2c026bac44b1ea7ac0332694a830e' would include an Authorization header like:</p> <p><b>Authorization: Basic OmQ2ZDJjMDI2YmFjNDRiMWVhN2FjMDMzMjY5NGE4MzBl</b></p> <h2>Pagination</h2> <p>Paged results take two parameters: the number of results to return per page (?per_page) and the page number based on that per page amount (?page). Page numbering starts with 1 and the default page size is 10.</p> <p>For instance, if there are 40 results and you specify page=2&per_page=10, you will receive records 11-20. Paged results will also return a Content-Range header. In the example above the content range header will look like this:</p> <p><b>Content-Range: items 11-20/40</b></p>
 *
 * OpenAPI spec version: 0.9.112
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CloudContentDeliveryManagment.Client
{
    /// <summary>
    /// Represents a set of configuration settings
    /// </summary>
    public class Configuration : IReadableConfiguration
    {
        #region Constants

        /// <summary>
        /// Version of the package.
        /// </summary>
        /// <value>Version of the package.</value>
        public const string Version = "1.0.0";

        /// <summary>
        /// Identifier for ISO 8601 DateTime Format
        /// </summary>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8 for more information.</remarks>
        // ReSharper disable once InconsistentNaming
        public const string ISO8601_DATETIME_FORMAT = "o";

        #endregion Constants

        #region Static Members

        private static readonly object GlobalConfigSync = new { };
        private static Configuration _globalConfiguration;

        /// <summary>
        /// Default creation of exceptions for a given method name and response object
        /// </summary>
        public static readonly ExceptionFactory DefaultExceptionFactory = (methodName, response) =>
        {
            var status = (int)response.StatusCode;
            if (status >= 400)
            {
                return new ApiException(status,
                    string.Format("Error calling {0}: {1}", methodName, response.Content),
                    response.Content);
            }
            if (status == 0)
            {
                return new ApiException(status,
                    string.Format("Error calling {0}: {1}", methodName, response.ErrorMessage), response.ErrorMessage);
            }
            return null;
        };

        /// <summary>
        /// Gets or sets the default Configuration.
        /// </summary>
        /// <value>Configuration.</value>
        public static Configuration Default
        {
            get { return _globalConfiguration; }
            set
            {
                lock (GlobalConfigSync)
                {
                    _globalConfiguration = value;
                }
            }
        }

        #endregion Static Members

        #region Private Members

        /// <summary>
        /// Gets or sets the API key based on the authentication name.
        /// </summary>
        /// <value>The API key.</value>
        private IDictionary<string, string> _apiKey = null;

        /// <summary>
        /// Gets or sets the prefix (e.g. Token) of the API key based on the authentication name.
        /// </summary>
        /// <value>The prefix of the API key.</value>
        private IDictionary<string, string> _apiKeyPrefix = null;

        private string _dateTimeFormat = ISO8601_DATETIME_FORMAT;
        private string _tempFolderPath = Path.GetTempPath();

        #endregion Private Members

        #region Constructors

        static Configuration()
        {
            _globalConfiguration = new GlobalConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class
        /// </summary>
        public Configuration()
        {
            UserAgent = "Swagger-Codegen/1.0.0/csharp";
            BasePath = "https://content-api.cloud.unity3d.com/api/v1";
            DefaultHeader = new ConcurrentDictionary<string, string>();
            ApiKey = new ConcurrentDictionary<string, string>();
            ApiKeyPrefix = new ConcurrentDictionary<string, string>();

            // Setting Timeout has side effects (forces ApiClient creation).
            Timeout = 100000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class
        /// </summary>
        public Configuration(
            IDictionary<string, string> defaultHeader,
            IDictionary<string, string> apiKey,
            IDictionary<string, string> apiKeyPrefix,
            string basePath = "https://content-api.cloud.unity3d.com/api/v1") : this()
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException("The provided basePath is invalid.", "basePath");
            if (defaultHeader == null)
                throw new ArgumentNullException("defaultHeader");
            if (apiKey == null)
                throw new ArgumentNullException("apiKey");
            if (apiKeyPrefix == null)
                throw new ArgumentNullException("apiKeyPrefix");

            BasePath = basePath;

            foreach (var keyValuePair in defaultHeader)
            {
                DefaultHeader.Add(keyValuePair);
            }

            foreach (var keyValuePair in apiKey)
            {
                ApiKey.Add(keyValuePair);
            }

            foreach (var keyValuePair in apiKeyPrefix)
            {
                ApiKeyPrefix.Add(keyValuePair);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class with different settings
        /// </summary>
        /// <param name="apiClient">Api client</param>
        /// <param name="defaultHeader">Dictionary of default HTTP header</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="accessToken">accessToken</param>
        /// <param name="apiKey">Dictionary of API key</param>
        /// <param name="apiKeyPrefix">Dictionary of API key prefix</param>
        /// <param name="tempFolderPath">Temp folder path</param>
        /// <param name="dateTimeFormat">DateTime format string</param>
        /// <param name="timeout">HTTP connection timeout (in milliseconds)</param>
        /// <param name="userAgent">HTTP user agent</param>
        [Obsolete("Use explicit object construction and setting of properties.", true)]
        public Configuration(
            // ReSharper disable UnusedParameter.Local
            ApiClient apiClient = null,
            IDictionary<string, string> defaultHeader = null,
            string username = null,
            string password = null,
            string accessToken = null,
            IDictionary<string, string> apiKey = null,
            IDictionary<string, string> apiKeyPrefix = null,
            string tempFolderPath = null,
            string dateTimeFormat = null,
            int timeout = 100000,
            string userAgent = "Swagger-Codegen/1.0.0/csharp"
            // ReSharper restore UnusedParameter.Local
            )
        {

        }

        /// <summary>
        /// Initializes a new instance of the Configuration class.
        /// </summary>
        /// <param name="apiClient">Api client.</param>
        [Obsolete("This constructor caused unexpected sharing of static data. It is no longer supported.", true)]
        // ReSharper disable once UnusedParameter.Local
        public Configuration(ApiClient apiClient)
        {

        }

        #endregion Constructors


        #region Properties

        private ApiClient _apiClient = null;
        /// <summary>
        /// Gets an instance of an ApiClient for this configuration
        /// </summary>
        public virtual ApiClient ApiClient
        {
            get
            {
                if (_apiClient == null) _apiClient = CreateApiClient();
                return _apiClient;
            }
        }

        private String _basePath = null;
        /// <summary>
        /// Gets or sets the base path for API access.
        /// </summary>
        public virtual string BasePath {
            get { return _basePath; }
            set {
                _basePath = value;
                // pass-through to ApiClient if it's set.
                if(_apiClient != null) {
                    _apiClient.RestClient.BaseUrl = new Uri(_basePath);
                }
            }
        }

        /// <summary>
        /// Gets or sets the default header.
        /// </summary>
        public virtual IDictionary<string, string> DefaultHeader { get; set; }

        /// <summary>
        /// Gets or sets the HTTP timeout (milliseconds) of ApiClient. Default to 100000 milliseconds.
        /// </summary>
        public virtual int Timeout
        {
            
            get { return ApiClient.RestClient.Timeout; }
            set { ApiClient.RestClient.Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the HTTP user agent.
        /// </summary>
        /// <value>Http user agent.</value>
        public virtual string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the username (HTTP basic authentication).
        /// </summary>
        /// <value>The username.</value>
        public virtual string Username { get; set; }

        /// <summary>
        /// Gets or sets the password (HTTP basic authentication).
        /// </summary>
        /// <value>The password.</value>
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets the API key with prefix.
        /// </summary>
        /// <param name="apiKeyIdentifier">API key identifier (authentication scheme).</param>
        /// <returns>API key with prefix.</returns>
        public string GetApiKeyWithPrefix(string apiKeyIdentifier)
        {
            var apiKeyValue = "";
            ApiKey.TryGetValue (apiKeyIdentifier, out apiKeyValue);
            var apiKeyPrefix = "";
            if (ApiKeyPrefix.TryGetValue (apiKeyIdentifier, out apiKeyPrefix))
                return apiKeyPrefix + " " + apiKeyValue;
            else
                return apiKeyValue;
        }

        /// <summary>
        /// Gets or sets the access token for OAuth2 authentication.
        /// </summary>
        /// <value>The access token.</value>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the temporary folder path to store the files downloaded from the server.
        /// </summary>
        /// <value>Folder path.</value>
        public virtual string TempFolderPath
        {
            get { return _tempFolderPath; }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // Possible breaking change since swagger-codegen 2.2.1, enforce a valid temporary path on set.
                    _tempFolderPath = Path.GetTempPath();
                    return;
                }

                // create the directory if it does not exist
                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }

                // check if the path contains directory separator at the end
                if (value[value.Length - 1] == Path.DirectorySeparatorChar)
                {
                    _tempFolderPath = value;
                }
                else
                {
                    _tempFolderPath = value + Path.DirectorySeparatorChar;
                }
            }
        }

        /// <summary>
        /// Gets or sets the date time format used when serializing in the ApiClient
        /// By default, it's set to ISO 8601 - "o", for others see:
        /// https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
        /// and https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
        /// No validation is done to ensure that the string you're providing is valid
        /// </summary>
        /// <value>The DateTimeFormat string</value>
        public virtual string DateTimeFormat
        {
            get { return _dateTimeFormat; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // Never allow a blank or null string, go back to the default
                    _dateTimeFormat = ISO8601_DATETIME_FORMAT;
                    return;
                }

                // Caution, no validation when you choose date time format other than ISO 8601
                // Take a look at the above links
                _dateTimeFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the prefix (e.g. Token) of the API key based on the authentication name.
        /// </summary>
        /// <value>The prefix of the API key.</value>
        public virtual IDictionary<string, string> ApiKeyPrefix
        {
            get { return _apiKeyPrefix; }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("ApiKeyPrefix collection may not be null.");
                }
                _apiKeyPrefix = value;
            }
        }

        /// <summary>
        /// Gets or sets the API key based on the authentication name.
        /// </summary>
        /// <value>The API key.</value>
        public virtual IDictionary<string, string> ApiKey
        {
            get { return _apiKey; }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("ApiKey collection may not be null.");
                }
                _apiKey = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        public void AddDefaultHeader(string key, string value)
        {
            DefaultHeader[key] = value;
        }

        /// <summary>
        /// Creates a new <see cref="ApiClient" /> based on this <see cref="Configuration" /> instance.
        /// </summary>
        /// <returns></returns>
        public ApiClient CreateApiClient()
        {
            return new ApiClient(BasePath) { Configuration = this };
        }


        /// <summary>
        /// Returns a string with essential information for debugging.
        /// </summary>
        public static String ToDebugReport()
        {
            String report = "C# SDK (CloudContentDeliveryManagment) Debug Report:\n";
            report += "    OS: " + System.Environment.OSVersion + "\n";
            report += "    .NET Framework Version: " + System.Environment.Version  + "\n";
            report += "    Version of the API: 0.9.112\n";
            report += "    SDK Package Version: 1.0.0\n";

            return report;
        }

        /// <summary>
        /// Add Api Key Header.
        /// </summary>
        /// <param name="key">Api Key name.</param>
        /// <param name="value">Api Key value.</param>
        /// <returns></returns>
        public void AddApiKey(string key, string value)
        {
            ApiKey[key] = value;
        }

        /// <summary>
        /// Sets the API key prefix.
        /// </summary>
        /// <param name="key">Api Key name.</param>
        /// <param name="value">Api Key value.</param>
        public void AddApiKeyPrefix(string key, string value)
        {
            ApiKeyPrefix[key] = value;
        }

        #endregion Methods
    }
}

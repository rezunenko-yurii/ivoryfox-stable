/* 
 * Content Delivery Client API
 *
 * <p>Cloud Content Delivery is a managed cloud service that hosts and delivers content to end users worldwide.</p> <p>You are currently viewing the documentation for the <b>Client API</b>, which is intended to be used at runtime by your game client. The <a href=\"https://content-api.cloud.unity3d.com/doc/\">Content Delivery Management API</a> is intended to be used by developers at build time for managing content.</p> <h2>Client API Domain</h2> <p>The Client API base domain uses a different domain from the Management API and is in the format:</p> <pre>https://9a474388-ba4d-469a-bf3c-8b557f89012d.client-api.unity3dusercontent.com/</pre> <h2>Client SDK</h2> <p>The Content Delivery Client API is based on Swagger. You can use the <a href=\"https://swagger.io/tools/swagger-codegen/\">Swagger Code Generator</a> and a client schema document to integrate client libraries with your projects.</p> <p>Once you create a bucket with the Management API, the Client API is accessible via a per-project API domain. To download the schema document necessary to generate a client library, put in your Project ID and click Download.</p> <form action=\"https://content-api.cloud.unity3d.com/doc_client/doc.json\" method=\"GET\"> <div><label for=\"projectID\">Project ID</label><input type=\"text\" name=\"projectID\" id=\"projectID\" required /></div> <div><button type=\"submit\">Download</button></div> </form> <h2>Authentication</h2> <p>Everything in the Client API requires NO AUTHENTICATION. The data served from this API is effectively public.</p>
 *
 * OpenAPI spec version: 0.9.112
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using CloudContentDeliveryClient.Client;
using CloudContentDeliveryClient.Model;

namespace CloudContentDeliveryClient.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IReleasesApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Get badged release
        /// </summary>
        /// <remarks>
        /// Gets a single badged release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>Release</returns>
        Release GetReleaseByBadgePublic (string bucketid, string badgename);

        /// <summary>
        /// Get badged release
        /// </summary>
        /// <remarks>
        /// Gets a single badged release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>ApiResponse of Release</returns>
        ApiResponse<Release> GetReleaseByBadgePublicWithHttpInfo (string bucketid, string badgename);
        /// <summary>
        /// Get release
        /// </summary>
        /// <remarks>
        /// Gets a single release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>Release</returns>
        Release GetReleasePublic (string bucketid, string releaseid);

        /// <summary>
        /// Get release
        /// </summary>
        /// <remarks>
        /// Gets a single release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>ApiResponse of Release</returns>
        ApiResponse<Release> GetReleasePublicWithHttpInfo (string bucketid, string releaseid);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Get badged release
        /// </summary>
        /// <remarks>
        /// Gets a single badged release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>Task of Release</returns>
        System.Threading.Tasks.Task<Release> GetReleaseByBadgePublicAsync (string bucketid, string badgename);

        /// <summary>
        /// Get badged release
        /// </summary>
        /// <remarks>
        /// Gets a single badged release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>Task of ApiResponse (Release)</returns>
        System.Threading.Tasks.Task<ApiResponse<Release>> GetReleaseByBadgePublicAsyncWithHttpInfo (string bucketid, string badgename);
        /// <summary>
        /// Get release
        /// </summary>
        /// <remarks>
        /// Gets a single release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>Task of Release</returns>
        System.Threading.Tasks.Task<Release> GetReleasePublicAsync (string bucketid, string releaseid);

        /// <summary>
        /// Get release
        /// </summary>
        /// <remarks>
        /// Gets a single release for a given bucket.
        /// </remarks>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>Task of ApiResponse (Release)</returns>
        System.Threading.Tasks.Task<ApiResponse<Release>> GetReleasePublicAsyncWithHttpInfo (string bucketid, string releaseid);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class ReleasesApi : IReleasesApi
    {
        private CloudContentDeliveryClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleasesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ReleasesApi(String basePath)
        {
            this.Configuration = new CloudContentDeliveryClient.Client.Configuration { BasePath = basePath };

            ExceptionFactory = CloudContentDeliveryClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleasesApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public ReleasesApi(CloudContentDeliveryClient.Client.Configuration configuration = null)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = CloudContentDeliveryClient.Client.Configuration.Default;
            else
                this.Configuration = configuration;

            ExceptionFactory = CloudContentDeliveryClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(String basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public CloudContentDeliveryClient.Client.Configuration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public CloudContentDeliveryClient.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public IDictionary<String, String> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(this.Configuration.DefaultHeader);
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            this.Configuration.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Get badged release Gets a single badged release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>Release</returns>
        public Release GetReleaseByBadgePublic (string bucketid, string badgename)
        {
             ApiResponse<Release> localVarResponse = GetReleaseByBadgePublicWithHttpInfo(bucketid, badgename);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get badged release Gets a single badged release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>ApiResponse of Release</returns>
        public ApiResponse< Release > GetReleaseByBadgePublicWithHttpInfo (string bucketid, string badgename)
        {
            // verify the required parameter 'bucketid' is set
            if (bucketid == null)
                throw new ApiException(400, "Missing required parameter 'bucketid' when calling ReleasesApi->GetReleaseByBadgePublic");
            // verify the required parameter 'badgename' is set
            if (badgename == null)
                throw new ApiException(400, "Missing required parameter 'badgename' when calling ReleasesApi->GetReleaseByBadgePublic");

            var localVarPath = "/buckets/{bucketid}/release_by_badge/{badgename}/";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (bucketid != null) localVarPathParams.Add("bucketid", this.Configuration.ApiClient.ParameterToString(bucketid)); // path parameter
            if (badgename != null) localVarPathParams.Add("badgename", this.Configuration.ApiClient.ParameterToString(badgename)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetReleaseByBadgePublic", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Release>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Release) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Release)));
        }

        /// <summary>
        /// Get badged release Gets a single badged release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>Task of Release</returns>
        public async System.Threading.Tasks.Task<Release> GetReleaseByBadgePublicAsync (string bucketid, string badgename)
        {
             ApiResponse<Release> localVarResponse = await GetReleaseByBadgePublicAsyncWithHttpInfo(bucketid, badgename);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get badged release Gets a single badged release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="badgename">Badge Name</param>
        /// <returns>Task of ApiResponse (Release)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Release>> GetReleaseByBadgePublicAsyncWithHttpInfo (string bucketid, string badgename)
        {
            // verify the required parameter 'bucketid' is set
            if (bucketid == null)
                throw new ApiException(400, "Missing required parameter 'bucketid' when calling ReleasesApi->GetReleaseByBadgePublic");
            // verify the required parameter 'badgename' is set
            if (badgename == null)
                throw new ApiException(400, "Missing required parameter 'badgename' when calling ReleasesApi->GetReleaseByBadgePublic");

            var localVarPath = "/buckets/{bucketid}/release_by_badge/{badgename}/";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (bucketid != null) localVarPathParams.Add("bucketid", this.Configuration.ApiClient.ParameterToString(bucketid)); // path parameter
            if (badgename != null) localVarPathParams.Add("badgename", this.Configuration.ApiClient.ParameterToString(badgename)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetReleaseByBadgePublic", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Release>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Release) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Release)));
        }

        /// <summary>
        /// Get release Gets a single release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>Release</returns>
        public Release GetReleasePublic (string bucketid, string releaseid)
        {
             ApiResponse<Release> localVarResponse = GetReleasePublicWithHttpInfo(bucketid, releaseid);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get release Gets a single release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>ApiResponse of Release</returns>
        public ApiResponse< Release > GetReleasePublicWithHttpInfo (string bucketid, string releaseid)
        {
            // verify the required parameter 'bucketid' is set
            if (bucketid == null)
                throw new ApiException(400, "Missing required parameter 'bucketid' when calling ReleasesApi->GetReleasePublic");
            // verify the required parameter 'releaseid' is set
            if (releaseid == null)
                throw new ApiException(400, "Missing required parameter 'releaseid' when calling ReleasesApi->GetReleasePublic");

            var localVarPath = "/buckets/{bucketid}/releases/{releaseid}/";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (bucketid != null) localVarPathParams.Add("bucketid", this.Configuration.ApiClient.ParameterToString(bucketid)); // path parameter
            if (releaseid != null) localVarPathParams.Add("releaseid", this.Configuration.ApiClient.ParameterToString(releaseid)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetReleasePublic", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Release>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Release) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Release)));
        }

        /// <summary>
        /// Get release Gets a single release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>Task of Release</returns>
        public async System.Threading.Tasks.Task<Release> GetReleasePublicAsync (string bucketid, string releaseid)
        {
             ApiResponse<Release> localVarResponse = await GetReleasePublicAsyncWithHttpInfo(bucketid, releaseid);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get release Gets a single release for a given bucket.
        /// </summary>
        /// <exception cref="CloudContentDeliveryClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketid">Bucket ID</param>
        /// <param name="releaseid">Release ID</param>
        /// <returns>Task of ApiResponse (Release)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Release>> GetReleasePublicAsyncWithHttpInfo (string bucketid, string releaseid)
        {
            // verify the required parameter 'bucketid' is set
            if (bucketid == null)
                throw new ApiException(400, "Missing required parameter 'bucketid' when calling ReleasesApi->GetReleasePublic");
            // verify the required parameter 'releaseid' is set
            if (releaseid == null)
                throw new ApiException(400, "Missing required parameter 'releaseid' when calling ReleasesApi->GetReleasePublic");

            var localVarPath = "/buckets/{bucketid}/releases/{releaseid}/";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (bucketid != null) localVarPathParams.Add("bucketid", this.Configuration.ApiClient.ParameterToString(bucketid)); // path parameter
            if (releaseid != null) localVarPathParams.Add("releaseid", this.Configuration.ApiClient.ParameterToString(releaseid)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetReleasePublic", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Release>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Release) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Release)));
        }

    }
}

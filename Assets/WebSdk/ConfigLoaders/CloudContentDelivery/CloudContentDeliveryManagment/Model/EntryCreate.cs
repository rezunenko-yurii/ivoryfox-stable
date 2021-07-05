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
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = CloudContentDeliveryManagment.Client.SwaggerDateConverter;

namespace CloudContentDeliveryManagment.Model
{
    /// <summary>
    /// EntryCreate
    /// </summary>
    [DataContract, Serializable]
    public partial class EntryCreate :  IEquatable<EntryCreate>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryCreate" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected EntryCreate() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryCreate" /> class.
        /// </summary>
        /// <param name="contentHash">contentHash (required).</param>
        /// <param name="contentSize">contentSize (required).</param>
        /// <param name="contentType">contentType.</param>
        /// <param name="labels">labels.</param>
        /// <param name="metadata">metadata.</param>
        /// <param name="path">path (required).</param>
        public EntryCreate(string contentHash = default(string), int? contentSize = default(int?), string contentType = default(string), List<string> labels = default(List<string>), Object metadata = default(Object), string path = default(string))
        {
            // to ensure "contentHash" is required (not null)
            if (contentHash == null)
            {
                throw new InvalidDataException("contentHash is a required property for EntryCreate and cannot be null");
            }
            else
            {
                this.ContentHash = contentHash;
            }
            // to ensure "contentSize" is required (not null)
            if (contentSize == null)
            {
                throw new InvalidDataException("contentSize is a required property for EntryCreate and cannot be null");
            }
            else
            {
                this.ContentSize = contentSize;
            }
            // to ensure "path" is required (not null)
            if (path == null)
            {
                throw new InvalidDataException("path is a required property for EntryCreate and cannot be null");
            }
            else
            {
                this.Path = path;
            }
            this.ContentType = contentType;
            this.Labels = labels;
            this.Metadata = metadata;
        }
        
        /// <summary>
        /// Gets or Sets ContentHash
        /// </summary>
        [DataMember(Name="content_hash", EmitDefaultValue=false)]
        public string ContentHash { get; set; }

        /// <summary>
        /// Gets or Sets ContentSize
        /// </summary>
        [DataMember(Name="content_size", EmitDefaultValue=false)]
        public int? ContentSize { get; set; }

        /// <summary>
        /// Gets or Sets ContentType
        /// </summary>
        [DataMember(Name="content_type", EmitDefaultValue=false)]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or Sets Labels
        /// </summary>
        [DataMember(Name="labels", EmitDefaultValue=false)]
        public List<string> Labels { get; set; }

        /// <summary>
        /// Gets or Sets Metadata
        /// </summary>
        [DataMember(Name="metadata", EmitDefaultValue=false)]
        public Object Metadata { get; set; }

        /// <summary>
        /// Gets or Sets Path
        /// </summary>
        [DataMember(Name="path", EmitDefaultValue=false)]
        public string Path { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EntryCreate {\n");
            sb.Append("  ContentHash: ").Append(ContentHash).Append("\n");
            sb.Append("  ContentSize: ").Append(ContentSize).Append("\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  Labels: ").Append(Labels).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  Path: ").Append(Path).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as EntryCreate);
        }

        /// <summary>
        /// Returns true if EntryCreate instances are equal
        /// </summary>
        /// <param name="input">Instance of EntryCreate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EntryCreate input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ContentHash == input.ContentHash ||
                    (this.ContentHash != null &&
                    this.ContentHash.Equals(input.ContentHash))
                ) && 
                (
                    this.ContentSize == input.ContentSize ||
                    (this.ContentSize != null &&
                    this.ContentSize.Equals(input.ContentSize))
                ) && 
                (
                    this.ContentType == input.ContentType ||
                    (this.ContentType != null &&
                    this.ContentType.Equals(input.ContentType))
                ) && 
                (
                    this.Labels == input.Labels ||
                    this.Labels != null &&
                    this.Labels.SequenceEqual(input.Labels)
                ) && 
                (
                    this.Metadata == input.Metadata ||
                    (this.Metadata != null &&
                    this.Metadata.Equals(input.Metadata))
                ) && 
                (
                    this.Path == input.Path ||
                    (this.Path != null &&
                    this.Path.Equals(input.Path))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.ContentHash != null)
                    hashCode = hashCode * 59 + this.ContentHash.GetHashCode();
                if (this.ContentSize != null)
                    hashCode = hashCode * 59 + this.ContentSize.GetHashCode();
                if (this.ContentType != null)
                    hashCode = hashCode * 59 + this.ContentType.GetHashCode();
                if (this.Labels != null)
                    hashCode = hashCode * 59 + this.Labels.GetHashCode();
                if (this.Metadata != null)
                    hashCode = hashCode * 59 + this.Metadata.GetHashCode();
                if (this.Path != null)
                    hashCode = hashCode * 59 + this.Path.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            // ContentHash (string) pattern
            Regex regexContentHash = new Regex(@"^[a-fA-F0-9]+$", RegexOptions.CultureInvariant);
            if (false == regexContentHash.Match(this.ContentHash).Success)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ContentHash, must match a pattern of " + regexContentHash, new [] { "ContentHash" });
            }

            // ContentSize (int?) minimum
            if(this.ContentSize < (int?)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ContentSize, must be a value greater than or equal to 0.", new [] { "ContentSize" });
            }

            // Path (string) maxLength
            if(this.Path != null && this.Path.Length > 65535)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Path, length must be less than 65535.", new [] { "Path" });
            }

            // Path (string) pattern
            Regex regexPath = new Regex(@"^\\S.*$", RegexOptions.CultureInvariant);
            if (false == regexPath.Match(this.Path).Success)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Path, must match a pattern of " + regexPath, new [] { "Path" });
            }

            yield break;
        }
    }

}
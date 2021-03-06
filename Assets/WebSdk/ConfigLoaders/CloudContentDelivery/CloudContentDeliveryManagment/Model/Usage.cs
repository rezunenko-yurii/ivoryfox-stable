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
    /// Usage
    /// </summary>
    [DataContract,Serializable]
    public partial class Usage :  IEquatable<Usage>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Usage" /> class.
        /// </summary>
        /// <param name="projectguid">projectguid.</param>
        /// <param name="quantity">quantity.</param>
        public Usage(Guid? projectguid = default(Guid?), decimal? quantity = default(decimal?))
        {
            this.Projectguid = projectguid;
            this.Quantity = quantity;
        }
        
        /// <summary>
        /// Gets or Sets Projectguid
        /// </summary>
        [DataMember(Name="projectguid", EmitDefaultValue=false)]
        public Guid? Projectguid { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        [DataMember(Name="quantity", EmitDefaultValue=false)]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Usage {\n");
            sb.Append("  Projectguid: ").Append(Projectguid).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
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
            return this.Equals(input as Usage);
        }

        /// <summary>
        /// Returns true if Usage instances are equal
        /// </summary>
        /// <param name="input">Instance of Usage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Usage input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Projectguid == input.Projectguid ||
                    (this.Projectguid != null &&
                    this.Projectguid.Equals(input.Projectguid))
                ) && 
                (
                    this.Quantity == input.Quantity ||
                    (this.Quantity != null &&
                    this.Quantity.Equals(input.Quantity))
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
                if (this.Projectguid != null)
                    hashCode = hashCode * 59 + this.Projectguid.GetHashCode();
                if (this.Quantity != null)
                    hashCode = hashCode * 59 + this.Quantity.GetHashCode();
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
            // Quantity (decimal?) minimum
            if(this.Quantity < (decimal?)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Quantity, must be a value greater than or equal to 0.", new [] { "Quantity" });
            }

            yield break;
        }
    }

}

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
    /// Release
    /// </summary>
    [DataContract, Serializable]
    public partial class Release :  IEquatable<Release>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Release" /> class.
        /// </summary>
        /// <param name="badges">badges.</param>
        /// <param name="changes">changes.</param>
        /// <param name="contentHash">contentHash.</param>
        /// <param name="contentSize">contentSize.</param>
        /// <param name="created">created.</param>
        /// <param name="createdBy">createdBy.</param>
        /// <param name="createdByName">createdByName.</param>
        /// <param name="entriesLink">entriesLink.</param>
        /// <param name="metadata">metadata.</param>
        /// <param name="notes">notes.</param>
        /// <param name="promotedFromBucket">promotedFromBucket.</param>
        /// <param name="promotedFromRelease">promotedFromRelease.</param>
        /// <param name="releaseid">releaseid.</param>
        /// <param name="releasenum">releasenum.</param>
        public Release(List<Badge> badges = default(List<Badge>), Changecount changes = default(Changecount), string contentHash = default(string), int? contentSize = default(int?), DateTime? created = default(DateTime?), string createdBy = default(string), string createdByName = default(string), string entriesLink = default(string), Object metadata = default(Object), string notes = default(string), Guid? promotedFromBucket = default(Guid?), Guid? promotedFromRelease = default(Guid?), Guid? releaseid = default(Guid?), int? releasenum = default(int?))
        {
            this.Badges = badges;
            this.Changes = changes;
            this.ContentHash = contentHash;
            this.ContentSize = contentSize;
            this.Created = created;
            this.CreatedBy = createdBy;
            this.CreatedByName = createdByName;
            this.EntriesLink = entriesLink;
            this.Metadata = metadata;
            this.Notes = notes;
            this.PromotedFromBucket = promotedFromBucket;
            this.PromotedFromRelease = promotedFromRelease;
            this.Releaseid = releaseid;
            this.Releasenum = releasenum;
        }
        
        /// <summary>
        /// Gets or Sets Badges
        /// </summary>
        [DataMember(Name="badges", EmitDefaultValue=false)]
        public List<Badge> Badges { get; set; }

        /// <summary>
        /// Gets or Sets Changes
        /// </summary>
        [DataMember(Name="changes", EmitDefaultValue=false)]
        public Changecount Changes { get; set; }

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
        /// Gets or Sets Created
        /// </summary>
        [DataMember(Name="created", EmitDefaultValue=false)]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="created_by", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets CreatedByName
        /// </summary>
        [DataMember(Name="created_by_name", EmitDefaultValue=false)]
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or Sets EntriesLink
        /// </summary>
        [DataMember(Name="entries_link", EmitDefaultValue=false)]
        public string EntriesLink { get; set; }

        /// <summary>
        /// Gets or Sets Metadata
        /// </summary>
        [DataMember(Name="metadata", EmitDefaultValue=false)]
        public Object Metadata { get; set; }

        /// <summary>
        /// Gets or Sets Notes
        /// </summary>
        [DataMember(Name="notes", EmitDefaultValue=false)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or Sets PromotedFromBucket
        /// </summary>
        [DataMember(Name="promoted_from_bucket", EmitDefaultValue=false)]
        public Guid? PromotedFromBucket { get; set; }

        /// <summary>
        /// Gets or Sets PromotedFromRelease
        /// </summary>
        [DataMember(Name="promoted_from_release", EmitDefaultValue=false)]
        public Guid? PromotedFromRelease { get; set; }

        /// <summary>
        /// Gets or Sets Releaseid
        /// </summary>
        [DataMember(Name="releaseid", EmitDefaultValue=false)]
        public Guid? Releaseid { get; set; }

        /// <summary>
        /// Gets or Sets Releasenum
        /// </summary>
        [DataMember(Name="releasenum", EmitDefaultValue=false)]
        public int? Releasenum { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Release {\n");
            sb.Append("  Badges: ").Append(Badges).Append("\n");
            sb.Append("  Changes: ").Append(Changes).Append("\n");
            sb.Append("  ContentHash: ").Append(ContentHash).Append("\n");
            sb.Append("  ContentSize: ").Append(ContentSize).Append("\n");
            sb.Append("  Created: ").Append(Created).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  CreatedByName: ").Append(CreatedByName).Append("\n");
            sb.Append("  EntriesLink: ").Append(EntriesLink).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  PromotedFromBucket: ").Append(PromotedFromBucket).Append("\n");
            sb.Append("  PromotedFromRelease: ").Append(PromotedFromRelease).Append("\n");
            sb.Append("  Releaseid: ").Append(Releaseid).Append("\n");
            sb.Append("  Releasenum: ").Append(Releasenum).Append("\n");
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
            return this.Equals(input as Release);
        }

        /// <summary>
        /// Returns true if Release instances are equal
        /// </summary>
        /// <param name="input">Instance of Release to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Release input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Badges == input.Badges ||
                    this.Badges != null &&
                    this.Badges.SequenceEqual(input.Badges)
                ) && 
                (
                    this.Changes == input.Changes ||
                    (this.Changes != null &&
                    this.Changes.Equals(input.Changes))
                ) && 
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
                    this.Created == input.Created ||
                    (this.Created != null &&
                    this.Created.Equals(input.Created))
                ) && 
                (
                    this.CreatedBy == input.CreatedBy ||
                    (this.CreatedBy != null &&
                    this.CreatedBy.Equals(input.CreatedBy))
                ) && 
                (
                    this.CreatedByName == input.CreatedByName ||
                    (this.CreatedByName != null &&
                    this.CreatedByName.Equals(input.CreatedByName))
                ) && 
                (
                    this.EntriesLink == input.EntriesLink ||
                    (this.EntriesLink != null &&
                    this.EntriesLink.Equals(input.EntriesLink))
                ) && 
                (
                    this.Metadata == input.Metadata ||
                    (this.Metadata != null &&
                    this.Metadata.Equals(input.Metadata))
                ) && 
                (
                    this.Notes == input.Notes ||
                    (this.Notes != null &&
                    this.Notes.Equals(input.Notes))
                ) && 
                (
                    this.PromotedFromBucket == input.PromotedFromBucket ||
                    (this.PromotedFromBucket != null &&
                    this.PromotedFromBucket.Equals(input.PromotedFromBucket))
                ) && 
                (
                    this.PromotedFromRelease == input.PromotedFromRelease ||
                    (this.PromotedFromRelease != null &&
                    this.PromotedFromRelease.Equals(input.PromotedFromRelease))
                ) && 
                (
                    this.Releaseid == input.Releaseid ||
                    (this.Releaseid != null &&
                    this.Releaseid.Equals(input.Releaseid))
                ) && 
                (
                    this.Releasenum == input.Releasenum ||
                    (this.Releasenum != null &&
                    this.Releasenum.Equals(input.Releasenum))
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
                if (this.Badges != null)
                    hashCode = hashCode * 59 + this.Badges.GetHashCode();
                if (this.Changes != null)
                    hashCode = hashCode * 59 + this.Changes.GetHashCode();
                if (this.ContentHash != null)
                    hashCode = hashCode * 59 + this.ContentHash.GetHashCode();
                if (this.ContentSize != null)
                    hashCode = hashCode * 59 + this.ContentSize.GetHashCode();
                if (this.Created != null)
                    hashCode = hashCode * 59 + this.Created.GetHashCode();
                if (this.CreatedBy != null)
                    hashCode = hashCode * 59 + this.CreatedBy.GetHashCode();
                if (this.CreatedByName != null)
                    hashCode = hashCode * 59 + this.CreatedByName.GetHashCode();
                if (this.EntriesLink != null)
                    hashCode = hashCode * 59 + this.EntriesLink.GetHashCode();
                if (this.Metadata != null)
                    hashCode = hashCode * 59 + this.Metadata.GetHashCode();
                if (this.Notes != null)
                    hashCode = hashCode * 59 + this.Notes.GetHashCode();
                if (this.PromotedFromBucket != null)
                    hashCode = hashCode * 59 + this.PromotedFromBucket.GetHashCode();
                if (this.PromotedFromRelease != null)
                    hashCode = hashCode * 59 + this.PromotedFromRelease.GetHashCode();
                if (this.Releaseid != null)
                    hashCode = hashCode * 59 + this.Releaseid.GetHashCode();
                if (this.Releasenum != null)
                    hashCode = hashCode * 59 + this.Releasenum.GetHashCode();
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
            yield break;
        }
    }

}

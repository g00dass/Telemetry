/*
 * Statistics API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Telemetry.Client.Client.OpenAPIDateConverter;

namespace Telemetry.Client.Model
{
    /// <summary>
    /// AppInfo
    /// </summary>
    [DataContract(Name = "AppInfo")]
    public partial class AppInfo : IEquatable<AppInfo>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppInfo" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="appVersion">appVersion.</param>
        /// <param name="userName">userName.</param>
        /// <param name="osName">osName.</param>
        /// <param name="lastUpdatedAt">lastUpdatedAt.</param>
        public AppInfo(Guid id = default(Guid), string appVersion = default(string), string userName = default(string), string osName = default(string), DateTime lastUpdatedAt = default(DateTime))
        {
            this.Id = id;
            this.AppVersion = appVersion;
            this.UserName = userName;
            this.OsName = osName;
            this.LastUpdatedAt = lastUpdatedAt;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets AppVersion
        /// </summary>
        [DataMember(Name = "appVersion", EmitDefaultValue = true)]
        public string AppVersion { get; set; }

        /// <summary>
        /// Gets or Sets UserName
        /// </summary>
        [DataMember(Name = "userName", EmitDefaultValue = true)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or Sets OsName
        /// </summary>
        [DataMember(Name = "osName", EmitDefaultValue = true)]
        public string OsName { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedAt
        /// </summary>
        [DataMember(Name = "lastUpdatedAt", EmitDefaultValue = false)]
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AppInfo {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  AppVersion: ").Append(AppVersion).Append("\n");
            sb.Append("  UserName: ").Append(UserName).Append("\n");
            sb.Append("  OsName: ").Append(OsName).Append("\n");
            sb.Append("  LastUpdatedAt: ").Append(LastUpdatedAt).Append("\n");
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
            return this.Equals(input as AppInfo);
        }

        /// <summary>
        /// Returns true if AppInfo instances are equal
        /// </summary>
        /// <param name="input">Instance of AppInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AppInfo input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.AppVersion == input.AppVersion ||
                    (this.AppVersion != null &&
                    this.AppVersion.Equals(input.AppVersion))
                ) && 
                (
                    this.UserName == input.UserName ||
                    (this.UserName != null &&
                    this.UserName.Equals(input.UserName))
                ) && 
                (
                    this.OsName == input.OsName ||
                    (this.OsName != null &&
                    this.OsName.Equals(input.OsName))
                ) && 
                (
                    this.LastUpdatedAt == input.LastUpdatedAt ||
                    (this.LastUpdatedAt != null &&
                    this.LastUpdatedAt.Equals(input.LastUpdatedAt))
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
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.AppVersion != null)
                    hashCode = hashCode * 59 + this.AppVersion.GetHashCode();
                if (this.UserName != null)
                    hashCode = hashCode * 59 + this.UserName.GetHashCode();
                if (this.OsName != null)
                    hashCode = hashCode * 59 + this.OsName.GetHashCode();
                if (this.LastUpdatedAt != null)
                    hashCode = hashCode * 59 + this.LastUpdatedAt.GetHashCode();
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
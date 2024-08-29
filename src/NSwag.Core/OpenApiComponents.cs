//-----------------------------------------------------------------------
// <copyright file="OpenApiComponents.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NJsonSchema;
using NSwag.Collections;
using System;

namespace NSwag
{
    /// <summary>Container for reusable components (OpenAPI only).</summary>
    // [JsonConverter(typeof(OpenApiComponentsConverter))]
    public class OpenApiComponents : JsonExtensionObject
    {
        /// <summary></summary>
        /// <param name="document"></param>
        public OpenApiComponents(OpenApiDocument document)
        {
            var schemas = new ObservableDictionary<string, JsonSchema>();
            schemas.CollectionChanged += (sender, args) =>
            {
                foreach (var pair in schemas.ToArray())
                {
                    if (pair.Value == null)
                    {
                        schemas.Remove(pair.Key);
                    }
                    else
                    {
                        pair.Value.Parent = this;
                    }
                }
            };
            Schemas = schemas;

            var requestBodies = new ObservableDictionary<string, OpenApiRequestBody>();
            requestBodies.CollectionChanged += (sender, args) =>
            {
                foreach (var path in RequestBodies.Values)
                {
                    path.Parent = document;
                }
            };
            RequestBodies = requestBodies;

            var responses = new ObservableDictionary<string, OpenApiResponse>();
            responses.CollectionChanged += (sender, args) =>
            {
                foreach (var path in Responses.Values)
                {
                    path.Parent = document;
                }
            };
            Responses = responses;

            var parameters = new ObservableDictionary<string, OpenApiParameter>();
            parameters.CollectionChanged += (sender, args) =>
            {
                foreach (var path in Parameters.Values)
                {
                    path.Parent = document;
                }
            };
            Parameters = parameters;

            Examples = new Dictionary<string, OpenApiExample>();

            var headers = new ObservableDictionary<string, OpenApiParameter>();
            headers.CollectionChanged += (sender, args) =>
            {
                foreach (var pair in headers.ToArray())
                {
                    if (pair.Value == null)
                    {
                        headers.Remove(pair.Key);
                    }
                    else
                    {
                        pair.Value.Parent = this;
                    }
                }
            };
            Headers = headers;

            SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>();
            Links = new Dictionary<string, OpenApiLink>();
            Callbacks = new Dictionary<string, OpenApiCallback>();
        }

        /// <summary>Gets or sets the types.</summary>
        [JsonProperty(PropertyName = "schemas", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, JsonSchema> Schemas { get; }

        /// <summary>Gets or sets the responses which can be used for all operations.</summary>
        [JsonProperty(PropertyName = "requestBodies", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiRequestBody> RequestBodies { get; }

        /// <summary>Gets or sets the responses which can be used for all operations.</summary>
        [JsonProperty(PropertyName = "responses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiResponse> Responses { get; }

        /// <summary>Gets or sets the parameters which can be used for all operations.</summary>
        [JsonProperty(PropertyName = "parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiParameter> Parameters { get; }

        /// <summary>Gets or sets the headers.</summary>
        [JsonProperty(PropertyName = "examples", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiExample> Examples { get; set; }

        /// <summary>Gets or sets the types.</summary>
        [JsonProperty(PropertyName = "headers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiParameter> Headers { get; }

        /// <summary>Gets or sets the security definitions.</summary>
        [JsonProperty(PropertyName = "securitySchemes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiSecurityScheme> SecuritySchemes { get; }

        /// <summary>Gets or sets the security definitions.</summary>
        [JsonProperty(PropertyName = "links", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiLink> Links { get; }

        /// <summary>Gets or sets the security definitions.</summary>
        [JsonProperty(PropertyName = "callbacks", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OpenApiCallback> Callbacks { get; }

        /*
        /// <summary>Gets or sets the extension data (i.e. additional properties which are not directly defined by the JSON object).</summary>
        [JsonExtensionData]
        public IDictionary<string, object> ExtensionData { get; set; }

        internal class OpenApiComponentsConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var components = (OpenApiComponents)value;
                writer.WriteStartObject();

                if (components.Schemas != null && components.Schemas.Any())
                {
                    writer.WritePropertyName("schemas");
                    serializer.Serialize(writer, components.Schemas);
                }

                if (components.RequestBodies != null && components.RequestBodies.Any())
                {
                    writer.WritePropertyName("requestBodies");
                    serializer.Serialize(writer, components.RequestBodies);
                }

                if (components.Responses != null && components.Responses.Any())
                {
                    writer.WritePropertyName("responses");
                    serializer.Serialize(writer, components.Responses);
                }

                if (components.Parameters != null && components.Parameters.Any())
                {
                    writer.WritePropertyName("parameters");
                    serializer.Serialize(writer, components.Parameters);
                }

                if (components.Examples != null && components.Examples.Any())
                {
                    writer.WritePropertyName("examples");
                    serializer.Serialize(writer, components.Examples);
                }

                if (components.Headers != null && components.Headers.Any())
                {
                    writer.WritePropertyName("headers");
                    serializer.Serialize(writer, components.Headers);
                }

                if (components.SecuritySchemes != null && components.SecuritySchemes.Any())
                {
                    writer.WritePropertyName("securitySchemes");
                    serializer.Serialize(writer, components.SecuritySchemes);
                }

                if (components.Links != null && components.Links.Any())
                {
                    writer.WritePropertyName("links");
                    serializer.Serialize(writer, components.Links);
                }

                if (components.Callbacks != null && components.Callbacks.Any())
                {
                    writer.WritePropertyName("callbacks");
                    serializer.Serialize(writer, components.Callbacks);
                }

                if (components.ExtensionData != null)
                {
                    foreach (var tuple in components.ExtensionData)
                    {
                        writer.WritePropertyName(tuple.Key);
                        serializer.Serialize(writer, tuple.Value);
                    }
                }

                writer.WriteEndObject();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }

                var components = new OpenApiComponents();
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(OpenApiComponents);
            }
        }
        */
    }
}
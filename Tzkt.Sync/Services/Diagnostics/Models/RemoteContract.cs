﻿using System.Text.Json.Serialization;

namespace Tzkt.Sync.Services.Diagnostics
{
    class RemoteContract
    {
        [JsonPropertyName("balance")]
        public long? Balance { get; set; }

        [JsonPropertyName("delegate")]
        public RemoteContractDelegate Delegate { get; set; }

        [JsonPropertyName("counter")]
        public long? Counter { get; set; }

        #region validation
        public bool IsValidFormat() =>
            Balance != null &&
            Delegate?.IsValidFormat() == true &&
            Counter != null;
        #endregion
    }

    class RemoteContractDelegate
    {
        [JsonPropertyName("setable")]
        public bool? Setable { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        #region validation
        public bool IsValidFormat() =>
            Setable != null &&
            (Value == null || Value != "");
        #endregion
    }
}
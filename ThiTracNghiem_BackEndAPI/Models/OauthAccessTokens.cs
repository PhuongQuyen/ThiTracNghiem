using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class OauthAccessTokens
    {
        public string Id { get; set; }
        public long? UserId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Scopes { get; set; }
        public byte Revoked { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

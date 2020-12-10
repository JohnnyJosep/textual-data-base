using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Persistence.Configurations
{
    public class TextEntryConfiguration : IEntityTypeConfiguration<TextEntry>
    {
        public void Configure(EntityTypeBuilder<TextEntry> builder)
        {
            var valueComparer = new ValueComparer<IDictionary<string, string>>(
                (kv1, kv2) => kv1.SequenceEqual(kv2),
                kv => kv.Aggregate(0, (a, vkv) => HashCode.Combine(a, vkv.GetHashCode())),
                kv => kv.ToDictionary(k => k.Key, v => v.Value));
            
            builder
                .Property(t => t.Metadata)
                .HasConversion(
                    v => Zip(v),
                    v => Unzip(v))
                .Metadata.SetValueComparer(valueComparer);
        }
        
        private static void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        private static byte[] Zip(IDictionary<string, string> metadata)
        {
            var json = JsonSerializer.Serialize(metadata);
            var bytes = Encoding.UTF8.GetBytes(json);
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }

        private static IDictionary<string, string> Unzip(byte[] bytes)
        {
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                CopyTo(gs, mso);
            }

            var json = Encoding.UTF8.GetString(mso.ToArray());
            var metadata = JsonSerializer.Deserialize<IDictionary<string, string>>(json);
            return metadata;
        }
    }
}
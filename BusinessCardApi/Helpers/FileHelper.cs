using BusinessCardApi.Models;
using System.Xml.Serialization;

namespace BusinessCardApi.Helpers
{
    public static class FileHelper
    {
        public static async Task<(bool Success, string? Base64, string? ErrorMessage, string? FileName)> TryConvertPhotoToBase64Async(IFormFile? photo, int maxSizeInBytes = 1024 * 1024)
        {
            if (photo == null)
                return (false, null, "No photo provided.", photo.FileName);

            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            if (fileBytes.Length > maxSizeInBytes)
                return (false, null, "Photo exceeds 1MB.", photo.FileName);

            string base64 = Convert.ToBase64String(fileBytes);
            return (true, base64, null, photo.FileName);
        }

        public static List<BusinessCard> ParseCsv(Stream stream)
        {
            var list = new List<BusinessCard>();
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var cols = line?.Split(',');
                if (cols?.Length >= 6)
                {
                    list.Add(new BusinessCard
                    {
                        Name = cols[0],
                        Gender = cols[1],
                        DateOfBirth = DateTime.TryParse(cols[2], out var dob) ? dob : DateTime.MinValue,
                        Email = cols[3],
                        Phone = cols[4],
                        Address = cols[5],
                    });
                }
            }
            return list;
        }

        public static List<BusinessCard> ParseXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<BusinessCard>));
            var result = serializer.Deserialize(stream) as List<BusinessCard> ?? new List<BusinessCard>();
            return result;
        }
    }
}

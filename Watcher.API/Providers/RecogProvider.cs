using SixLabors.ImageSharp;
using Watcher.Data.Entities;
using Watcher.Data.SQL;

namespace Watcher.API.Providers
{
    public interface IRecogProvider
    {
        Task<bool> PersonDetected(Stream stream);
    }

    public class RecogProvider : IRecogProvider
    {
        private ISqlUnitOfWork _sql;

        public RecogProvider(ISqlUnitOfWork sqlUnitOfWork)
        {
            _sql = sqlUnitOfWork;
        }

        public async Task<bool> PersonDetected(Stream stream)
        {
            using var img = Image.Load(stream);

            var faceEncoding = ExtractFaceEncoding(img);
            if (faceEncoding == null) return false;

            var people = await _sql.People.GetAllAsync();

            int? matchedPersonId = null;
            foreach (var person in people)
            {
                var similarity = CalculateSimilarity(faceEncoding, person.FaceEmbedding);
                if (similarity < 0.6) // Threshold
                {
                    matchedPersonId = person.Id;
                    break;
                }
            }

            if (matchedPersonId == null)
            {
                var newPerson = new Person { FaceEmbedding = faceEncoding };
                await _sql.People.AddAsync(newPerson);
                matchedPersonId = newPerson.Id;
            }

            var newEvent = new EventLog { PersonId = matchedPersonId.Value };
            await _sql.Events.AddAsync(newEvent);
            return true;
        }

        private byte[] ExtractFaceEncoding(Image img)
        {
            // Use OpenCV or ML.NET to extract face embeddings
            return new byte[128]; // Dummy placeholder, replace with actual embedding logic
        }

        private double CalculateSimilarity(byte[] enc1, byte[] enc2)
        {
            // Calculate cosine similarity between encodings
            return 0.5; // Dummy placeholder
        }
    }
}

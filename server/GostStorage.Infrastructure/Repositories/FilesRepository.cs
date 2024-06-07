using GostStorage.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using IMinioClientFactory = Minio.AspNetCore.IMinioClientFactory;

namespace GostStorage.Infrastructure.Repositories;

public class FilesRepository(IFieldsRepository fieldsRepository, IMinioClientFactory minioClientFactory) : IFilesRepository
{
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IMinioClient _minioClient = minioClientFactory.CreateClient();
    
    public async Task UploadFileAsync(IFormFile file, string extension, long docId)
    {
        var primary = (await _fieldsRepository.GetFieldsByDocIds(new List<long> { docId })).First(f => f.IsPrimary);
        if (file.Length > 0)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var putObjectArgs = new PutObjectArgs()
                .WithBucket("documents")
                .WithObject(primary.Designation + '.' + extension)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream");
            
            _ = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
    }
}
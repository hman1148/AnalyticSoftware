using Amazon.S3;
using Amazon.S3.Model;

namespace AnalyticSoftware.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        
        public S3Service(string accessKey, string secretKey, string region)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(region));  
        }

        public async Task<bool> CreateBucketAsync(string bucketName)
        {
            try
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true,
                };

                var response = await _s3Client.PutBucketAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            } catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

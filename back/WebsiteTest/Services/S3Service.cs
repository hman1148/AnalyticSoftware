using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace AnalyticSoftware.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private static readonly RegionEndpoint BUCKET_REGION = RegionEndpoint.USEast2;
        private string _bucketName;
        
        public S3Service(string accessKey, string secretKey, string region)
        {
            DotNetEnv.Env.Load();

            _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME") ?? "";

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(region));  
        }

       public async Task<DataResponse<string>> CreateS3Folder(string folderName)
        {
            if (DoesFolderExist(folderName).Result)
            {
                DataResponse<string> folderExists = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = "Folder already exists, contact your Administrator",
                    Success = false
                };
                return folderExists;
            }

            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = folderName,
                    ContentBody = string.Empty,
                };

                PutObjectResponse response = await _s3Client.PutObjectAsync(putRequest);

                DataResponse<string> successResponse = new DataResponse<string>
                {
                    Data = folderName,
                    Message = $"Folder {folderName} was successfully added!",
                    Success = true
                };
                return successResponse;
            } catch (Exception ex)
            {
                DataResponse<string> failedUploadResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = $"Failed to create folder: {folderName} and got the following error: {ex.Message}",
                    Success = false
                };
                return failedUploadResponse;
            }
        }

        public async Task<DataResponse<string>> UploadFile(string bucketName, string filePath)
        {
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(_s3Client);

                await fileTransferUtility.UploadAsync(filePath, bucketName);

                DataResponse<string> successResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = "Sucessfully uploaded file!",
                    Success = true
                };
                return successResponse;
            } catch (Exception ex)
            {
                DataResponse<string> failedUploadResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = $"Failed to upload file and got back error: {ex.Message}",
                    Success = false
                };
                return failedUploadResponse;
            }
        }

        private async Task<bool> DoesFolderExist(string folderName)
        {
            string updatedFolderName = folderName + "/";
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = updatedFolderName,
                    MaxKeys = 1
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                return response.S3Objects.Count > 0;
            } catch (Exception ex)
            {
                throw new Exception("Couldn't perform search");
            }
        }
    }
}

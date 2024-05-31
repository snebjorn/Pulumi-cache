using Pulumi;
using Ecrx = Pulumi.Awsx.Ecr;

return await Deployment.RunAsync(() =>
{
    var ecrRepo = new Ecrx.Repository("ecr");

    var imageTag = "latest";
    var dockerImage = new Ecrx.Image(
        $"docker-image",
        new()
        {
            ImageTag = imageTag,
            // remember to login in to the ECR in docker for CacheFrom to work
            // See https://docs.aws.amazon.com/AmazonECR/latest/userguide/registry_auth.html
            // aws ecr get-login-password --region eu-west-1 | docker login --username AWS --password-stdin <aws_account_id>.dkr.ecr.<region>.amazonaws.com
            CacheFrom = { ecrRepo.Url.Apply(url => $"{url}:{imageTag}") },
            RepositoryUrl = ecrRepo.Url,
            Context = "../App",
            Platform = "linux/amd64",
        }
    );
});

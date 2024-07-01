using System;
using System.Threading.Tasks;

namespace OPMFileUploader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int i = 0;

            Console.WriteLine(Environment.CommandLine);

            try
            {
                var uploader = new FileUploader(
                   uploadUrl: args[i++],
                   authUrl: args[i++],
                   uploadFilePath: args[i++],
                   loginId: args[i++],
                   password: args[i++]);

                uploader.FileUploadProgress += (x) => Console.WriteLine($"Upload progress: {x:d2}");

                Console.WriteLine(await uploader.Run());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

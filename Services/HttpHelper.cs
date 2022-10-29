using System.Net.Http.Headers;

namespace WeatherCore.Services
{
    public static class HttpHelper
    {
        public static HttpClient Client { get; set; } = null!;

        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public static async Task<HttpContent> GetContentAsync(string url)
        {
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return response.Content;
        }
        public static async Task GetFileAsync(string url, Stream destination, IProgress<double> progress, CancellationToken? token = null)
        {
            var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var total = response.Content.Headers.ContentLength ?? -1L;

            using var source = await response.Content.ReadAsStreamAsync();

            await CopyStreamWithProgressAsync(source, destination, total, progress);
        }
        private static async Task CopyStreamWithProgressAsync(Stream input, Stream output, long total, IProgress<double> progress, CancellationToken? token = null)
        {
            const int IO_BUFFER_SIZE = 8 * 1024;

            var canReportProgress = total != -1 && progress != null;
            var totalRead = 0L;
            byte[] buffer = new byte[IO_BUFFER_SIZE];
            int read;

            while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                //token.ThrowIfCancellationRequested();
                await output.WriteAsync(buffer, 0, read);
                totalRead += read;
                if (canReportProgress)
                    progress?.Report((totalRead * 1d) / (total * 1d) * 100);
            }
        }
    }
}

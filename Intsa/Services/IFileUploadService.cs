using BlazorInputFile;
using System.Threading.Tasks;

namespace Intsa.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(IFileListEntry file);
    }
}

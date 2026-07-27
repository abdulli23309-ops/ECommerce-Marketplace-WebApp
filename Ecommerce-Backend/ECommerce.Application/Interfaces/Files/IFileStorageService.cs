namespace ECommerce.Application.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves the file stream to the configured storage location and returns the
        /// relative URL (e.g., /uploads/products/abc123.jpg).
        /// Throws ArgumentException if the file extension is not allowed.
        /// </summary>
        Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string subFolder);

        /// <summary>
        /// Deletes the file at the given relative URL (if it exists).
        /// </summary>
        void DeleteFile(string relativeUrl);
    }
}
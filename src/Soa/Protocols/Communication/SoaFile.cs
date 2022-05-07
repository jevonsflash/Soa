namespace Soa.Protocols.Communication
{
    /// <summary>
    ///     transfer file, use in  upload and download file
    /// </summary>
    public class SoaFile
    {
        public SoaFile()
        {
        }

        public SoaFile(string name, byte[] data)
        {
            FileName = name;
            Data = data;
        }

        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
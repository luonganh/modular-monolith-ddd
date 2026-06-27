namespace ModularMonolithDDD.BuildingBlocks.Application.Queries
{
    /// <summary>
    /// SQL Parameters – Translate Page/PerPage into OFFSET and FETCH in SQL.
    /// Performance – Leverage OFFSET/FETCH for efficient pagination (instead of TOP).
    /// Memory Efficiency – Retrieve only the necessary rows.
    /// </summary>
    public struct PageData
    {
        /// <summary>
        /// The offset.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// The next.
        /// </summary>
        public int Next { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageData"/> struct.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="next">The next.</param>
        public PageData(int offset, int next)
        {
            Offset = offset;
            Next = next;
        }
    }
}